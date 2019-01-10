using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Authorisation.Adaptor.Request;
using Authorisation.Adaptor.Response;
using Microsoft.Azure.CognitiveServices.Vision.Face;
using Microsoft.Azure.CognitiveServices.Vision.Face.Models;

namespace Authorisation.Core.Services
{
    public class CognitiveAdminService : ICognitiveAdminService
    {
        private const string _baseUri = "https://australiaeast.api.cognitive.microsoft.com";

        private const string _subscriptionKey = "0998c75abb2342c492ef4506dee28217";

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
            PersistedFace persistedFaceResult = null;

            using (var ms = new MemoryStream(addFaceToPersonRequest.faceCapture))
            {
                persistedFaceResult = await _faceClient.PersonGroupPerson.AddFaceFromStreamAsync(addFaceToPersonRequest.GroupId.ToString(), addFaceToPersonRequest.PersonId, ms);
            }

            return new AddFaceToPersonResponse(persistedFaceResult);
        }

        public async Task<IGetPersonResponse> Handle(IGetPersonRequest getPersonRequest)
        {
            var person = await _faceClient.PersonGroupPerson.GetAsync(getPersonRequest.GroupId.ToString(), getPersonRequest.PersonId);

            return new GetPersonResponse(person);
        }

        public async Task Handle(IUpdatePersonRequest updatePersonRequest)
        {
            await _faceClient.PersonGroupPerson.UpdateAsync(updatePersonRequest.GroupId.ToString(), updatePersonRequest.PersonId, updatePersonRequest.UserData.ToString());
        }

        public async Task<IDeletePersonResponse> Handle(IDeletePersonRequest deletePersonRequest)
        {
            throw new NotImplementedException();
        }

        public async Task<ITrainGroupResponse> Handle(ITrainGroupRequest trainGroupRequest)
        {
            var groupIdToTrain = trainGroupRequest.GroupId.ToString();

            await _faceClient.PersonGroup.TrainAsync(groupIdToTrain);

            TrainingStatus trainingStatus = null;

            var bail = 50;

            do
            {
                await Task.Delay(1000);

                trainingStatus = await _faceClient.PersonGroup.GetTrainingStatusAsync(groupIdToTrain);

            } while (trainingStatus.Status == TrainingStatusType.Running && --bail > 0);

            return new TrainGroupResponse(trainingStatus);
        }

        public async Task<IGetGroupResponse> Handle(IGetGroupRequest getGroupRequest)
        {
            var group = await _faceClient.PersonGroup.GetAsync(getGroupRequest.GroupId.ToString());

            return new GetGroupResponse(group);
        }
    }
}
