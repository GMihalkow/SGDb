using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Refit;
using SGDb.Common.Infrastructure;
using SGDb.Creators.Gateway.Models.Creators;

namespace SGDb.Creators.Gateway.Services.Creators.Contracts
{
    public interface ICreatorsService
    {
        [Get("/api/Creators/GetAll")]
        Task<IEnumerable<CreatorViewModel>> GetAll();
        
        [Patch("/api/Creators/Edit")]
        Task Edit(CreatorEditModel editModel);
    }
}