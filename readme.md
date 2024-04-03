# Sentiment and Theme Analysis - Semantic Kernel Sample

This sample uses the [semantic kernel](https://learn.microsoft.com/en-us/semantic-kernel/overview/) with a GPT model using [Azure Open AI](https://aka.ms/azureopenai) to evaluate survey comments for sentiment and the themes driving that sentiment. This sample batches the comments for analysis in increments of 50 records at a time. This way, hundreds of records can be processed.

1. Create and deploy an Azure OpenAI Service resource. See [here](https://learn.microsoft.com/en-us/azure/ai-services/openai/how-to/create-resource?pivots=web-portal).

1.  Specify your endpoint and other information needed to call your GPT deployment. For example:
```code
dotnet user-secrets init

dotnet user-secrets set "serviceType" "AzureOpenAI"
dotnet user-secrets set "serviceId" "gpt-35-turbo"
dotnet user-secrets set "deploymentId" "gpt-35-turbo"
dotnet user-secrets set "modelId" "gpt-3.5-turbo"
dotnet user-secrets set "endpoint" "https:// ... your endpoint ... .openai.azure.com/"
dotnet user-secrets set "apiKey" "... your Azure OpenAI key ..."
```

2. Prepare an Excel file using the following format:
```code
   Id   Comments
```
For example:
![Input](/Images/input.png)

Here's an example of the output:
![Output](/Images/output.png)

Here's the UI:
![UI](/Images/UI.png)



