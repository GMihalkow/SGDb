using MassTransit;
using SGDb.Common.Messages.Identity;
using SGDb.Creators.Models.Creators;
using SGDb.Creators.Services.Creators.Contracts;
using System.Threading.Tasks;

namespace SGDb.Creators.Messages
{
    public class UserCreatedConsumer : IConsumer<UserCreatedMessage>
    {
        private readonly ICreatorsService _creatorsService;

        public UserCreatedConsumer(ICreatorsService creatorsService)
            => this._creatorsService = creatorsService;

        public async Task Consume(ConsumeContext<UserCreatedMessage> context)
        {
            var creatorInputModel = new CreatorInputModel
            {
                UserId = context.Message.UserId,
                Username = context.Message.Username
            };

            await this._creatorsService.Create(creatorInputModel);
        }
    }
}