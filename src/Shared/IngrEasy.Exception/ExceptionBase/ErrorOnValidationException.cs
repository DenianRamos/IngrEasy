namespace IngrEasy.Exception.ExceptionBase;

public class ErrorOnValidationException : IngrEasyException
{
    public IList<string> ErrorMessage { get; set; }
    
    public ErrorOnValidationException(IList<string> errorMessage) : base(string.Empty)
    {
        ErrorMessage = errorMessage;
    }
}