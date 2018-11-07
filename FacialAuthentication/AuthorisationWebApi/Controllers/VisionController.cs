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

namespace AuthorisationWebApi.Controllers
{

/// Create ML.Net service using both Deep Learning and Anomaly detection

    [Route("api/[controller]")]
    public class VisionController : Controller
    {

        private const string _subscriptionKey = "8f89498a-a26c-47c6-8cb3-f1005ca63233";

        [HttpGet]
        public void Add()
        {

        }

        
        public Task<int> Get()
        {
            // In faceListListId and persistedFaceId
            throw new NotImplementedException();
        }


        [HttpPatch]
        public void Update()
        {

        }

        [HttpDelete]
        public void Delete()
        {

        }

        // These methods will be ion service
        // Chain of Repso for detech, identify andd Validate
        #region Place In Service/brokerware
        [HttpPost()]
        public async Task<JsonResult> Verify([FromBody] Byte [] personCaptureAsBytes)
        {
            var client = new HttpClient();
            var queryString = HttpUtility.ParseQueryString(string.Empty);

            // Request headers
            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", _subscriptionKey);

            var uri = "https://westus.api.cognitive.microsoft.com/face/v1.0/identify?" + queryString;

            HttpResponseMessage response;

            // Request body
           // byte[] byteData = Encoding.UTF8.GetBytes("{body}");

            using (var content = new ByteArrayContent(personCaptureAsBytes))
            {
               content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
               response = await client.PostAsync(uri, content);
            }
            
            return Json(response);
        } 

        [HttpPost]
        public async Task<JsonResult> Identify([FromBody] Byte [] personCaptureAsBytes)
        {
            throw new NotImplementedException();
        }

        [HttpPost]
        public async Task<JsonResult> Detect([FromBody] Byte [] personCaptureAsBytes)
        {
            throw new NotImplementedException();
        }

        
        [HttpPost]
        public async Task<JsonResult> FindSimilar([FromBody] Byte [] personCaptureAsBytes)
        {
            throw new NotImplementedException();
        }


        [HttpPost()]
        public async Task<DetectedFace> IdentifyEx([FromBody] Byte [] personCaptureAsBytes)
        {
            // todo: place in autofaced serviuce
            // if byte array doesnt work pass as fle in uri from client.

            const string baseUri = "https://westcentralus.api.cognitive.microsoft.com/face/v1.0";

            var faceServiceUri = new Uri(baseUri);

            DetectedFace detectedFace = null;

            IFaceClient faceClient = new FaceClient(
            new ApiKeyServiceClientCredentials(_subscriptionKey),
            new System.Net.Http.DelegatingHandler[] { });

            faceClient.Endpoint = faceServiceUri.AbsoluteUri;

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
                using (Stream imageFileStream = new MemoryStream(personCaptureAsBytes))
                {
                    // The second argument specifies to return the faceId, while
                    // the third argument specifies not to return face landmarks.
                    IList<DetectedFace> faceList = 
                        await faceClient.Face.DetectWithStreamAsync(
                            imageFileStream, true, false, faceAttributes);

                    var detectedFaces = faceList.Count;

                    if(detectedFaces > 1)
                    {
                        throw new ApplicationException($"{detectedFaces} people detected.");
                    }

                    detectedFace = faceList.FirstOrDefault();
                }
            }
            catch (APIErrorException)
            {
                throw;
            }
            catch(Exception)
            {
                throw;
            }

            return detectedFace;
        }

        #endregion Place In Service/brokerware
    }
}