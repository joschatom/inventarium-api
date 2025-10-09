using AutoMapper;
using InventariumAPI.Data;
using InventariumAPI.DTOs.Lendout;
using InventariumAPI.DTOs.User;
using InventariumAPI.Interfaces;
using InventariumAPI.Middleware;
using InventariumAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Runtime.InteropServices;
using ObjectModel = InventariumAPI.Models.ObjectEntry;

namespace InventariumAPI.Controllers
{
    [Route("api/object")]
    [ApiController]
    public class ObjectController
        (IObjectRepository _repository, 
        IMapper _mapper,
        IUserRepository _userRepository,
        DataContext _context)
        : BaseController<ObjectModel, TModelId,
            DTOs.Object.ObjectDTO,
            DTOs.Object.CreateObjectDTO,
            DTOs.Object.UpdateObjectDTO>
        (_repository, _mapper, _context)
    {
        private readonly IObjectRepository repository = _repository;
        private readonly IMapper mapper = _mapper;

        [HttpGet("{id}/managers")]
        public async Task<FallibleResponse<IEnumerable<UserDTO>>> GetManagers(TModelId id)
            => new(mapper.Map<IEnumerable<UserDTO>>(
                await repository.GetManagers(id)
                ?? throw new KeyNotFoundException($"Object with ID {id} doesn't exists.")));

        [HttpPost("{id}/managers")]
        public async Task<FallibleResponse> AddManager(TModelId id, TModelId userId)
        {
            var obj = await repository.GetAsync(id)
                ?? throw new KeyNotFoundException($"Object with ID {id} doesn't exists.");

            var user = await _userRepository.GetAsync(userId)
                ?? throw new KeyNotFoundException($"User with ID {userId} doesn't exists.");

            if (user.Role < UserRole.Manager)
                throw new InvalidOperationException($"The {user} isn't a manager.");

            if (obj.Managers.Any(m => m.UserId == userId))
                throw new InvalidOperationException($"The {user} is already a manager for {obj}.");

            await repository.AddManager(id, userId);
            await repository.SaveChangesAsync();

            return new();
        }

        [HttpDelete("{id}/managers/{userId}")]
        public async Task<FallibleResponse> RemoveManager(TModelId id, TModelId userId)
        {
            var obj = await repository.GetAsync(id)
                ?? throw new KeyNotFoundException($"Object with ID {id} doesn't exists.");

            var user = await _userRepository.GetAsync(userId)
                ?? throw new KeyNotFoundException($"User with ID {userId} doesn't exists.");

            if (user.Role < UserRole.Manager)
                throw new InvalidOperationException($"The {user} isn't a manager.");

            if (!obj.Managers.Any(m => m.UserId == userId))
                throw new InvalidOperationException($"The {user} is not a manager for {obj}.");

            await repository.RemoveManager(id, userId);
            await repository.SaveChangesAsync();

            return new();
        }

        [HttpGet("{id}/lendout")]
        public async Task<FallibleResponse<LendoutDTO>> GetCurrentLendout(TModelId id)
            => mapper.Map<LendoutDTO>(
                await repository.GetLendout(id)
                ?? throw new KeyNotFoundException($"Lendout for object with ID {id} doesn't exists."));

        /// <summary>
        /// Get reason why an object is broken if applicable.
        /// </summary>
        /// <param name="id">Object ID</param>
        /// <returns>Reason String</returns>
        /// <exception cref="KeyNotFoundException">Object not found</exception>
        [HttpGet("{id}/broken")]
        public async Task<FallibleResponse<string?>> GetAsBrokenObject(TModelId id)
            => (await repository.AsBrokenAsync(id))?.Reason
                 ?? throw new KeyNotFoundException($"Object with ID {id} doesn't exists.");

        /// <summary>
        /// Update the reason for an update to be broken or remove
        /// it as a broken object when given <c>null</c>
        /// </summary>
        /// <param name="id"></param>
        /// <param name="reason">The new reason why the object is broken(min 8 characters)
        /// or <c>null</c> if object is no longer broken.</param>
        /// <returns><c>true</c> if object was removed from 
        /// the broken objects otherwise <c>false</c>
        /// </returns>
        /// <exception cref="KeyNotFoundException">Object not found.</exception>
        [HttpPatch("{id}/broken")]
        public async Task<FallibleResponse<bool>> GetAsBrokenObject(
            TModelId id,
            [FromBody] [MinLength(8)]
            string? reason
            )
        {
            var deleted = await repository.SetBrokenAsync(id, reason);

            _ = deleted ?? throw 
                new KeyNotFoundException($"Object with ID {id} doesn't exists.");

            await repository.SaveChangesAsync();

            return deleted;

            
        }
    }
}
