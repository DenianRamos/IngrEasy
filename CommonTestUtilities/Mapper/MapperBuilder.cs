using AutoMapper;
using CommonTestUtilities.IdEncryption;
using IngrEasy.Application.Services.AutoMapper;

namespace CommonTestUtilities.Mapper;

public class MapperBuilder
{
    public static IMapper Build()
    {
        var idEncripter = IdEncripterBuilder.Build();
        var mapper =new MapperConfiguration(opt =>
        {
            opt.AddProfile(new AutoMapping(idEncripter));
        }).CreateMapper();
        return mapper;
    }
}