using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MassTransit;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SGDb.Common.Controllers;
using SGDb.Common.Data.Models;
using SGDb.Common.Infrastructure;
using SGDb.Common.Infrastructure.Attributes.Authorization;
using SGDb.Common.Infrastructure.Extensions;
using SGDb.Common.Messages.Creators;
using SGDb.Creators.Models.Common;
using SGDb.Creators.Models.Publishers;
using SGDb.Creators.Services.Creators.Contracts;
using SGDb.Creators.Services.Publishers.Contracts;

namespace SGDb.Creators.Controllers
{
    [AuthorizeMultipleRoles(new[] {RolesConstants.Administrator, RolesConstants.Creator})]
    public class PublishersController : BaseController
    {
        private readonly IPublishersService _publishersService;
        private readonly IBus _bus;
        private readonly ICreatorsService _creatorsService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public PublishersController(IPublishersService publishersService, IBus bus, ICreatorsService creatorsService,
            IHttpContextAccessor httpContextAccessor)
        {
            this._bus = bus;
            this._creatorsService = creatorsService;
            this._httpContextAccessor = httpContextAccessor;
            this._publishersService = publishersService;
        }

        public async Task<IActionResult> GetAllSearchPublishers()
            => this.Ok(Result<IEnumerable<PublisherSearchViewModel>>
                .SuccessWith(await this._publishersService.GetSearchPublishers()));

        public async Task<IActionResult> GetAllPublishersForMultiselect()
            => this.Ok(Result<IEnumerable<BasicMultiselectOptionViewModel<int>>>
                .SuccessWith(await this._publishersService.GetAllPublishersForMultiselect()));

        [HttpPost]
        public async Task<IActionResult> Create([FromForm] PublisherInputModel publisherInputModel)
        {
            var publisherViewModel = await this._publishersService.GetByName(publisherInputModel.Name);

            if (publisherViewModel != null)
                return this.BadRequest(Result.Failure("A publisher with this name already exists."));
            
            try
            {
                var creatorId = (await this._creatorsService.GetByUserId(this._httpContextAccessor.UserId()))?.Id;
                
                if (!creatorId.HasValue)
                    throw new InvalidOperationException("Creator not found.");
                
                publisherInputModel.CreatorId = creatorId.Value;

                await this._publishersService.Create(publisherInputModel);
                
                var publisherCreatedMessage = new PublisherCreatedMessage();
                var messageData = new Message(publisherCreatedMessage);

                await this._publishersService.Save(messageData);
                await this._bus.PublishWithTimeout(publisherCreatedMessage);
                await this._publishersService.MarkAsPublished(messageData.GuidId);
                
                return this.Ok(Result.Success());
            }
            catch (InvalidOperationException e)
            {
                return this.BadRequest(Result.Failure(e.Message));
            }
        }

        [HttpPatch]
        public async Task<IActionResult> Edit(PublisherEditModel publisherEditModel)
        {
            var publisherById = await this._publishersService.Get(publisherEditModel.Id);

            if (publisherById == null)
                return this.NotFound(Result.Failure("Publisher not found."));

            var publisherByName = await this._publishersService.GetByName(publisherEditModel.Name);
            
            if (publisherByName != null)
                return this.BadRequest(Result.Failure("A publisher with this name already exists."));

            await this._publishersService.Edit(publisherEditModel);
            
            return this.Ok(Result.Success());
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var publisher = await this._publishersService.Get(id);

            if (publisher == null)
                return this.NotFound(Result.Failure());

            await this._publishersService.Delete(id);

            var publisherDeletedMessage = new PublisherDeletedMessage();
            var messageData = new Message(publisherDeletedMessage);

            await this._publishersService.Save(messageData);
            await this._bus.PublishWithTimeout(publisherDeletedMessage);
            await this._publishersService.MarkAsPublished(messageData.GuidId);

            return this.Ok(Result.Success());
        }
    }
}