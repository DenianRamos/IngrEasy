using FluentValidation;
using IngrEasy.Communication.Requests;
using IngrEasy.Domain.Extensions;
using IngrEasy.Exception;

namespace IngrEasy.Application.UseCases.User.Update;

public class UpdateUserValidator : AbstractValidator<RequestUpdateUserJson>
{

    public UpdateUserValidator()
    {
        RuleFor(request => request.Name).NotEmpty().WithMessage(ResourceErrorMessage.NAME_EMPTY);
        RuleFor(request => request.Email).NotEmpty().WithMessage(ResourceErrorMessage.EMAIL_EMPTY);
        
        When(request => string.IsNullOrWhiteSpace(request.Email).IsFalse(), () =>
        {
            RuleFor(request => request.Email).EmailAddress().WithMessage(ResourceErrorMessage.EMAIL_INVALID);
        });
    }
}