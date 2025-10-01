using AutoMapper;
using InventariumAPI.Data;
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
    }
}
