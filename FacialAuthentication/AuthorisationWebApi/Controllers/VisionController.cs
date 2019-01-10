using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;
using Authorisation.Core.Services;
using Authorisation.Adaptor.Request;
using Authorisation.Adaptor.Response;

namespace AuthorisationWebApi.Controllers
{

/// Create ML.Net service using both Deep Learning and Anomaly detection

    [Route("api/[controller]")]
    public class VisionController : Controller
    {  
        private readonly ICognitiveFaceService _cognitiveFaceService;
 
        public VisionController(ICognitiveFaceService cognitiveFaceService)
        {
            _cognitiveFaceService = cognitiveFaceService;
        }

        public async Task<string> Get()
        {
            return await Task<string>.Factory.StartNew(() => "ACK");
        }

        // These methods will be ion service
        // Chain of Repso for detech, identify andd Validate
        //#region Place In Service/brokerware
        [HttpPost("Verify")]
        public async Task<IVerifyPersonResponse> Verify(Guid faceId, Guid personId, int groupId)         
        {
            // map
            var request = new VerifyPersonRequest
            {
                FaceId = faceId,
                PersonId = personId,
                GroupId = groupId
            };

            return await _cognitiveFaceService.Handle(request);
        }



        [HttpPost("Detect")]
        public async Task<IDetectFaceResponse> Detect([FromBody]byte[] faceCapture)
        {
            // validation

            // USe infrasturcute to convert
            var detectFaceRequest = new DetectFaceRequest(faceCapture);

            // convert otJson response
            return await _cognitiveFaceService.Handle(detectFaceRequest);
        }

        
        [HttpPost("FindSimilar")]
        public async Task<IFindSimilarFacesResponse> FindSimilar(Guid faceId)
        {
            var request = new FindSimilarFacesRequest { PrinciplaFaceId = faceId };

            return await _cognitiveFaceService.Handle(request);
        }


        [HttpPost("Identify")]
        public async Task<IEnumerable<IIdentifyFaceResponse>> Identify(Guid faceId,
                                                    string groupId, int? maxNumOfCandidatesReturned, double? confidenceThreshold)
        {
            var identifyFaceRequest = new IdentifyFaceRequest
            {
                FaceId = faceId,
                GroupId = groupId,
                MaxNumOfCandidates = maxNumOfCandidatesReturned,
                ConfidenceThreshold = confidenceThreshold
            };

            return await _cognitiveFaceService.Handle(identifyFaceRequest);
        }
    }
}