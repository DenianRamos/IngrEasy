using IngrEasy.API.Filters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace IngrEasy.API.Attributes;

public class AuthenticatedUser : TypeFilterAttribute
{
    public AuthenticatedUser() : base(typeof(AuthentificatedUserFilter))
    {
    }
}