using AutoMapper;
using InventariumAPI.Data;
using InventariumAPI.DTOs.Lendout;
using InventariumAPI.DTOs.User;
using InventariumAPI.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.InteropServices;
using ObjectModel = InventariumAPI.Models.ObjectEntry;

namespace InventariumAPI.Controllers
{
    [Route("api/object")]
    [ApiController]
    public class ObjectController(IObjectRepository _repository,  IMapper _mapper, DataContext _context) 
        : BaseController<ObjectModel, TModelId,
            DTOs.Object.ObjectDTO,
            DTOs.Object.CreateObjectDTO,
            DTOs.Object.UpdateObjectDTO>
        (_repository, _mapper, _context)
    {
        private readonly IObjectRepository repository = _repository;
        private readonly IMapper mapper = _mapper;

        [HttpGet("{id}/managers")]
        public async Task<IEnumerable<UserDTO>> GetManagers(TModelId id)
            => mapper.Map<IEnumerable<UserDTO>>(
                await repository.GetManagers(id)
                ?? throw new KeyNotFoundException($"Object with ID {id} doesn't exists."));

        [HttpGet("{id}/lendout")]
        public async Task<LendoutDTO> GetCurrentLendout(TModelId id)
            => mapper.Map<LendoutDTO>(
                await repository.GetLendout(id)
                ?? throw new KeyNotFoundException($"Lendout for object with ID {id} doesn't exists."));
    }
}
