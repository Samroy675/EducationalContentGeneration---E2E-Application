namespace EducationalContentGeneration.UI.Models
{
    public class HistoryItem
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Type { get; set; } = "";
        public string Title { get; set; } = "";
        public string Content { get; set; } = "";
        public DateTime Time { get; set; }
    }
}
