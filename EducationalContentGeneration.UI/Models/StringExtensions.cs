namespace EducationalContentGeneration.UI.Models;

public static class StringExtensions
{
    public static string OrDash(this string? s) =>
        string.IsNullOrWhiteSpace(s) ? "—" : s;
}
