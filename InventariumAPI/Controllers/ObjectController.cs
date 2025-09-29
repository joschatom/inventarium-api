using AutoMapper;
using InventariumAPI.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.InteropServices;
using ObjectModel = InventariumAPI.Models.ObjectEntry;

namespace InventariumAPI.Controllers
{
    [Route("api/objects")]
    [ApiController]
    public class ObjectController(IObjectRepository _repository,  IMapper _mapper) 
        : BaseController<ObjectModel, int,
            DTOs.Object.ObjectDTO,
            DTOs.Object.CreateObjectDTO,
            DTOs.Object.UpdateObjectDTO>
        (_repository, _mapper)
    {
    }
}
