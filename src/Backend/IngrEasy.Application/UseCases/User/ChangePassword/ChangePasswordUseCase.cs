using IngrEasy.Communication.Requests;
using IngrEasy.Domain;
using IngrEasy.Domain.Extensions;
using IngrEasy.Domain.Repositories.User;
using IngrEasy.Domain.Security.Criptography;
using IngrEasy.Domain.Services.LoggedUser;
using IngrEasy.Exception;
using IngrEasy.Exception.ExceptionBase;

namespace IngrEasy.Application.UseCases.User.ChangePassword;

public class ChangePasswordUseCase : IChangePasswordUseCase
{
    private readonly ILoggedUser _loggedUser;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUpdateUserOnlyRepository _updateUserOnlyRepository;
    private readonly IPasswordEncrypter _passwordEncrypter;

    public ChangePasswordUseCase(ILoggedUser loggedUser, IUnitOfWork unitOfWork, IUpdateUserOnlyRepository updateUserOnlyRepository, IPasswordEncrypter passwordEncrypter)
    {
        _loggedUser = loggedUser;
        _unitOfWork = unitOfWork;
        _updateUserOnlyRepository = updateUserOnlyRepository;
        _passwordEncrypter = passwordEncrypter;
    }
    
    public async Task Execute(RequestChangePasswordJson request)
    {
        var loggedUser = await _loggedUser.User();
        
        Validate(request, loggedUser);
        
        var user = await _updateUserOnlyRepository.GetById(loggedUser.Id);
        
        user.Password = _passwordEncrypter.Encrypt(request.NewPassword);
        
        _updateUserOnlyRepository.Update(user);
        
        await _unitOfWork.Commit();
        
        
    }

    private void Validate(RequestChangePasswordJson request, Domain.User loggedUser)
    {
        var result = new ChangePasswordValidator().Validate(request);

        var currentPasswordEncripted = _passwordEncrypter.Encrypt(request.Password);

        if (currentPasswordEncripted.Equals(loggedUser.Password).IsFalse())
        {
            result.Errors.Add(new FluentValidation.Results.ValidationFailure(string.Empty, ResourceErrorMessage.PASSWORD_DIFFERENT_CURRENT_PASSWORD));
        }
        
        if (result.IsValid.IsFalse())
            throw new ErrorOnValidationException(result.Errors.Select( e => e.ErrorMessage).ToList());

    }
}