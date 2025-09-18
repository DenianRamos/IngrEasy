namespace IngrEasy.Exception.ExceptionBase;

public class NotFoundException : IngrEasyException
{
    public NotFoundException(string message) : base(message)
    {
    }
}