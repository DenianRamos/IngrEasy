using IngrEasy.Communication.Requests;
using IngrEasy.Domain;
using IngrEasy.Domain.Extensions;
using IngrEasy.Domain.Repositories.User;
using IngrEasy.Domain.Services.LoggedUser;
using IngrEasy.Exception;
using IngrEasy.Exception.ExceptionBase;

namespace IngrEasy.Application.UseCases.User.Update;

public class UpdateUseCase : IUpdateUseCase
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserReadOnlyRepository _userReadOnlyRepository;
    private readonly ILoggedUser _loggedUser;
    private readonly IUpdateUserOnlyRepository _userUpdateOnlyRepository;


    public UpdateUseCase(IUnitOfWork unitOfWork, IUserReadOnlyRepository userReadOnlyRepository, ILoggedUser loggedUser, IUpdateUserOnlyRepository userUpdateOnlyRepository)
    {
        _unitOfWork = unitOfWork;
        _userReadOnlyRepository = userReadOnlyRepository;
        _loggedUser = loggedUser;
        _userUpdateOnlyRepository = userUpdateOnlyRepository;
    }
    public async Task Execute(RequestUpdateUserJson request)
    {
        var loggedUser = await _loggedUser.User();
        await Validate(request, loggedUser.Email);
        
        var user = await _userUpdateOnlyRepository.GetById(loggedUser.Id);
        
        user.Name = request.Name;
        user.Email = request.Email;
        
        _userUpdateOnlyRepository.Update(user);
       await _unitOfWork.Commit();
    }
    
    
    private async Task Validate(RequestUpdateUserJson request, string currentEmail)
    {
        var validator = new UpdateUserValidator();

        var result = await validator.ValidateAsync(request);

        if (currentEmail.Equals(request.Email).IsFalse())
        {
            var userExist = await _userReadOnlyRepository.ExistActiveUserByEmail(request.Email);
            if (userExist)
                result.Errors.Add(new FluentValidation.Results.ValidationFailure("email", ResourceErrorMessage.EMAIL_ALREADY_EXIST));

            if (result.IsValid.IsFalse())
            {
                var error = result.Errors.Select(error => error.ErrorMessage).ToList();
                throw new ErrorOnValidationException(error);

            }
        }
    }
}