namespace IngrEasy.Communication.Response;

public class ResponseRegisterUserJson
{
    public string Name { get; set; } = String.Empty;

    public ResponseTokensJson Tokens { get; set; } = default!;

}