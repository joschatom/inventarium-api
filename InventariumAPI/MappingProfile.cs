using AutoMapper;
using InventariumAPI.Interfaces;

namespace InventariumAPI;

public class MappingProfile: Profile
{
    public MappingProfile()
    {
        MapDtoTypes<DTOs.Object.ObjectDTO>();
    }

    public void MapDtoTypes<T>()
        where T : class, IDtoTypes
    {
        CreateMap(T.CreateDTO, T.BaseType);
        CreateMap(T.BaseDTO, T.BaseType)
            .ReverseMap();
        CreateMap(T.UpdateDTO, T.BaseType)
            .ForAllMembers(m => m.Condition(
                (source, destination, sourceMember) => sourceMember is not null)
            );

    }
}
