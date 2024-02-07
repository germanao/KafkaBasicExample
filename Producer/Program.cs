using Confluent.Kafka;
using Confluent.SchemaRegistry;
using Confluent.SchemaRegistry.Serdes;
using kafka;

var schemaConfig = new SchemaRegistryConfig
{
    Url = "http://localhost:8081"
};

var schemaRegistry = new CachedSchemaRegistryClient(schemaConfig);

var config = new ProducerConfig { BootstrapServers = "localhost:9092" };

using var producer = new ProducerBuilder<string, KafkaMessage>(config)
            .SetValueSerializer(new AvroSerializer<KafkaMessage>(schemaRegistry))
            .Build();

var message = new Message<string, KafkaMessage>
{
    Key = Guid.NewGuid().ToString(),
    Value = new KafkaMessage
    {
        Id = Guid.NewGuid().ToString(),
        Key = "key",
        Partition = 2,
        Topic = "test-topic2",
        //TODO: Try remove one value to test if it is really required
    }
};

var result = await producer.ProduceAsync("test-topic2", message);
Console.WriteLine($"Offset: {result.Offset}");
Console.WriteLine($"Partition: {result.Partition}");
Console.WriteLine($"Topic: {result.Topic}");
Console.WriteLine($"TopicPartition: {result.TopicPartition}");