using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Authorisation.Adaptor.Request;
using Authorisation.Adaptor.Response;
using Microsoft.Azure.CognitiveServices.Vision.Face;

namespace Authorisation.Core.Services
{
    public class CognitiveAdminService : ICognitiveAdminService
    {
        private const string _baseUri = "https://australiaeast.api.cognitive.microsoft.com";

        private const string _subscriptionKey = "";

        private readonly IFaceClient _faceClient = new FaceClient(new ApiKeyServiceClientCredentials(_subscriptionKey), new DelegatingHandler[] { })
        {
            Endpoint = _baseUri
        };

        public async Task<IAddPersonResponse> Handle(IAddPersonRequest addPersonRequest)
        {
            var addedPerson = await _faceClient.PersonGroupPerson.CreateAsync(addPersonRequest.GroupId.ToString(), addPersonRequest.PersonName, addPersonRequest?.UserData?.ToString());

            return new AddPersonResponse(addedPerson);
        }

        public async Task<IAddFaceToPersonResponse> Handle(IAddFaceToPersonRequest addFaceToPersonRequest)
        {
            throw new NotImplementedException();
        }

        public async Task<IGetPersonResponse> Handle(IGetPersonRequest getPersonRequest)
        {
            throw new NotImplementedException();
        }

        public async Task<IUpdatePersonResponse> Handle(IUpdatePersonRequest updatePersonRequest)
        {
            throw new NotImplementedException();
        }

        public async Task<IDeletePersonResponse> Handle(IDeletePersonRequest deletePersonRequest)
        {
            throw new NotImplementedException();
        }

        public async Task<ITrainGroupResponse> Handle(ITrainGroupRequest trainGroupRequest)
        {
            throw new NotImplementedException();
        }
    }
}
