using static blazorServerSK.Pages.Index;

namespace blazorServerSK.Data;

public class SentimentAndThemeService
{   

    public Task<List<SentimentAndTheme>> GetSentimentAsync(List<SurveyFeedback> comments)
    {
        var rng = new Random();
        var sentiments = new List<string> { "Positive", "Negative", "Neutral" };
        var themes = new List<string> { "Technology", "Science", "Politics", "Economy", "Health", "Sports" };
        var sentimentAndThemes = new List<SentimentAndTheme>();
        foreach (var comment in comments)
        {
            sentimentAndThemes.Add(new SentimentAndTheme
            {
                Id = comment.Id,
                Comment = comment.Comments,
                Sentiment = sentiments[rng.Next(sentiments.Count)],
                Theme = themes[rng.Next(themes.Count)]
            });
        }
        return Task.FromResult(sentimentAndThemes);
    }
}
