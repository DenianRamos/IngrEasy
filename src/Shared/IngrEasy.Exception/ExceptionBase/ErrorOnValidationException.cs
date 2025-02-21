namespace IngrEasy.Exception.ExceptionBase;

public class ErrorOnValidationException : IngrEasyException
{
    public IList<string> Errors { get; set; }
    
    public ErrorOnValidationException(IList<string> errors) : base(string.Empty)
    {
        Errors = errors;
    }
}