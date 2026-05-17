using Bogus;

using Orders.Integrations.Hub.Core.Application.Commands;

namespace Orders.Integrations.Hub.TestCommon.Fakers.Commands;

public sealed class SendNotificationCommandFaker : Faker<SendNotificationCommand> {
    private string? _topicArn;
    private bool _topicArnSet;

    public SendNotificationCommandFaker() {
        CustomInstantiator(faker => new SendNotificationCommand(
            Message: new OrderUpdateFaker().Generate(),
            TopicArn: _topicArnSet
                ? _topicArn
                : $"arn:aws:sns:us-east-1:{faker.Random.Replace("############")}:{faker.Lorem.Word()}-topic" 
        ));
    }

    public SendNotificationCommandFaker WithTopicArn(string topicArn) {
        _topicArnSet = true;
        _topicArn = topicArn;
        return this;
    }

    public SendNotificationCommandFaker WithoutTopicArn() {
        _topicArnSet = true;
        _topicArn = null;
        return this;
    }
}