using Amazon.SimpleNotificationService;
using Amazon.SimpleNotificationService.Model;

using MassTransit;
using MassTransit.Testing;

using Microsoft.Extensions.DependencyInjection;

using NSubstitute;
using NSubstitute.ExceptionExtensions;

using Orders.Integrations.Hub.Core.Adapters.In.Messaging.EventHandlers;
using Orders.Integrations.Hub.Core.Application.Commands;
using Orders.Integrations.Hub.IntegrationTests.CommandHandlers.Extensions;
using Orders.Integrations.Hub.TestCommon.Fakers.Commands;

namespace Orders.Integrations.Hub.IntegrationTests.CommandHandlers;

public sealed class PubSubCommandHandlerTests : IAsyncLifetime {
    private readonly ITestHarness _harness;
    private readonly IAmazonSimpleNotificationService _sns;

    public PubSubCommandHandlerTests() {
        _sns = Substitute.For<IAmazonSimpleNotificationService>();
        _sns
            .PublishAsync(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<CancellationToken>())
            .Returns(new PublishResponse { MessageId = Guid.NewGuid().ToString() });

        ServiceProvider provider = new ServiceCollection()
            .AddSingleton(_sns)
            .AddLogging()
            .AddDefaultTestHarness<PubSubCommandHandler>()
            .BuildServiceProvider(true);

        _harness = provider.GetRequiredService<ITestHarness>();
    }

    public async ValueTask InitializeAsync() => await _harness.Start();
    public async ValueTask DisposeAsync() => await _harness.Stop();

    [Fact]
    public async Task Consume_Should_Publish_To_SNS_With_TopicArn_From_Command() {
        SendNotificationCommand command = new SendNotificationCommandFaker()
            .WithTopicArn("arn:aws:sns:us-east-1:123456789012:test-topic")
            .Generate();

        await _harness.Bus.Publish(command, TestContext.Current.CancellationToken);

        Assert.True(await _harness.GetConsumerHarness<PubSubCommandHandler>()
            .Consumed.Any<SendNotificationCommand>(TestContext.Current.CancellationToken));

        await _sns.Received(1).PublishAsync(
            Arg.Is<string>(topic => topic == command.TopicArn),
            Arg.Any<string>(),
            Arg.Any<CancellationToken>()
        );
    }

    [Fact]
    public async Task Consume_Should_Fault_When_SNS_Throws() {
        _sns
            .PublishAsync(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<CancellationToken>())
            .ThrowsAsync(new Exception("sns error"));

        await _harness.Bus.Publish(new SendNotificationCommandFaker().Generate(), TestContext.Current.CancellationToken);

        Assert.True(await _harness.Published.Any<Fault<SendNotificationCommand>>(TestContext.Current.CancellationToken));
    }
}