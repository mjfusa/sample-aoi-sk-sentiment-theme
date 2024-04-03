using static blazorServerSK.Pages.Index;
using Microsoft.SemanticKernel;

namespace blazorServerSK.Data;

public class SentimentAndThemeService
{

    public async Task<List<SentimentAndTheme>> GetSentimentAsync(List<SurveyFeedback> comments)
    {
        var skillsDirectory = Path.Combine(System.IO.Directory.GetCurrentDirectory(), "skills");
        var skillName = "Sentiment";
        var functionName = "GetSentiment";
        var functionDirectory = Path.Combine(skillsDirectory, skillName);
        if (!Directory.Exists(functionDirectory))
        {
            throw new Exception(message:  $"Unable to find skill {skillName}");
        }

        var skill = _kernel.CreatePluginFromPromptDirectory(functionDirectory, skillName);
        if (!skill.Contains(functionName))
        {
            throw new Exception(message:  $"Unable to find {skillName}.{functionName}");
        }

        var function = skill[functionName];
        KernelArguments arguments = new KernelArguments();
        var inputComments = "|" + "Id" + "|" + "comment" + "|" + "\n";
        foreach (var comment in comments)
        {
            inputComments += "|" + comment.Id + "|" + comment.Comments + "|" + "\n";
        }   
        
        arguments.Add("input", inputComments);

        var res= await _kernel.InvokeAsync<string>(function, arguments);
        res = res.Replace("[OUTPUT]\n", "");        
        // write a function that will parse the output and return a list of SentimentAndTheme the delimiter is "|"
        var sentimentAndThemes = new List<SentimentAndTheme>();
        var lines = res.Split("\n");
        int lineCount = 0;
        foreach (var line in lines)
        {
            if (lineCount == 0)
            {
                lineCount++;
                continue;
            }
            if (line.Length == 0)
            {
                break;
            }
            if (line.Contains("[END OUTPUT]"))
            {
                continue;
            }
            if (line.Contains("|:---"))
            {
                continue;
            }
            if (line.Contains("|---"))
            {
                continue;
            }
            if (line.Contains("|[Id1"))
            {   
                continue;
            }
            
            var values = line.Split("|");
            try {
            sentimentAndThemes.Add(new SentimentAndTheme
            {
                Id = int.Parse(values[1]),
                Comment = values[2],
                Sentiment = values[3],
                Theme = values[4]
            });
            } catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
        return sentimentAndThemes;
    }
    public Kernel? kernel;
    private string modelId;
    private string endpoint;
    private string apiKey;
    private  Kernel? _kernel;
    
    public SentimentAndThemeService()
    {
        var kernelSettings = KernelSettings.LoadSettings();
        modelId = kernelSettings.DeploymentOrModelId;
        endpoint = kernelSettings.Endpoint;
        apiKey = kernelSettings.ApiKey;
        InitializeKernel();
    }
    
    public bool InitializeKernel()
    {
        var builder = Kernel.CreateBuilder()
                       .AddAzureOpenAIChatCompletion(modelId, endpoint, apiKey);
        builder.Services.AddLogging(c => c.AddDebug().SetMinimumLevel(LogLevel.Trace));
#pragma warning disable CS9204
        // builder.Plugins.AddFromType<ConversationSummaryPlugin>(); 
#pragma warning restore CS8509        
        //builder.Plugins.AddFromPromptDirectory("./../../../Plugins/WriterPlugin");
        _kernel = builder.Build();
        return true;
    }
}

public class SentimentAndThemePlugin

{
    public int Id { get; set; }
    public string? Comment { get; set; }

    public string? Sentiment { get; set; }
    public string? Theme { get; set; }
}
