using Confluent.Kafka;

var config = new ConsumerConfig
{
    GroupId = "Group 1",
    BootstrapServers = "localhost:9092",
};

using var consumer = new ConsumerBuilder<string, string>(config).Build();

consumer.Subscribe("test-topic");

while (true)
{
    try
    {
        var consumeResult = consumer.Consume();
        Console.WriteLine($"Message: {consumeResult.Message.Key} = {consumeResult.Message.Value}");
    }
    catch (ConsumeException e)
    {
        Console.WriteLine($"Error reading message: {e.Error.Reason}");
    }
}