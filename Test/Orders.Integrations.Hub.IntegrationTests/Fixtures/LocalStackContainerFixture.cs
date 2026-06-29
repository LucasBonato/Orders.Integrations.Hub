using System.Diagnostics;

using Testcontainers.LocalStack;

namespace Orders.Integrations.Hub.IntegrationTests.Fixtures;

public class LocalStackContainerFixture : IAsyncLifetime
{
    private const string TerraformEnvDir = "terraform/envs/test";

    private readonly LocalStackContainer _container = new LocalStackBuilder("localstack/localstack:4.0.3")
        .Build();

    private readonly string _terraformDir = ResolveTerraformDir();

    public string EndpointUrl => $"http://localhost:{_container.GetMappedPublicPort(4566)}";
    public string BucketName { get; private set; } = "s3-local-bucket";
    public string SnsTopicArn { get; private set; } = string.Empty;

    public async ValueTask InitializeAsync()
    {
        await _container.StartAsync();
        try
        {
            await ApplyTerraform();
        }
        catch
        {
            await _container.DisposeAsync();
            throw;
        }
    }

    public async ValueTask DisposeAsync()
    {
        try
        {
            await DestroyTerraform();
        }
        finally
        {
            await _container.DisposeAsync();
        }
    }

    private static string ResolveTerraformDir()
    {
        var dir = new DirectoryInfo(AppContext.BaseDirectory);
        while (dir != null && !dir.GetFiles("*.sln").Any())
            dir = dir.Parent;
        return dir == null
            ? throw new DirectoryNotFoundException("Solution root not found")
            : Path.Combine(dir.FullName, "Infra", TerraformEnvDir);
    }

    private async Task ApplyTerraform()
    {
        if (!Directory.Exists(_terraformDir))
            throw new DirectoryNotFoundException($"Terraform directory not found: {_terraformDir}");

        await RunTerraform("init");
        await RunTerraform(
            $"apply -auto-approve -var=aws_base_url={EndpointUrl} " +
            $"-var=aws_access_key=dummy -var=aws_secret_key=dummy"
        );

        BucketName = (await RunTerraform("output -raw s3_bucket_name")).Trim();
        SnsTopicArn = (await RunTerraform("output -raw sns_topic_arn")).Trim();
    }

    private async Task DestroyTerraform()
    {
        if (!Directory.Exists(_terraformDir)) return;
        await RunTerraform(
            $"destroy -auto-approve -var=aws_base_url={EndpointUrl} " +
            $"-var=aws_access_key=dummy -var=aws_secret_key=dummy"
        );
    }

    private async Task<string> RunTerraform(string arguments)
    {
        using var process = new Process
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = "terraform",
                Arguments = arguments,
                WorkingDirectory = _terraformDir,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            }
        };
        process.StartInfo.EnvironmentVariables["TF_IN_AUTOMATION"] = "true";
        process.StartInfo.EnvironmentVariables["AWS_ACCESS_KEY_ID"] = "dummy";
        process.StartInfo.EnvironmentVariables["AWS_SECRET_ACCESS_KEY"] = "dummy";
        process.StartInfo.EnvironmentVariables["AWS_REGION"] = "us-east-1";

        process.Start();
        string output = await process.StandardOutput.ReadToEndAsync();
        string error = await process.StandardError.ReadToEndAsync();
        await process.WaitForExitAsync();

        if (process.ExitCode != 0)
            throw new InvalidOperationException(
                $"Terraform failed (exit {process.ExitCode}): {error}\n{output}");

        return output;
    }
}
