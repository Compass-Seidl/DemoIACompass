
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.MapPost("/api/chat", async (HttpContext context) =>
{
    var request = await JsonSerializer.DeserializeAsync<ChatRequest>(context.Request.Body);
    var client = new HttpClient();
    client.DefaultRequestHeaders.Add("api-key", "SUA_CHAVE_AQUI");
    client.BaseAddress = new Uri("https://SEU_ENDPOINT.openai.azure.com");

    var payload = new
    {
        messages = new[] {
            new { role = "system", content = "Você é um assistente útil." },
            new { role = "user", content = request.Question }
        },
        temperature = 0.7,
        max_tokens = 100
    };

    var content = new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json");
    var response = await client.PostAsync("/openai/deployments/gpt-4/chat/completions?api-version=2023-05-15", content);
    var result = await response.Content.ReadAsStringAsync();
    return Results.Content(result, "application/json");
});

app.Run();

record ChatRequest(string Question);
