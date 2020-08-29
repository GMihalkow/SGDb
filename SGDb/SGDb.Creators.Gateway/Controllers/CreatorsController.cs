using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SGDb.Common.Controllers;
using SGDb.Common.Infrastructure;
using SGDb.Common.Infrastructure.Extensions;
using SGDb.Creators.Gateway.Models.Creators;
using SGDb.Creators.Gateway.Services.Creators;
using SGDb.Creators.Gateway.Services.UserRoles.Contracts;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SGDb.Creators.Gateway.Controllers
{
    [Authorize(Roles = RolesConstants.Administrator)]
    public class CreatorsController : BaseController
    {
        private readonly IUserRolesService _userRolesService;
        private readonly ICreatorsService _creatorsService;

        public CreatorsController(IUserRolesService userRolesService, ICreatorsService creatorsService)
        {
            this._userRolesService = userRolesService;
            this._creatorsService = creatorsService;
        }

        public async Task<IActionResult> GetAll()
        {
            var creators = await this._creatorsService.GetAll();
            var userRoles = await this._userRolesService.GetByUserIds(creators.Select(c => c.UserId));

            var creatorSearchViewModels = new List<CreatorSearchViewModel>();

            userRoles.ForEach(ur =>
            {
                var creator = creators.FirstOrDefault(c => c.UserId == ur.UserId);

                if (creator != null) {
                    var creatorSearchViewModel = new CreatorSearchViewModel
                    {
                        Id = creator.Id,
                        Username = creator.Username,
                        CreatedOn = creator.CreatedOn,
                        RoleName = ur.RoleName,
                        TotalGamesCreatedCount = creator.TotalGamesCreatedCount
                    };

                    creatorSearchViewModels.Add(creatorSearchViewModel);
                }
            });

            return this.Ok(Result<IEnumerable<CreatorSearchViewModel>>.SuccessWith(creatorSearchViewModels));
        }
    }
}