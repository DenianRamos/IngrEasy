namespace IngrEasy.Exception.ExceptionBase;

public class InvalidLoginException : IngrEasyException
{
    public InvalidLoginException() : base(ResourceErrorMessage.EMAIL_OR_PASSWORD_INVALID)
    {
    }
}