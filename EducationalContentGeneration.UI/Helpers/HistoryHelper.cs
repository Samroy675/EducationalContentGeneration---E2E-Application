using EducationalContentGeneration.UI.Models;

namespace EducationalContentGeneration.UI.Helpers
{
    public static class HistoryHelper
    {
        public static HistoryItem Create(string type, string subject, string topic, string content)
        {
            return new HistoryItem
            {
                Type = type,
                Title = $"{subject} - {topic}",
                Content = content,
                Time = DateTime.Now
            };
        }
    }
}
