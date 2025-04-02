using CommonTestUtilities.Requests;
using FluentAssertions;
using IngrEasy.Application.UseCases.User.ChangePassword;
using IngrEasy.Exception;

namespace Validator.tests.User.ChangePassword;

public class ChangePasswordValidatorTest
{
    [Fact]

    public void ChangePasswordValidator_Valid()
    {
        var validator = new ChangePasswordValidator();

        var request = RequestChangePasswordBuilder.Build();
        var result = validator.Validate(request);
        
        result.IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    [InlineData(4)]
    [InlineData(5)]
    public void Error_Password_Invalid(int passwordLenght)
    {
        var validator = new ChangePasswordValidator();
        var request = RequestChangePasswordBuilder.Build(passwordLenght);
        var result = validator.Validate(request);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle().And
            .Contain(e => e.ErrorMessage.Equals(ResourceErrorMessage.PASSWORD_INVALID));
    }

    [Fact]
    public void Error_Password_Empty()
    {
        var validator = new ChangePasswordValidator();
        var request = RequestChangePasswordBuilder.Build();
        request.NewPassword = string.Empty;
        var result = validator.Validate(request);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle().And
            .Contain(e => e.ErrorMessage.Equals(ResourceErrorMessage.PASSWORD_EMPTY)); 
    }
}