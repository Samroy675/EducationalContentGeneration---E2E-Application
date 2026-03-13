namespace EducationalContentGeneration.UI.Models
{
    public class ChatMessage
    {
        public string Role { get; set; } = "user";
        public string Content { get; set; } = string.Empty;
        public bool IsLoading { get; set; } = false;
    }
}
