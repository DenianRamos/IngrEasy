namespace IngrEasy.Exception.ExceptionBase;

public class ErrorOnValidationException : IngrEasyException
{
    public IList<string> Errors { get; }
    
    public ErrorOnValidationException(IList<string> errors)
    {
        Errors = errors;
    }
}