using System.Net.Http.Json;

namespace WebApi.Test;

public class IngrEasyClassFixture : IClassFixture<CustomWebApplicationFactory>

{
    
    private readonly HttpClient _client;

    public IngrEasyClassFixture(CustomWebApplicationFactory factory) => _client =  factory.CreateClient();

    protected async Task<HttpResponseMessage> DoPost(string method, object request, string culture = "en")
    {
        ChangeRequestCulture(culture);
        return await _client.PostAsJsonAsync(method, request);

    }

    private void ChangeRequestCulture(string culture)
    {
        if(_client.DefaultRequestHeaders.Contains("Accept-Language"))
            _client.DefaultRequestHeaders.Remove("Accept-Language");
        
        _client.DefaultRequestHeaders.Add("Accept-Language", culture);
    }

}