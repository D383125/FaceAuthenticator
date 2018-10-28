using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.CognitiveServices.Vision.Face;
using Microsoft.Azure.CognitiveServices.Vision.Face.Models;

namespace AuthorisationWebApi.Controllers
{
    [Route("api/[controller]")]
    public class VisionController : Controller
    {
        [HttpPost()]
        public async Task<DetectedFace> IdentifyIndividual([FromBody] Byte [] personCaptureAsBytes)
        {
            // todo: place in autofaced serviuce
            // if byte array doesnt work pass as fle in uri from client.

            const string baseUri = "https://westcentralus.api.cognitive.microsoft.com/face/v1.0";

            var faceServiceUri = new Uri(baseUri);

            const string subscriptionKey = "7dcab6c83c844842bd44a0078911d8fb";

            DetectedFace detectedFace = null;

            IFaceClient faceClient = new FaceClient(
            new ApiKeyServiceClientCredentials(subscriptionKey),
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
            catch (APIErrorException aex)
            {
                throw;
            }
            catch(Exception ex)
            {
                throw;
            }

            return detectedFace;
        }

        /* 
        [HttpPost()]
        public async Task<DetectedFace> IdentifyIndividual(string imageFilePath)
        {
            // todo: place in autofaced serviuce
            // if byte array doesnt work pass as fle in uri from client.

            const string baseUri = "https://westcentralus.api.cognitive.microsoft.com/face/v1.0";

            var faceServiceUri = new Uri(baseUri);

            const string subscriptionKey = "7dcab6c83c844842bd44a0078911d8fb";

            DetectedFace detectedFace = null;

            IFaceClient faceClient = new FaceClient(
            new ApiKeyServiceClientCredentials(subscriptionKey),
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
                 using (Stream imageFileStream = System.IO.File.OpenRead(imageFilePath))
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

            return detectedFace;
        }
        */

    }
}