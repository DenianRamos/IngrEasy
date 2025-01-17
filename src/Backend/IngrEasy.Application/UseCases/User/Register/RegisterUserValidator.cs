using FluentValidation;
using IngrEasy.Communication.Requests;
using IngrEasy.Exception;


namespace IngrEasy.Application.UseCases.User.Register;

public class RegisterUserValidator : AbstractValidator<RequestRegisterUserJson>
{
    public RegisterUserValidator()
    {
        RuleFor(x => x.Name).NotEmpty().WithMessage(ResourceErrorMessage.NAME_EMPTY);
        RuleFor(x => x.Email).NotEmpty().WithMessage(ResourceErrorMessage.EMAIL_EMPTY);
        When(user =>
                string.IsNullOrEmpty(user.Email) == false, () =>
            { 
                RuleFor(x => x.Email).EmailAddress().WithMessage(ResourceErrorMessage.EMAIL_INVALID);
            }
        );
        RuleFor(x => x.Password).NotEmpty().WithMessage(ResourceErrorMessage.PASSWORD_EMPTY);
        RuleFor(x => x.Password.Length).GreaterThanOrEqualTo(6).WithMessage(ResourceErrorMessage.PASSWORD_INVALID);
    }
}