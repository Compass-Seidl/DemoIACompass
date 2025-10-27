using Azure;

internal class ChatCompletionsClient
{
    private Uri endpoint;
    private AzureKeyCredential credential;
    private AzureAIInferenceClientOptions azureAIInferenceClientOptions;

    public ChatCompletionsClient(Uri endpoint, AzureKeyCredential credential, AzureAIInferenceClientOptions azureAIInferenceClientOptions)
    {
        this.endpoint = endpoint;
        this.credential = credential;
        this.azureAIInferenceClientOptions = azureAIInferenceClientOptions;
    }
}