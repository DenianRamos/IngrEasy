using IngrEasy.API.Filters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace IngrEasy.API.Attributes;

public class AuthentificatedUserAttribute : TypeFilterAttribute
{
    public AuthentificatedUserAttribute() : base(typeof(AuthentificatedUserFilter))
    {
    }
}