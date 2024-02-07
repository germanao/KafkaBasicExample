using Confluent.Kafka;

var config = new ProducerConfig { BootstrapServers = "localhost:9092" };

using var producer = new ProducerBuilder<string, string>(config).Build();

var message = new Message<string, string>
{
    Key = Guid.NewGuid().ToString(),
    Value = "New test message"
};

var result = await producer.ProduceAsync("test-topic", message);
Console.WriteLine($"Offset: {result.Offset}");
Console.WriteLine($"Partition: {result.Partition}");
Console.WriteLine($"Topic: {result.Topic}");
Console.WriteLine($"TopicPartition: {result.TopicPartition}");