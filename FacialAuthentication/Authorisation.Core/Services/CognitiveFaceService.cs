using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using System.Linq;
using System.Diagnostics;

using Authorisation.Adaptor.Request;
using Authorisation.Adaptor.Response;
using Microsoft.Azure.CognitiveServices.Vision.Face;
using Microsoft.Azure.CognitiveServices.Vision.Face.Models;

namespace Authorisation.Core.Services
{
    public class CognitiveFaceService : ICognitiveFaceService
    {
        private const string _baseUri = "https://australiaeast.api.cognitive.microsoft.com";  

        private const string _subscriptionKey = "";

        private readonly IFaceClient _faceClient = new FaceClient(new ApiKeyServiceClientCredentials(_subscriptionKey), new DelegatingHandler[] { })
        {
            Endpoint = _baseUri
        };

        public CognitiveFaceService()
        {

        }

        public async Task<IDetectFaceResponse> Handle(IDetectFaceRequest detectFaceRequest)
        {

            DetectedFace detectedFace = null;

            // The list of Face attributes to return.
            IList<FaceAttributeType> faceAttributes =
                new FaceAttributeType[]
                {
                    FaceAttributeType.Gender, FaceAttributeType.Age,
                    FaceAttributeType.Smile, FaceAttributeType.Emotion,
                    FaceAttributeType.Glasses, FaceAttributeType.Hair
                };

            try
            {
                using (Stream imageFileStream = new MemoryStream(detectFaceRequest.FaceCapture))
                {
                    // The second argument specifies to return the faceId, while
                    // the third argument specifies not to return face landmarks.

                    IList<DetectedFace> faceList =
                        await _faceClient.Face.DetectWithStreamAsync(
                            imageFileStream, true, false, faceAttributes);

                    detectedFace = faceList.FirstOrDefault();                    
                }
            }
            catch (APIErrorException aex)
            {
                if (!Debugger.IsAttached)
                    Debugger.Launch();

                Debug.WriteLine(aex);                
            }

            return new DetectFaceResponse(detectedFace);            
        }

        public async Task<IVerifyPersonResponse> Handle(IVerifyPersonRequest verifyPersonRequest)
        {
            VerifyResult verifyResult = null;            

            try
            {
                verifyResult =
                        await _faceClient.Face.VerifyFaceToPersonAsync(verifyPersonRequest.FaceId, verifyPersonRequest.PersonId, verifyPersonRequest.GroupId.ToString());
            }
            catch (APIErrorException aex)
            {
                if (!Debugger.IsAttached)
                    Debugger.Launch();

                Debug.WriteLine(aex);

                throw;
            }

            return new VerifyPersonResponse(verifyResult);
        }

        public async Task<IEnumerable<IIdentifyFaceResponse>> Handle(IIdentifyFaceRequest identifyFaceRequest)
        {
            var identifyResults =
                        await _faceClient.Face.IdentifyAsync(new[] { identifyFaceRequest.FaceId }, identifyFaceRequest.GroupId, null, identifyFaceRequest.MaxNumOfCandidates,
                        identifyFaceRequest.ConfidenceThreshold);            

            var result = identifyResults.Select(r => new IdentifyFaceResponse(r));                               

            return result;
        }

        public async Task<IFindSimilarFacesResponse> Handle(IFindSimilarFacesRequest findSimilarFacesRequest)
        {
            var similarFacesResult =
                        await _faceClient.Face.FindSimilarAsync(findSimilarFacesRequest.PrinciplaFaceId);

            return new FindSimilarFacesResponse(similarFacesResult);
        }
    }
}
