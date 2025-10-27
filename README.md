# ChatBotGitHub

Lightweight .NET 8 console chatbot that calls a hosted inference API (example: GitHub/third‑party model endpoint) to get chat completions. Intended as a minimal reference for integrating a remote LLM/chat completions service from a .NET 8 application.

## Introduction

This project demonstrates a simple interactive console chatbot written in C# (C# 12 / .NET 8). It shows:
- how to call a remote chat completions API from a console app,
- safe handling of credentials (do not hardcode secrets),
- minimal request/response flow and error handling.

Use this as a starting point to integrate any compatible inference/chat API by adapting the endpoint, credential type and client calls.

## Prerequisites

- .NET 8 SDK: https://dotnet.microsoft.com/download/dotnet/8.0
- Visual Studio 2022 (or VS Code) — for Visual Studio use the __Build > Build Solution__ and __Debug > Start Debugging__ commands
- Network access to your model inference endpoint

## Installation

1. Clone the repository:
   git clone https://github.com/Compass-Seidl/DemoIACompass.git

2. Open the `ChatBotGitHub` project in Visual Studio or use the CLI:
   - Restore packages:
     dotnet restore
   - Build:
     dotnet build

## Configuration

Do NOT keep secrets in source code. Configure the endpoint, key and model/deployment name via environment variables or configuration.

Recommended environment variables (examples):

- MODEL_ENDPOINT — base inference endpoint (example: `https://models.github.ai/inference`)
- MODEL_KEY — API key / token for the inference service
- MODEL_NAME — model or deployment id (example: `openai/gpt-5-mini`)

Set variables (PowerShell):
$env:MODEL_ENDPOINT="https://models.github.ai/inference"
$env:MODEL_KEY="your_api_key_here"
$env:MODEL_NAME="openai/gpt-5-mini"

Set variables (bash):
export MODEL_ENDPOINT="https://models.github.ai/inference"
export MODEL_KEY="your_api_key_here"
export MODEL_NAME="openai/gpt-5-mini"

Alternatively, add them to `appsettings.Development.json` or your CI secret store and read via configuration.

## How to run

From the project folder:

dotnet run --project ChatBotGitHub

Or, in Visual Studio:
- Open solution
- Select the `ChatBotGitHub` project as startup
- Use __Debug > Start Without Debugging__ or __Debug > Start Debugging__

When running, the console will prompt:

🤖 Chatbot iniciado. Digite 'sair' para encerrar.
Você: <type your message>

Example session:
Você: Hello
Bot: Hello — how can I help you today?

Type `sair` (or Ctrl+C) to exit.

## Code notes

The minimal example in `Program.cs`:
- Creates a client for an inference endpoint
- Sends simple system + user messages
- Prints the first returned completion content
- Catches and prints exceptions

Be sure to adapt:
- client instantiation to match your provider SDK (Azure, GitHub, OpenAI, etc.)
- request shape (some providers expect different message formats)
- authentication mechanism

## Dependencies

Required NuGet packages (as used by the example code):
- Azure.Core (for `AzureKeyCredential`)
- Azure.AI.Inference (or the provider-specific SDK you choose; e.g., Azure.AI.OpenAI for Azure OpenAI)

Install via CLI (example):
dotnet add ChatBotGitHub package Azure.Core
dotnet add ChatBotGitHub package Azure.AI.Inference

Adjust package names to match your chosen provider's SDK.

## Project structure

ChatBotGitHub/
- Program.cs         — console chatbot main (interactive)
- ChatBotGitHub.csproj
- README.md

Solution root contains other example projects (ImageClassification, etc.) in the repository.

## Troubleshooting

- "Endpoint/Key errors": verify environment variables and that the key is valid for the endpoint.
- "Network/Timeouts": ensure outbound network access and correct base URL.
- "Invalid response shape": inspect the provider docs; message/response structure may differ.
- If you previously hardcoded keys, remove them and rotate secrets.

## Contributing

Contributions, fixes and improvements welcome. Open PRs against `master` branch on the repo.

## License

This project is provided under the MIT License. See the `LICENSE` file for details.
