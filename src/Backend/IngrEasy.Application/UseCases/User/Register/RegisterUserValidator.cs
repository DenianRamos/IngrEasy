using FluentValidation;
using IngrEasy.Application.SharedValidators;
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
        RuleFor(x => x.Password).SetValidator(new PasswordValidator<RequestRegisterUserJson>());
    }
}