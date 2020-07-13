using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Hangfire;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SGDb.Common.Data.Models;
using SGDb.Common.Infrastructure.Extensions;

namespace SGDb.Common.Messages
{
    public class MessagesHostedService : IHostedService
    {
        private readonly IBus _bus;
        private readonly IRecurringJobManager _recurringJobManager;
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public MessagesHostedService(IRecurringJobManager recurringJobManager, IServiceScopeFactory serviceScopeFactory,
            IBus bus)
        {
            this._bus = bus;
            this._recurringJobManager = recurringJobManager;
            this._serviceScopeFactory = serviceScopeFactory;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            this._recurringJobManager.AddOrUpdate(
                nameof(MessagesHostedService),
                () => this.ProcessPendingMessages(),
                "*/30 * * * * *");

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
            => Task.CompletedTask;

        public void ProcessPendingMessages()
        {
            using (var scope = this._serviceScopeFactory.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetService<DbContext>();

                var pendingMessages = dbContext
                    .Set<Message>()
                    .Where(m => !m.Published)
                    .ToList()
                    .Where(m => (DateTime.Now - m.CreatedOn).TotalSeconds > 10);

                pendingMessages.ForEach(pm =>
                {
                    try
                    {
                        this._bus.PublishWithTimeout(pm.Data, pm.Type).GetAwaiter().GetResult();

                        pm.MarkAsPublished();

                        dbContext.SaveChanges();
                    }
                    catch (OperationCanceledException)
                    {
                    }
                });
            }
        }
    }
}