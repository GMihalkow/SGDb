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
using SGDb.Creators.Models.Genres;
using SGDb.Creators.Services.Creators.Contracts;
using SGDb.Creators.Services.Genres.Contracts;

namespace SGDb.Creators.Controllers
{
    [AuthorizeMultipleRoles(new[] { RolesConstants.Administrator, RolesConstants.Creator })]
    public class GenresController : BaseController
    {
        private readonly IGenresService _genresService;
        private readonly ICreatorsService _creatorsService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IBus _bus;

        public GenresController(IGenresService genresService, ICreatorsService creatorsService, IHttpContextAccessor httpContextAccessor, IBus bus)
        {
            this._genresService = genresService;
            this._creatorsService = creatorsService;
            this._httpContextAccessor = httpContextAccessor;
            this._bus = bus;
        }

        public async Task<IActionResult> GetAllGenresForMultiselect()
            => this.Ok(Result<IEnumerable<BasicMultiselectOptionViewModel<int>>>.SuccessWith(
                (await this._genresService.GetAllGenresForMultiselect())));

        public async Task<IActionResult> GetAllSearchGenres()
            => this.Ok(Result<IEnumerable<GenreSearchViewModel>>.SuccessWith(
                await this._genresService.GetAllSearchGenres()));

        [HttpPost]
        public async Task<IActionResult> Create([FromForm] GenreInputModel inputModel)
        {
            var genreViewModel = await this._genresService.GetByName(inputModel.Name);

            if (genreViewModel != null)
                return this.BadRequest(Result.Failure("A genre with this name already exists."));

            try
            {
                var creatorId = (await this._creatorsService.GetByUserId(this._httpContextAccessor.UserId()))?.Id;

                if (!creatorId.HasValue)
                    throw new InvalidOperationException("Creator not found.");

                inputModel.CreatorId = creatorId.Value;

                await this._genresService.Create(inputModel);

                var genreCreatedMessage = new GenreCreatedMessage();
                var messageData = new Message(genreCreatedMessage);

                await this._genresService.Save(messageData);
                await this._bus.PublishWithTimeout(genreCreatedMessage);
                await this._genresService.MarkAsPublished(messageData.GuidId);

                return this.Ok(Result.Success());
            }
            catch (InvalidOperationException ex)
            {
                return this.BadRequest(Result.Failure(ex.Message));
            }
        }

        [HttpPatch]
        public async Task<IActionResult> Edit(GenreEditModel genreEditModel)
        {
            var genreViewModel = await this._genresService.Get(genreEditModel.Id);

            if (genreViewModel == null)
                return this.NotFound(Result.Failure("Genre not found."));

            var genreByName = await this._genresService.GetByName(genreEditModel.Name);

            if (genreByName != null)
                return this.BadRequest(Result.Failure("A genre with this name already exists."));

            var currentCreatorId = (await this._creatorsService.GetByUserId(this._httpContextAccessor.UserId()))?.Id;

            if (genreViewModel.CreatorId != currentCreatorId && !this.User.IsInRole(RolesConstants.Administrator))
                return this.BadRequest(Result.Failure());

            await this._genresService.Edit(genreEditModel);

            return this.Ok(Result.Success());
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var genreViewModel = await this._genresService.Get(id);

            if (genreViewModel == null)
                return this.NotFound(Result.Failure());

            var currentCreator = await this._creatorsService.GetByUserId(this._httpContextAccessor.UserId());

            if (currentCreator.Id != genreViewModel.CreatorId && !this.HttpContext.User.IsInRole(RolesConstants.Administrator))
                return this.BadRequest(Result.Failure());

            await this._genresService.Delete(id);

            var genreDeletedMessage = new GenreDeletedMessage();
            var messageData = new Message(genreDeletedMessage);

            await this._genresService.Save(messageData);
            await this._bus.PublishWithTimeout(genreDeletedMessage);
            await this._genresService.MarkAsPublished(messageData.GuidId);

            return this.Ok(Result.Success());
        }
    }
}