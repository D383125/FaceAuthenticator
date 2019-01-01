using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;

using Microsoft.Azure.CognitiveServices.Vision.Face;
using Microsoft.Azure.CognitiveServices.Vision.Face.Models;

using Authorisation.Core.Services;

namespace AuthorisationWebApi.Controllers
{
   [Route("api/[controller]")]
    public class TrainingController : Controller 
    {
        private readonly ICognitiveAdminService _cognitiveAdminService;

        private const string _subscriptionKey = "";

        const string _baseUri = "https://australiaeast.api.cognitive.microsoft.com"; // work around for Resource not found
        

        private readonly IFaceClient _faceClient = new FaceClient(new ApiKeyServiceClientCredentials(_subscriptionKey), new DelegatingHandler[] { })
        {
            Endpoint = _baseUri
        };

        public TrainingController(ICognitiveAdminService cognitiveAdminService)
        {
            _cognitiveAdminService = cognitiveAdminService;
        }

        public async Task<string> Get()
        {
            return await Task<string>.Factory.StartNew(() => "ACK");
        }

        [HttpGet("[action]")]
        public async Task<TrainingStatus> Train(string groupId)
        {
            await _faceClient.PersonGroup.TrainAsync(groupId);

            TrainingStatus trainingStatus = null;

            var bail = 50;

            do
            {
                await Task.Delay(1000);

                trainingStatus = await _faceClient.PersonGroup.GetTrainingStatusAsync(groupId);

            } while(trainingStatus.Status == TrainingStatusType.Running && --bail > 0);

            return trainingStatus;
        }
    }
}
