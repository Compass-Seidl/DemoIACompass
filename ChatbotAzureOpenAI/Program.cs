using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.AI.OpenAI;
using Microsoft.Extensions.AI;
using Microsoft.Extensions.Configuration;

var builder = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json", optional: true)
    .AddEnvironmentVariables();

var configuration = builder.Build();

var services = new ServiceCollection();
services.AddOpenAIClient(options =>
{
    options.ApiKey = configuration["GitHubModels:ApiKey"];
    options.Endpoint = "https://api.github.com/models/YOUR_MODEL_ENDPOINT"; // ajuste conforme o modelo
});

services.AddSingleton<IChatClient, OpenAIChatClient>();

var provider = services.BuildServiceProvider();
var chatClient = provider.GetRequiredService<IChatClient>();

Console.WriteLine("🤖 GitHub ChatBot iniciado. Digite sua pergunta:");

while (true)
{
    Console.Write("> ");
    var input = Console.ReadLine();
    if (string.IsNullOrWhiteSpace(input)) break;

    var response = await chatClient.GetChatMessageAsync(input);
    Console.WriteLine($"🧠 Resposta: {response}");
}