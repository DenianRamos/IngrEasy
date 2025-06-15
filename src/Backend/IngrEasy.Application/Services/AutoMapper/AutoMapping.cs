using AutoMapper;
using IngrEasy.Communication.Requests;
using IngrEasy.Communication.Response;
using IngrEasy.Domain;
using IngrEasy.Domain.Entities;
using Sqids;
using DishType = IngrEasy.Communication.Enums.DishType;

namespace IngrEasy.Application.Services.AutoMapper;

public class AutoMapping : Profile
{
    private readonly SqidsEncoder<int> _sqidsEncoder;
    public AutoMapping(SqidsEncoder<int> sqidsEncoder)
    {
        _sqidsEncoder = sqidsEncoder;
        RequestToDomain();
        DomainToResponse();
    }

    public void RequestToDomain()
    {
        CreateMap<RequestRegisterUserJson, User>()
            .ForMember(dest => dest.Password, opt => opt.Ignore());

        CreateMap<RequestRecipeJson, Recipe>()
            .ForMember(dest => dest.Instructions, opt => opt.Ignore())
            .ForMember(dest => dest.Ingredients, opt => opt.MapFrom(src => src.Ingredients.Distinct()))
            .ForMember(dest => dest.DishTypes, opt => opt.MapFrom(src => src.DishTypes.Distinct()));

        CreateMap<string, Ingredient>()
            .ForMember(dest => dest.Item, opt => opt.MapFrom(src => src));
        
        CreateMap<DishType,Domain.Entities.DishType>()
            .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src));
        

        CreateMap<RequestInstructionJson, Instruction>();

    }
    
    public void DomainToResponse()
    {
        CreateMap<User, ResponseUserProfileJson>();
        CreateMap<Recipe, ResponseRegisteredRecipeJson>()
            .ForMember(dest => dest.Id, opt => 
                opt.MapFrom( src => _sqidsEncoder.Encode(src.Id)));

        CreateMap<Recipe, ResponseShortRecipeJson>().ForMember(dest => dest.Id,
                config => config.MapFrom(source => _sqidsEncoder.Encode(source.Id)))
            .ForMember(dest => dest.AmountIngredient,
                config => config.MapFrom(source => source.Ingredients.Count));

    }
}