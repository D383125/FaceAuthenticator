using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.IO;

using Microsoft.Azure.CognitiveServices.Vision.Face;
using Microsoft.Azure.CognitiveServices.Vision.Face.Models;
using Authorization.Contracts;

using Newtonsoft.Json.Linq;
using AuthorisationWebApi.ViewModel;

namespace AuthorisationWebApi.Controllers
{
    [Route("api/[controller]")]
    public class AdministrationController : Controller
    {
        // todo 
        // 1. Add DI
        //2. Delegate all these ops to a broker service
        const string _baseUri = "https://australiaeast.api.cognitive.microsoft.com"; // work around for Resource not found

        private const string _subscriptionKey = "";


        private readonly IFaceClient _faceClient = new FaceClient(new ApiKeyServiceClientCredentials(_subscriptionKey), new DelegatingHandler[] { })
        {
            Endpoint = _baseUri
        };

        private readonly IAdministrationVisionService _administrationVisionService;

        public AdministrationController()
        {
            
        }

      //  public AdministrationController(IAdministrationVisionService administrationVisionService)
      //  {
       //     _administrationVisionService = administrationVisionService;
       // }

        public async Task<string> Get()
        {
            return await Task<string>.Factory.StartNew(() => "ACK");
        }


        [HttpPost("[action]")]
        public async void AddGroup([FromBody] JObject requestData)
        {
            var groupId = requestData["groupId"].ToString();

            var groupName = requestData["groupName"].ToString();

            var userData = requestData["userData"];

            var t = _faceClient.PersonGroup.GetAsync(groupId);

            if(t.Id == 0)
            {
                await _faceClient.PersonGroup.CreateAsync(groupId,  groupName);
            }
        }


        [HttpPost("[action]")]
        public async Task<Person> AddPerson([FromBody]string personName, int groupId, object userData)
        {           
            var addedPerson = await _faceClient.PersonGroupPerson.CreateAsync(groupId.ToString(), personName, userData?.ToString());

            return addedPerson;
        }

        
        //[HttpPost("[action]")]
        //public async Task<PersistedFace> AddFaceToPerson(AddFaceToPersonRequest addFaceToPersonRequest)
        //{
        //    string userData = null;

        //    PersistedFace persistedFaceResult = null;

        //    try
        //    {
        //        using (var ms = addFaceToPersonRequest.FaceImage.OpenReadStream())
        //        {
        //            persistedFaceResult = await _faceClient.PersonGroupPerson.AddFaceFromStreamAsync(addFaceToPersonRequest.GroupId,
        //                addFaceToPersonRequest.PersonId, ms, userData);
        //        }
        //    }
        //    catch (APIErrorException ex)
        //    {
        //        throw; // ex.Response.Content;
        //    }

        //    return persistedFaceResult;
        //}

        [HttpPost("[action]")]
        public async Task<PersistedFace> AddFaceToPerson(JObject requestData)
        {
            Guid personId = Guid.Parse(requestData["personId"].ToString());

            string groupId = requestData["groupId"].ToString();

            byte[] faceImage = Convert.FromBase64String(requestData["faceCapture"].ToString());
            
            PersistedFace persistedFaceResult = null;

            try
            {
                using (var ms = new MemoryStream(faceImage))
                {
            
                    persistedFaceResult = await _faceClient.PersonGroupPerson.AddFaceFromStreamAsync(groupId.ToString(), personId, ms);
                }
            }
            catch (APIErrorException ex)
            {
                throw; // ex.Response.Content;
            }

            return persistedFaceResult;
        }


        [HttpGet("[action]")]
        public async Task<PersonGroup> GetGroup(string groupId)
        {
            return await _faceClient.PersonGroup.GetAsync(groupId);
        }

        [HttpGet("[action]")] // route is specified - may only need Person
        public async Task<Person> GetPerson(Guid personId, string groupId)
        {
            return await _faceClient.PersonGroupPerson.GetAsync(groupId, personId);
        }

        [HttpPatch("[action]")]
        public async void UpdatePerson(Guid personId, string groupId, string userData = null)
        {
            await _faceClient.PersonGroupPerson.UpdateAsync(groupId, personId, userData);
        }

        [HttpPatch("[action]")]
        public async void UpdateGroup(string groupId, string groupName, string userData = null)
        {
            await _faceClient.PersonGroup.UpdateAsync(groupId, groupName, userData);
        }

        [HttpDelete("[action]")]
        public async void DeletePerson(Guid personId, string groupId)
        {
            await _faceClient.PersonGroupPerson.DeleteAsync(groupId, personId);
        }

        [HttpDelete("[action]")]
        public async void DeleteGroup(string groupId)
        {
            await _faceClient.PersonGroup.DeleteAsync(groupId);
        }

    }
}
