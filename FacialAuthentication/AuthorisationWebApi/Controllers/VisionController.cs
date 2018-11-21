using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.CognitiveServices.Vision.Face;
using Microsoft.Azure.CognitiveServices.Vision.Face.Models;

using Authorization.Contracts;
using Newtonsoft.Json.Linq;

namespace AuthorisationWebApi.Controllers
{

/// Create ML.Net service using both Deep Learning and Anomaly detection

    [Route("api/[controller]")]
    public class VisionController : Controller
    {
        private const string _baseUri = "https://australiaeast.api.cognitive.microsoft.com"; // work around for Resource not found  
        
        private const string _subscriptionKey = "";

        private readonly IFaceClient _faceClient = new FaceClient(new ApiKeyServiceClientCredentials(_subscriptionKey), new DelegatingHandler[] { })
        {
            Endpoint = _baseUri
        };

        private readonly IVisionService _visionService;

        public VisionController()
        {
            
        }
/* 
        public VisionController(IVisionService visionService)
        {
            _visionService = visionService;
        }
*/
        public async Task<string> Get()
        {
            return await Task<string>.Factory.StartNew(() => "ACK");
        }

        // These methods will be ion service
        // Chain of Repso for detech, identify andd Validate
        //#region Place In Service/brokerware
        [HttpPost("Verify")]
        public async Task<VerifyResult> Verify(JObject requestData)         
        {
            VerifyResult verifyResult = null;

            var faceId = Guid.Parse(Convert.ToString(requestData["faceId"]));

             var personId = Guid.Parse(Convert.ToString(requestData["personId"]));

            var groupId = Convert.ToString(requestData["groupId"]);

            try
            {              
                verifyResult = 
                        await _faceClient.Face.VerifyFaceToPersonAsync(faceId, personId, groupId);
            }
            catch (APIErrorException aex)
            {
                System.Diagnostics.Debug.WriteLine(aex);          

                throw;
            }
            

            return verifyResult;
        } 


        [HttpPost("Detect")]
        public async Task<DetectedFace> Detect([FromBody]JObject requestData)
        {
            byte [] faceCapture = Convert.FromBase64String(requestData["faceCaptureAsBase64"].ToString());

            DetectedFace detectedFace = null;

            // The list of Face attributes to return.
            IList<FaceAttributeType> faceAttributes =
                new FaceAttributeType[]
                {
                    FaceAttributeType.Gender, FaceAttributeType.Age,
                    FaceAttributeType.Smile, FaceAttributeType.Emotion,
                    FaceAttributeType.Glasses, FaceAttributeType.Hair
                };


            // Call the Face API.
            try
            {
                 using (Stream imageFileStream = new MemoryStream(faceCapture))
                {
                    // The second argument specifies to return the faceId, while
                    // the third argument specifies not to return face landmarks.

                    IList<DetectedFace> faceList = 
                        await _faceClient.Face.DetectWithStreamAsync(
                            imageFileStream, true, false, faceAttributes);

                    var detectedFaces = faceList.Count; 

                    if(detectedFaces > 1)
                    {
                        throw new ApplicationException($"{detectedFaces} people detected.");
                    }

                    detectedFace = faceList.First();
                }
            }
            catch (APIErrorException aex)
            {
                System.Diagnostics.Debug.WriteLine(aex);          

                throw;
            }

            return detectedFace;
        }      
        
        [HttpPost("FindSimilar")]
        public async Task<IEnumerable<SimilarFace>> FindSimilar(Guid faceId)
        {
            IList<SimilarFace> similarFacesResult = null;

            try
            {
                similarFacesResult = 
                        await _faceClient.Face.FindSimilarAsync(faceId);
            }
            catch (APIErrorException aex)
            {
                System.Diagnostics.Debug.WriteLine(aex);          

                throw;
            }

            return similarFacesResult;
        }


        [HttpPost("Identify")]
        public async Task<IEnumerable<IdentifyResult>> Identify(Guid faceId, 
                                                    string groupId, int? maxNumOfCandidatesReturned, double? confidenceThreshold)
        {
            IList<IdentifyResult> identifyResults = null;

            // Call the Face API.
            try
            {
                identifyResults = 
                        await _faceClient.Face.IdentifyAsync( new [] { faceId }, groupId, null, maxNumOfCandidatesReturned, confidenceThreshold);
            }
            catch (APIErrorException aex)
            {
                System.Diagnostics.Debug.WriteLine(aex);          

                throw;
            }

            return identifyResults;
        }
    }
}