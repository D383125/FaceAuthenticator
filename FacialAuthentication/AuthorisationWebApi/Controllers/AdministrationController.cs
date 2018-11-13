using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

using Microsoft.Azure.CognitiveServices.Vision.Face;
using Microsoft.Azure.CognitiveServices.Vision.Face.Models;

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

        [HttpPost("AddGroup")]
        public async void AddGroup(string groupId, string groupName, string userData = null)
        {
            await _faceClient.PersonGroup.CreateAsync(groupId, groupName, userData);
        }

        [HttpPost("AddPerson")]
        public async void AddPerson(string personName, string groupId, string userData = null)
        {           
            await _faceClient.PersonGroupPerson.CreateAsync(groupId, personName, userData);
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
