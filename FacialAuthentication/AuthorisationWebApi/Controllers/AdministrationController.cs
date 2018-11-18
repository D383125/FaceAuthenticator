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

namespace AuthorisationWebApi.Controllers
{
    [Route("api/[controller]")]
    public class AdministrationController : Controller
    {
        // todo 
        // 1. Add DI
        //2. Delegate all these ops to a broker service
        const string _baseUri = "https://australiaeast.api.cognitive.microsoft.com"; // work around for Resource not found
        
        private const string _subscriptionKey = "77a68897922a41608473f4208b2a3f5c";

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


        [HttpPost("AddGroup")]
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

        [HttpPost("AddPerson")]
        public async void AddPerson([FromBody]JObject requestData)
        {           

            var personName =  requestData["personName"].ToString();
             
            var groupId = requestData["groupId"].ToString();
             
            var userData = requestData["userData"];
            
            await _faceClient.PersonGroupPerson.CreateAsync(groupId, personName, userData?.ToString());
        }

        [HttpPost("AddFaceToPerson")]
        public async Task<PersistedFace> AddFaceToPerson(Guid personId, string groupId, byte [] faceImage, string userData = null)
        {          
            PersistedFace persistedFaceResult = null;

            using(var ms = new MemoryStream(faceImage))
            { 
                persistedFaceResult = await _faceClient.PersonGroupPerson.AddFaceFromStreamAsync(groupId, personId, null, userData);
            }

            return persistedFaceResult;
        }


        [HttpGet("Group")]
        public async Task<PersonGroup> Get(string groupId)
        {
            return await _faceClient.PersonGroup.GetAsync(groupId);
        }

        [HttpGet("Person")] // route is specified - may only need Person
        public async Task<Person> GetPerson(Guid personId, string groupId)
        {
            return await _faceClient.PersonGroupPerson.GetAsync(groupId, personId);
        }

        [HttpPatch("Person")]
        public async void UpdatePerson(Guid personId, string groupId, string userData = null)
        {
            await _faceClient.PersonGroupPerson.UpdateAsync(groupId, personId, userData);
        }

        [HttpPatch("Group")]
        public async void UpdateGroup(string groupId, string groupName, string userData = null)
        {
            await _faceClient.PersonGroup.UpdateAsync(groupId, groupName, userData);
        }

        [HttpDelete("Person")]
        public async void DeletePerson(Guid personId, string groupId)
        {
            await _faceClient.PersonGroupPerson.DeleteAsync(groupId, personId);
        }

        [HttpDelete("Group")]
        public async void DeleteGroup(string groupId)
        {
            await _faceClient.PersonGroup.DeleteAsync(groupId);
        }

    }
}
