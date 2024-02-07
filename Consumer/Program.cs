using Confluent.Kafka;
using Confluent.Kafka.SyncOverAsync;
using Confluent.SchemaRegistry;
using Confluent.SchemaRegistry.Serdes;
using kafka;

var schemaConfig = new SchemaRegistryConfig
{
    Url = "http://localhost:8081"
};

var schemaRegistry = new CachedSchemaRegistryClient(schemaConfig);

var config = new ConsumerConfig
{
    GroupId = "Group 1",
    BootstrapServers = "localhost:9092",
};

using var consumer = new ConsumerBuilder<string, KafkaMessage>(config)
    .SetValueDeserializer(new AvroDeserializer<KafkaMessage>(schemaRegistry).AsSyncOverAsync())
    .Build();

consumer.Subscribe("test-topic2");

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