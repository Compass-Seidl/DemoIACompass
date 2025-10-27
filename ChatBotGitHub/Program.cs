using Azure;
using Azure.AI.Inference;
using System;

class Program
{
    static void Main()
    {
        var endpoint = new Uri("https://models.github.ai/inference");
        var credential = new AzureKeyCredential(Environment.GetEnvironmentVariable("Compass"));
        var model = "openai/gpt-5-mini";

        var client = new ChatCompletionsClient(endpoint, credential, new AzureAIInferenceClientOptions());

        Console.WriteLine("🤖 Chatbot iniciado. Digite 'sair' para encerrar.");

        while (true)
        {
            Console.Write("Você: ");
            var input = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(input)) continue;
            if (input.Trim().ToLower() == "sair") break;

            var requestOptions = new ChatCompletionsOptions()
            {
                Messages =
                {
                    new ChatRequestSystemMessage("You are a helpful assistant."),
                    new ChatRequestUserMessage(input),
                },
                Model = model
            };

            try
            {
                var response = client.Complete(requestOptions);
                Console.WriteLine($"Bot: {response.Value.Content}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro: {ex.Message}");
            }
        }

        Console.WriteLine("👋 Chat encerrado.");
    }
}