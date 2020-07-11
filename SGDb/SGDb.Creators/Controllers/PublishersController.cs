using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SGDb.Common.Controllers;
using SGDb.Common.Infrastructure;
using SGDb.Common.Infrastructure.Attributes.Authorization;
using SGDb.Creators.Models.Common;
using SGDb.Creators.Services.Publishers.Contracts;

namespace SGDb.Creators.Controllers
{
    public class PublishersController : BaseController
    {
        private readonly IPublishersService _publishersService;

        public PublishersController(IPublishersService publishersService)
        {
            this._publishersService = publishersService;
        }

        [AuthorizeMultipleRoles(new[] {RolesConstants.Administrator, RolesConstants.Creator})]
        public async Task<IActionResult> GetAllPublishersForMultiselect()
            => this.Ok(Result<IEnumerable<BasicMultiselectOptionViewModel<uint>>>
                .SuccessWith(await this._publishersService.GetAllPublishersForMultiselect()));
    }
}