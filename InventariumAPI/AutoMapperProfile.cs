using AutoMapper;
using AutoMapper.Internal;
using Bogus.Bson;
using InventariumAPI.Data;
using InventariumAPI.Interfaces;
using Microsoft.AspNetCore.Hosting.Builder;
using System.Diagnostics;
using System.Numerics;
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
        MapDtoTypes<DTOs.Lendout.LendoutDTO>();
        CreateMap<DTOs.Object.BrokenObjectDTO, Models.BrokenObject>()
            .ReverseMap();
   
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
                m.MapAtRuntime();
           
                m.Condition(
                    (source, destination, sourceMember) => {
                        

                        Console.WriteLine(
                            $"Mapping member of type {sourceMember?.GetType()} if {sourceMember != null} to {destination} with value {sourceMember}");
                        return sourceMember != null 
                        && (sourceMember is not int n || n != 0) 
                        && source != null;
                    });
                }
                
            );

    }




}
