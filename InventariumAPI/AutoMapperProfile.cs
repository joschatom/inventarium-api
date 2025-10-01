using AutoMapper;
using AutoMapper.Internal;
using InventariumAPI.Data;
using InventariumAPI.Interfaces;
using Microsoft.AspNetCore.Hosting.Builder;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace InventariumAPI;

public class MapperProfile: Profile
{
    public DataContext? context;

    public MapperProfile()
    {

        MapDtoTypes<DTOs.Category.CategoryDTO>();
        MapDtoTypes<DTOs.Object.ObjectDTO>();
        MapDtoTypes<DTOs.Location.LocationDTO>();
        MapDtoTypes<DTOs.User.UserDTO>();

   
    }

    public void MapDtoTypes<T>()
    where T : class, IDtoTypes
    {
        CreateMap(T.CreateDTO, T.BaseType)
            .ReverseMap();

        CreateMap(T.BaseDTO, T.BaseType)
            .ReverseMap();
        CreateMap(T.UpdateDTO, T.BaseType)
            .ForAllMembers(m => {
                m.Condition(
                    (source, destination, sourceMember) => sourceMember is not null);
                    
                }

            );

    }
}
