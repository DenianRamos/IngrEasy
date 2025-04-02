using FluentValidation;
using IngrEasy.Application.SharedValidators;
using IngrEasy.Communication.Requests;

namespace IngrEasy.Application.UseCases.User.ChangePassword;

public class ChangePasswordValidator : AbstractValidator<RequestChangePasswordJson>
{
    public ChangePasswordValidator()
    {
        RuleFor(x => x.NewPassword)
            .SetValidator(new PasswordValidator<RequestChangePasswordJson>());
    }
}