using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;

using Authorization.Contracts;
using Microsoft.Azure.CognitiveServices.Vision.Face;
using Microsoft.Azure.CognitiveServices.Vision.Face.Models;

namespace AuthorisationWebApi.Controllers
{
   [Route("api/[controller]")]
    public class TrainingController : Controller 
    {
        private readonly ITrainingVisionService _trainingVisionService;

        private const string _subscriptionKey = "";

        const string _baseUri = "https://australiaeast.api.cognitive.microsoft.com"; // work around for Resource not found
        

        private readonly IFaceClient _faceClient = new FaceClient(new ApiKeyServiceClientCredentials(_subscriptionKey), new DelegatingHandler[] { })
        {
            Endpoint = _baseUri
        };

        public TrainingController()
        {
            
        }
        //public TrainingController(ITrainingVisionService trainingVisionService)
        //{
        //    _trainingVisionService = trainingVisionService;
        //}

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
