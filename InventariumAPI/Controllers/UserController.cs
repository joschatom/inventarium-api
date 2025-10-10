using AutoMapper;
using AutoMapper.Configuration.Annotations;
using InventariumAPI.Data;
using InventariumAPI.DTOs.Lendout;
using InventariumAPI.Interfaces;
using InventariumAPI.Models;
using InventariumAPI.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.InteropServices;

namespace InventariumAPI.Controllers;

[Route("api/user")]
[ApiController]
public class UserController(IUserRepository _repository, IMapper _mapper)
    : BaseController<Models.User, TModelId,
        DTOs.User.UserDTO,
        DTOs.User.CreateUserDTO,
        DTOs.User.UpdateUserDTO>
    (_repository, _mapper)
{
    private readonly IUserRepository repository = _repository;
    private readonly IMapper mapper = _mapper;
    [HttpGet("{id}/promote")]
    public async Task Promote(TModelId id)
        => await repository.Promote(id);
   

    [HttpGet("{id}/demote")]
    public async Task Demote(TModelId id)
        => await repository.Demote(id);
}       
