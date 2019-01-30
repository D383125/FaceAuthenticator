using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

using Authorisation.Core.Services;
using Authorisation.Adaptor.Request;
using Authorisation.Adaptor.Response;

namespace AuthorisationWebApi.Controllers
{
    [Route("api/[controller]")]
    public class AdministrationController : Controller
    {

        private readonly ICognitiveAdminService _cognitiveAdminService;

        public AdministrationController(ICognitiveAdminService cognitiveAdminService)
        {
            _cognitiveAdminService = cognitiveAdminService;
        }

        public async Task<string> Get()
        {
            return await Task<string>.Factory.StartNew(() => "ACK");
        }

        [HttpPost("[action]")]
        public async Task<IAddPersonResponse> AddPerson([FromBody]string personName, int groupId, string userData)
        {
            //todo: mapping
            var request = new AddPersonRequest
            {
                GroupId = groupId,
                Name = personName,
                UserData = userData
            };

            return await _cognitiveAdminService.Handle(request);
        }

        [HttpPost("[action]")]
       public async Task<IAddFaceToPersonResponse> AddFaceToPerson([FromBody]JObject requestData)
        {
            var personId = Guid.Parse(requestData["personId"].ToString());

            var groupId = requestData["groupId"].ToString();
            

            var faceImage = Convert.FromBase64String(requestData["faceCapture"].ToString());            

            var request = new AddFaceToPersonRequest
            {
                PersonId = personId,

                GroupId = Convert.ToInt32(groupId),

                FaceCapture = faceImage
            };

            return await _cognitiveAdminService.Handle(request);

             //await Task.Delay(100);

             //return null;
        }

        [HttpGet("[action]")]
        public async Task<IGetGroupResponse> GetGroup(string groupId)
        {
            var request = new GetGroupRequest { Id = Convert.ToInt32(groupId) };

            return await _cognitiveAdminService.Handle(request);
        }

        [HttpGet("[action]")] // route is specified - may only need Person
        public async Task<IGetPersonResponse> GetPerson(Guid personId, string groupId)
        {
            var request = new GetPersonRequest
            {
                PersonId = personId,

                GroupId = Convert.ToInt32(groupId)
            };

            return await _cognitiveAdminService.Handle(request);
        }

        #region Move to service

        //[HttpPost("[action]")]
        //public async void AddGroup([FromBody] JObject requestData)
        //{
        //    var groupId = requestData["groupId"].ToString();

        //    var groupName = requestData["groupName"].ToString();

        //    var userData = requestData["userData"];

        //    var t = _faceClient.PersonGroup.GetAsync(groupId);

        //    if (t.Id == 0)
        //    {
        //        await _faceClient.PersonGroup.CreateAsync(groupId, groupName);
        //    }
        //}

        //[HttpPatch("[action]")]
        //public async void UpdatePerson(Guid personId, string groupId, string userData = null)
        //{
        //    await _faceClient.PersonGroupPerson.UpdateAsync(groupId, personId, userData);
        //}

        //[HttpPatch("[action]")]
        //public async void UpdateGroup(string groupId, string groupName, string userData = null)
        //{
        //    await _faceClient.PersonGroup.UpdateAsync(groupId, groupName, userData);
        //}

        //[HttpDelete("[action]")]
        //public async void DeletePerson(Guid personId, string groupId)
        //{
        //    await _faceClient.PersonGroupPerson.DeleteAsync(groupId, personId);
        //}

        //[HttpDelete("[action]")]
        //public async void DeleteGroup(string groupId)
        //{
        //    await _faceClient.PersonGroup.DeleteAsync(groupId);
        //}
        #endregion

    }
}
