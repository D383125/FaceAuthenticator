using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ClientProxy;

namespace FaceAuth.Model
{
    public class FaceProvider
    {
        private readonly Uri _serviceUri;


        public FaceProvider(Uri serviceUri)
        {
            _serviceUri = serviceUri;
        }


        public async Task<ObservableCollection<IdentifyFaceResult>> IdentifyAsync(Guid faceId, int groupId, int? confidenceThreshold, int? maxPersonsToReturn)
        {
            var visionClient = new VisionClient(_serviceUri.AbsoluteUri);

            var identifyReponse = await visionClient.IdentifyAsync(faceId, groupId.ToString(), null, null);

            throw new NotImplementedException(); //todo:
        }

        public async Task<DetectedFace> DetectAsync(Byte[] imageAsBytes)
        {
            var visionClient = new VisionClient(_serviceUri.AbsoluteUri);

            var detectedFaceResponse = await visionClient.DetectAsync(imageAsBytes);

            return new DetectedFace(detectedFaceResponse);
        }

        
        public async void Verify() => throw new NotImplementedException();
    }
}
