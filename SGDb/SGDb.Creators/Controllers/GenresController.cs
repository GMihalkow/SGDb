using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SGDb.Common.Controllers;
using SGDb.Common.Infrastructure;
using SGDb.Common.Infrastructure.Attributes.Authorization;
using SGDb.Creators.Models.Common;
using SGDb.Creators.Services.Genres.Contracts;

namespace SGDb.Creators.Controllers
{
    public class GenresController : BaseController
    {
        private readonly IGenresService _genresService;

        public GenresController(IGenresService genresService)
            => this._genresService = genresService;

        [AuthorizeMultipleRoles(new[] {RolesConstants.Administrator, RolesConstants.Creator})]
        public async Task<IActionResult> GetAllGenresForMultiselect()
            => this.Ok(Result<IEnumerable<BasicMultiselectOptionViewModel<int>>>.SuccessWith(
                (await this._genresService.GetAllGenresForMultiselect())));
    }
}