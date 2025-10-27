# DemoIACompass — .NET 8 AI / ML Examples

A compact .NET 8 solution with practical examples showing how to integrate AI and ML into .NET applications:
- Image classification using an ONNX model (`ImageClassification`).
- Console chatbot calling a hosted inference API (`ChatBotGitHub`).
- Minimal web chatbot using Azure OpenAI (`ChatbotAzureOpenAI`).

Target audience: .NET developers who want runnable examples of inference, preprocessing, and safe credential handling.

## Contents

- Introduction
- Prerequisites
- Installation
- Running examples
- Configuration (secrets & files)
- Project structure
- Dependencies
- Troubleshooting
- License

---

## Introduction

This repository provides small, focused examples that demonstrate:
- Loading and preprocessing images for ONNX models.
- Calling remote inference/chat completions endpoints from a console app.
- A minimal ASP.NET-like endpoint to call Azure OpenAI (Web API example).
The code is intentionally minimal to be easy to read and adapt.

## Prerequisites

- .NET 8 SDK — https://dotnet.microsoft.com/download/dotnet/8.0
- Visual Studio 2022 or Visual Studio Code
- Network access to your inference endpoint (if using hosted models)
- ONNX model file (e.g., `resnet50.onnx`) and labels file (e.g., `animais.txt`) for the image example

If using Visual Studio:
- Build solution using __Build > Build Solution__
- Run using __Debug > Start Debugging__ or __Debug > Start Without Debugging__

## Installation

Clone repo and restore:

git clone https://github.com/Compass-Seidl/DemoIACompass.git
cd DemoIACompass
dotnet restore

Install common sample packages (run per project if needed):

dotnet add ImageClassification package Microsoft.ML.OnnxRuntime
dotnet add ImageClassification package System.Drawing.Common
dotnet add ChatBotGitHub package Azure.Core
dotnet add ChatBotGitHub package Azure.AI.Inference
dotnet add ChatbotAzureOpenAI package Azure.AI.OpenAI

(Adjust package names if you choose a different provider SDK.)

## How to run

From repo root you can run each sample project:

- Image classification (console)
  dotnet run --project ImageClassification

- Console chatbot calling hosted inference (ChatBotGitHub)
  dotnet run --project ChatBotGitHub

- Web chatbot using Azure OpenAI (minimal API)
  dotnet run --project ChatbotAzureOpenAI
  Then POST JSON to `http://localhost:5000/api/chat`:
  { "Question": "Hello" }

## Configuration (secrets & files)

Do NOT hardcode secrets. Use environment variables or secure configuration.

ChatBotGitHub (console):
- MODEL_ENDPOINT — inference endpoint (example: `https://models.github.ai/inference`)
- MODEL_KEY — API key
- MODEL_NAME — model id (example: `openai/gpt-5-mini`)

ChatbotAzureOpenAI (web example):
- AZURE_OPENAI_ENDPOINT — `https://<your-resource>.openai.azure.com/`
- AZURE_OPENAI_KEY — API key
- AZURE_OPENAI_MODEL — deployment name (e.g., `gpt-35-turbo`)

PowerShell:
$env:MODEL_ENDPOINT="https://models.github.ai/inference"
$env:MODEL_KEY="your_api_key"
$env:MODEL_NAME="openai/gpt-5-mini"

Bash:
export MODEL_ENDPOINT="https://models.github.ai/inference"
export MODEL_KEY="your_api_key"
export MODEL_NAME="openai/gpt-5-mini"

ImageClassification data files:
- Place `resnet50.onnx`, `animais.txt` (labels) and example images inside `ImageClassification` project folder.
- In Visual Studio, select the file in Solution Explorer and set __Copy to Output Directory__ = `Copy if newer` (so files appear under `bin\Debug\net8.0` after build).

## Examples / Usage

ImageClassification
- Ensure `resnet50.onnx`, `animais.txt` and an image (e.g., `image1.png`) are in the project and copied to output.
- Run and observe predicted class printed to console.
Notes:
- Verify model input name/shape and preprocessing (resize to 224×224, normalize, channel order). Use Netron to inspect ONNX model input names and shapes.

ChatBotGitHub (console)
- Set environment variables for endpoint, key and model.
- Run and type messages in console; type `sair` to exit.
- The example uses `Azure.AI.Inference` style client — adapt to the provider SDK and API surface you target.

ChatbotAzureOpenAI (web)
- Configure Azure OpenAI endpoint/key and model, then run the project.
- POST { "Question": "text" } to `/api/chat` and receive JSON { "answer": "..." }.

## Project structure

Solution root
- ChatBotGitHub/
  - Program.cs (console chatbot)
  - ChatBotGitHub.csproj
- ChatbotAzureOpenAI/
  - Program.cs (minimal web endpoint for Azure OpenAI)
  - ChatbotAzureOpenAI.csproj
- ImageClassification/
  - Program.cs (ONNX inference example)
  - resnet50.onnx (add to project)
  - animais.txt (labels — add to project)
  - ImageClassification.csproj
- README.md
- LICENSE

Adjust names if your local structure differs.

## Common troubleshooting

- FileNotFoundException for labels or model:
  - Ensure files are in output folder (`bin\Debug\net8.0`). Use __Copy to Output Directory__ = `Copy if newer`.
- "Parameter is not valid" when loading images:
  - Ensure image is valid and convert to RGB 24bpp before processing; avoid indexed or unusual formats.
- Authentication errors:
  - Verify correct endpoint, key, and that the key has permission for that model/deployment.
- Response format differences:
  - Different providers return different shapes. Inspect responses and adapt parsing logic.

## Dependencies (examples)

- Microsoft.NET.Sdk (project)
- Microsoft.ML.OnnxRuntime
- System.Drawing.Common
- Azure.Core
- Azure.AI.Inference (or provider-specific SDK)
- Azure.AI.OpenAI (for Azure OpenAI example)

## License

MIT License — see LICENSE file.

---

If you want, I can:
- generate an `appsettings.Development.json` example showing expected configuration keys,
- add a simple `.gitignore` snippet,
- or produce a short CONTRIBUTING.md describing safe handling of secrets.
