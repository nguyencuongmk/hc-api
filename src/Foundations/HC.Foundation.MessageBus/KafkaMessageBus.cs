using Confluent.Kafka;

namespace HC.Foundation.MessageBus
{
    public class KafkaMessageBus : IKafkaMessageBus
    {
        private readonly IProducer<Null, string> _producer;

        public KafkaMessageBus(IProducer<Null, string> producer)
        {
            _producer = producer;
        }

        public async Task PublishAsync(string topic, string message)
        {
            await _producer.ProduceAsync(topic, new Message<Null, string> { Value = message });
        }
    }
}