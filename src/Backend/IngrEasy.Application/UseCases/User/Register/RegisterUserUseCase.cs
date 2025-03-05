using AutoMapper;
using FluentValidation.Results;
using IngrEasy.Application.Services.AutoMapper;
using IngrEasy.Application.Services.Cryptography;
using IngrEasy.Communication.Requests;
using IngrEasy.Communication.Response;
using IngrEasy.Domain;
using IngrEasy.Domain.Repositories.User;
using IngrEasy.Domain.Security.Tokens;
using IngrEasy.Exception;
using IngrEasy.Exception.ExceptionBase;

namespace IngrEasy.Application.UseCases.User.Register;

public class RegisterUserUseCase : IRegisterUseUseCase
{
    
    private readonly IUserReadOnlyRepository _userReadOnlyRepository;
    private readonly IMapper _mapper;
    private readonly IUserWriteOnlyRepository _userWriteOnlyRepository;
    private readonly PasswordEncripter _passwordEncripter;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IAccessTokenGenerator _accessTokenGenerator;
    private readonly IUpdateUserOnlyRepository _updateUserOnlyRepository;


    public RegisterUserUseCase(IUserReadOnlyRepository userReadOnlyRepository, IUserWriteOnlyRepository userWriteOnlyRepository, IMapper mapper, PasswordEncripter passwordEncripter, IUnitOfWork unitOfWork, IAccessTokenGenerator accessTokenGenerator)
    {
        _userReadOnlyRepository = userReadOnlyRepository;
        _userWriteOnlyRepository = userWriteOnlyRepository;
        
        _mapper = mapper;
        _passwordEncripter = passwordEncripter;
        _unitOfWork = unitOfWork;
        _accessTokenGenerator = accessTokenGenerator;
    }

    public async Task<ResponseRegisterUserJson> Execute(RequestRegisterUserJson request)
    {
        await Validate(request);
        
        var user = _mapper.Map<Domain.User>(request);
        
        user.Password = _passwordEncripter.Encrypt(request.Password);
        user.UserIdentifier = Guid.NewGuid();
        
       await _userWriteOnlyRepository.Add(user);
       await _unitOfWork.Commit();
        return new ResponseRegisterUserJson
        {
            Name = user.Name,
            Tokens = new ResponseTokensJson()
            {
                AccessToken = _accessTokenGenerator.Generate(user.UserIdentifier)
            }
        };
    }


    public async Task Validate(RequestRegisterUserJson request)
    {
        var validate = new RegisterUserValidator();
        
        var result = validate.Validate(request);

       var emailExist = await _userReadOnlyRepository.ExistActiveUserByEmail(request.Email);
        if (emailExist)
        {
            result.Errors.Add(new ValidationFailure(string.Empty,ResourceErrorMessage.EMAIL_ALREADY_EXIST));
        }
        if (result.IsValid == false)
        {
         var errors =  result.Errors.Select(e => e.ErrorMessage).ToList();

            throw new ErrorOnValidationException(errors);
        }
  
    }
    
}