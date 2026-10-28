namespace PrvaWebAplikacija.Repository
{
    // Escapes user text for ILIKE, so "%" and "_" are matched literally instead of as wildcards.
    // Use with EF.Functions.ILike(column, pattern, LikePattern.EscapeChar).
    internal static class LikePattern
    {
        public const string EscapeChar = "\\";

        public static string Escape(string text) =>
            text.Replace("\\", "\\\\").Replace("%", "\\%").Replace("_", "\\_");

        public static string Contains(string text) => "%" + Escape(text) + "%";
    }
}
