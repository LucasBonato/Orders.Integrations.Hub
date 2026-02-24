using System.Text.RegularExpressions;

namespace Orders.Integrations.Hub.Integrations.Common.Extensions;

public static partial class StringExtensions
{
    public static string ToPrettyString(this object? value)
    {
        if (value == null) return string.Empty;

        string? raw = value.ToString();

        if (string.IsNullOrWhiteSpace(raw)) return string.Empty;

        raw = raw.Replace("_", " ");

        string result = LowerToUpperTransitionRegex().Replace(raw, "$1 $2");

        return char.ToUpper(result[0]) + result[1..].ToLowerInvariant() + ".";
    }

    [GeneratedRegex("([a-z])([A-Z])")]
    private static partial Regex LowerToUpperTransitionRegex();
}