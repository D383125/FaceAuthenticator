using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

using Authorisation.Core.Services;
using Authorisation.Adaptor.Response;
using Authorisation.Adaptor.Request;

namespace AuthorisationWebApi.Controllers
{
   [Route("api/[controller]")]
    public class TrainingController : Controller 
    {
        private readonly ICognitiveAdminService _cognitiveAdminService;

        public TrainingController(ICognitiveAdminService cognitiveAdminService)
        {
            _cognitiveAdminService = cognitiveAdminService;
        }

        public async Task<string> Get()
        {
            return await Task<string>.Factory.StartNew(() => "ACK");
        }

        [HttpGet("[action]")]
        public async Task<ITrainGroupResponse> Train(string groupId)
        {
            var request = new TrainGroupRequest { GroupId = Convert.ToInt32(groupId) };

            return await _cognitiveAdminService.Handle(request);            
        }
    }
}
