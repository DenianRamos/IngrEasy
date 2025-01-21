using AutoMapper;
using IngrEasy.Application.Services.AutoMapper;

namespace CommonTestUtilities.Mapper;

public class MapperBuilder
{
    public static IMapper Build()
    {
       return  new MapperConfiguration(opt =>
        {
            opt.AddProfile(new AutoMapping());
        }).CreateMapper();
    }
}