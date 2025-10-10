using AutoMapper;
using InventariumAPI.Data;
using InventariumAPI.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.InteropServices;

namespace InventariumAPI.Controllers;

[Route("api/category")]
[ApiController]
public class CategoryController(ICategoryRepository _repository,  IMapper _mapper) 
    : BaseController<Models.Category, TModelId,
        DTOs.Category.CategoryDTO,
        DTOs.Category.CreateCategoryDTO,
        DTOs.Category.UpdateCategoryDTO>
    (_repository, _mapper);
