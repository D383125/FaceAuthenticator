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


        public async Task<ObservableCollection<IdentifyResult>> IdentifyAsync(Guid faceId, int groupId, int? confidenceThreshold, int? maxPersonsToReturn)
        {
            var visionClient = new VisionClient(_serviceUri.AbsoluteUri);

            return await visionClient.IdentifyAsync(faceId, groupId.ToString(), null, null);
        }

        public async Task<DetectedFace> DetectAsync(Byte[] imageAsBytes)
        {
            var visionClient = new VisionClient(_serviceUri.AbsoluteUri);

            return await visionClient.DetectAsync(imageAsBytes);
        }

        
        public async void Verify() => throw new NotImplementedException();
    }
}
