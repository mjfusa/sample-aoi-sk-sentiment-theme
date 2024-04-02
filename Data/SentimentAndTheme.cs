namespace blazorServerSK.Data;

public class SentimentAndTheme
{
    public int Id { get; set; }
    public string? Comment { get; set; }

    public string? Sentiment { get; set; }
    public string? Theme { get; set; }
}
