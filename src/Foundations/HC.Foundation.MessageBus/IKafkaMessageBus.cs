namespace HC.Foundation.MessageBus
{
    public interface IKafkaMessageBus
    {
        Task PublishAsync(string topic, string message);
    }
}