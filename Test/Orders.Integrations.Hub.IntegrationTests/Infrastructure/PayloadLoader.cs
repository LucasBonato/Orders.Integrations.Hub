namespace Orders.Integrations.Hub.IntegrationTests.Infrastructure;

public static class PayloadLoader {
    public static string LoadRaw(string integration, string name) {
        string path = FindPayloadFile(integration, name);
        return File.ReadAllText(path);
    }

    private static string FindPayloadFile(string integration, string name) {
        string[] searchPaths = [
            Path.Combine(AppContext.BaseDirectory, "Payloads", integration, $"{name}.json"),
            Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "..", "..",
                "Test", "Orders.Integrations.Hub.IntegrationTests", "Payloads", integration, $"{name}.json"),
            Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "..",
                "Test", "Orders.Integrations.Hub.IntegrationTests", "Payloads", integration, $"{name}.json"),
            Path.Combine(AppContext.BaseDirectory, "..", "..", "..",
                "Payloads", integration, $"{name}.json"),
        ];

        foreach (string path in searchPaths) {
            if (File.Exists(path))
                return Path.GetFullPath(path);
        }

        throw new FileNotFoundException(
            $"Payload file not found for {integration}/{name}.json. " +
            $"Searched:\n{string.Join("\n", searchPaths)}"
        );
    }
}