using System;
using System.Threading;
using System.Threading.Tasks;
using MassTransit;

namespace SGDb.Common.Infrastructure.Extensions
{
    public static class BusExtensions
    {
        public static async Task PublishWithTimeout<TMessage>(this IBus bus, TMessage message)
            => await bus.PublishWithTimeout(message, message.GetType());
        
        public static async Task PublishWithTimeout<TMessage>(this IBus bus, TMessage message, Type type)
            => await bus.Publish(message, type, new CancellationTokenSource(TimeSpan.FromSeconds(10)).Token);
    }
}