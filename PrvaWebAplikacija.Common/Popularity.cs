namespace PrvaWebAplikacija.Common
{
    // Allowed popularity values for anime and manga (stored as text in the database)
    public static class Popularity
    {
        public static readonly string[] Options = { "Very high", "High", "Medium", "Low" };

        public static bool IsValid(string? value) => value == null || Options.Contains(value);
    }
}
