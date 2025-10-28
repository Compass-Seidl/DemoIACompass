using Azure;
using Azure.AI.Inference;
using System;
using System.Threading;
using System.Threading.Tasks;

class Program
    {
        static void Main()
        {
            var endpoint = new Uri("https://models.github.ai/inference");
            var chave = Environment.GetEnvironmentVariable("Compass");
            var credential = new AzureKeyCredential(chave);
            var model = "openai/gpt-5-mini";

            var client = new ChatCompletionsClient(endpoint, credential, new AzureAIInferenceClientOptions());

            Console.WriteLine("🤖 Chatbot iniciado. Digite 'sair' para encerrar.");

            while (true)
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.Write("Você: ");
                Console.ResetColor();

                var input = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(input)) continue;
                if (input.Trim().ToLower() == "sair") break;

                var requestOptions = new ChatCompletionsOptions()
                {
                    Messages =
                {
                    new ChatRequestSystemMessage("Você é um assistente prestativo."),
                    new ChatRequestUserMessage(input),
                },
                    Model = model
                };

                var cts = new CancellationTokenSource();
                var token = cts.Token;

                // Spinner animado enquanto o modelo responde
                var spinnerTask = Task.Run(() =>
                {
                    var spinner = new[] { "|", "/", "-", "\\" };
                    int i = 0;
                    while (!token.IsCancellationRequested)
                    {
                        Console.Write($"\rPensando... {spinner[i++ % spinner.Length]}");
                        Thread.Sleep(100);
                    }
                    Console.Write("\r                    \r"); // Limpa a linha
                });

                try
                {
                    var response = client.Complete(requestOptions);
                    cts.Cancel(); // Para o spinner
                    Console.WriteLine(); // Quebra de linha após o spinner

                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"Bot: {response.Value.Content}");
                    Console.ResetColor();
                }
                catch (Exception ex)
                {
                    cts.Cancel(); // Para o spinner
                    Console.WriteLine(); // Quebra de linha após o spinner

                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"Erro: {ex.Message}");
                    Console.ResetColor();
                }
            }

            Console.WriteLine("👋 Chat encerrado.");
        }
}


