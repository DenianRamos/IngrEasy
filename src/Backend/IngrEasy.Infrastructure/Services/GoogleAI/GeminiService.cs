using System.Net.Http.Json;
using System.Text.Json;
using IngrEasy.Domain.Dtos;
using IngrEasy.Domain.Enum;
using IngrEasy.Domain.Extensions;
using IngrEasy.Domain.Services.OpenAI;

namespace IngrEasy.Infrastructure.Services.GoogleAI;

public class GeminiService : IGenerateRecipeAI
{
    private readonly GoogleAIConfig _config;

    public GeminiService(GoogleAIConfig config) => _config = config;

    public async Task<GeneratedRecipeDto> Generate(IList<string> ingredients)
    {
        if (ingredients == null || !ingredients.Any())
            throw new ArgumentException("Ingredients cannot be null or empty.", nameof(ingredients));

        var messages = new
        {
            contents = new[]
            {
                new
                {
                    role = "user",
                    parts = new[]
                    {
                        new { text = string.Join(";", ingredients) }
                    }
                }
            },
            system_instruction = new
            {
                parts = new[]
                {
                    new { text = ResourceOpenAI.STARTING_GENERATE_RECIPE }
                }
            }
        };

        var endpoint = $"https://generativelanguage.googleapis.com/v1beta/models/gemini-2.5-flash:generateContent?key={_config.ApiKey}";

        using var httpClient = new HttpClient();
        var response = await httpClient.PostAsJsonAsync(endpoint, messages);

        var json = await response.Content.ReadAsStringAsync();

        using var doc = JsonDocument.Parse(json);

        var rawText = doc.RootElement
            .GetProperty("candidates")[0]
            .GetProperty("content")
            .GetProperty("parts")[0]
            .GetProperty("text")
            .GetString();

        var responseList = rawText
            .Split("\n")
            .Where(line => line.Trim().Equals(string.Empty).IsFalse())
            .Select(item => item.Replace("[", "").Replace("]", ""))
            .ToList();

        var step = 1;

        return new GeneratedRecipeDto
        {
            Title = responseList[0],
            CookingTime = (CookingTime)Enum.Parse(typeof(CookingTime), responseList[1]),
            Ingredients = responseList[2].Split(";"),
            Instructions = responseList[3].Split("@").Select(instruction => new GeneratedInstructionDto
            {
                Text = instruction.Trim(),
                Step = step++
            }).ToList()
        };
    }
}