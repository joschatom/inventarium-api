using AutoMapper;
using InventariumAPI.Data;
using InventariumAPI.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.InteropServices;

namespace InventariumAPI.Controllers
{
    [Route("api/location")]
    [ApiController]
    public class LocationController(ILocationRepository _repository,  IMapper _mapper, DataContext _context) 
        : BaseController<Models.Location, TModelId,
            DTOs.Location.LocationDTO,
            DTOs.Location.CreateLocationDTO,
            DTOs.Location.UpdateLocationDTO>
        (_repository, _mapper, _context)
    {
    }
}
