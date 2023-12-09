using Confluent.Kafka;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace HC.Foundation.MessageBus.IoC
{
    public static class RegisterServiceExtensions
    {
        public static IServiceCollection AddKafkaMessageBus(this IServiceCollection services, Action<ProducerConfig> configAction)
        {
            services.AddSingleton<IKafkaMessageBus, KafkaMessageBus>();

            services.AddSingleton(sp =>
            {
                var config = sp.GetRequiredService<IOptions<ProducerConfig>>();

                var builder = new ProducerBuilder<Null, string>(config.Value);

                return builder.Build();
            });

            services.Configure(configAction);

            return services;
        }

        //public static IServiceCollection AddKafkaConsumer<TValue, THandler>(this IServiceCollection services,
        //    Action<KafkaConsumerConfig> configAction) where THandler : class, IKafkaHandler
        //{
        //    services.AddScoped<IKafkaHandler, THandler>();

        //    services.AddHostedService<BackgroundKafkaConsumer>();

        //    services.Configure(configAction);

        //    return services;
        //}

        //public static IServiceCollection AddKafkaProducer(this IServiceCollection services,
        //    Action<KafkaProducerConfig> configAction)
        //{
        //    services.AddConfluentKafkaProducer();

        //    services.AddSingleton<KafkaProducer>();

        //    services.Configure(configAction);

        //    return services;
        //}

        //private static IServiceCollection AddConfluentKafkaProducer(this IServiceCollection services)
        //{
        //    services.AddSingleton(
        //        sp =>
        //        {
        //            var config = sp.GetRequiredService<IOptions<KafkaProducerConfig>>();

        //            var builder = new ProducerBuilder<Null, string>(config.Value).SetValueSerializer(new KafkaSerializer<string>());

        //            return builder.Build();
        //        });

        //    return services;
        //}
    }
}