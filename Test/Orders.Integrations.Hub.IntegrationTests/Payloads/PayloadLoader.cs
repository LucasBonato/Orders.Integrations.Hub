namespace Orders.Integrations.Hub.IntegrationTests.Payloads;

/// <summary>
/// Loads raw payload templates from <c>Payloads/Templates/{integration}/{name}.json</c>.
/// Templates are never re-serialized by the app's serializers — signatures are computed
/// over the exact bytes that are posted.
/// </summary>
public static class PayloadLoader {
    private static readonly string BasePath = Path.Join(AppContext.BaseDirectory, "Payloads", "Templates");

    /// <summary>Loads a fixture file verbatim, with no token substitution.</summary>
    public static string Load(string integration, string name)
    {
        if (
            Path.IsPathRooted(integration) || 
            integration.Contains(Path.DirectorySeparatorChar) || 
            integration.Contains(Path.AltDirectorySeparatorChar)
        )
            throw new ArgumentException("integration must be a relative single path segment", nameof(integration));
        
        if (
            Path.IsPathRooted(name) ||
            name.Contains(Path.DirectorySeparatorChar) ||
            name.Contains(Path.AltDirectorySeparatorChar)
        )
            throw new ArgumentException("name must be a relative single path segment", nameof(name));

        string filename = $"{name.Replace(".json", string.Empty)}.json";
        
        string path = Path.Join(BasePath, integration, filename);

        if (!File.Exists(path))
            throw new FileNotFoundException(
                $"Payload fixture '{path.Replace(BasePath, "")}' was not found at '{path}'. " +
                "Check the file exists under Payloads/Templates and is set to " +
                "CopyToOutputDirectory in the .csproj.",
                path
            );
        
        if (string.IsNullOrEmpty(path) || Path.GetFileName(path) != filename)
            throw new ArgumentException("Path is empty and could not get the filename", nameof(path));

        return File.ReadAllText(path);
    }

    /// <summary>
    /// Loads a fixture file and replaces each {{token}} placeholder with the given value.
    /// Throws if a requested replacement token isn't found in the file, to catch typos and
    /// stale fixtures early rather than silently shipping a literal "{{token}}" in a stub
    /// response.
    /// </summary>
    public static string Load(string integration, string name, params (string Key, string Value)[] replacements)
    {
        string json = Load(integration, name);
        string relativePath = $"{integration}/{name}.json";

        foreach ((string key, string value) in replacements) {
            string placeholder = $"{{{{{key}}}}}";

            if (!json.Contains(placeholder, StringComparison.Ordinal))
                throw new InvalidOperationException($"Payload fixture '{relativePath}' has no placeholder '{placeholder}' to replace.");

            json = json.Replace(placeholder, value, StringComparison.Ordinal);
        }

        return json;
    }
}