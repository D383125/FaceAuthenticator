using System;
using System.Threading.Tasks;
using System.Linq;
using Windows.Media.Capture;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.Storage.Streams;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;
using System.Collections.ObjectModel;

using ClientProxy;

namespace FaceAuth.ViewModel
{
    class MainPageViewModel
    {
        private readonly Uri _webServiceUri;
        private readonly int _groupId;

        public MainPageViewModel(Uri webServiceUri, int groupId)
        {
            _webServiceUri = webServiceUri;

            _groupId = groupId;
        }

        public async Task<ObservableCollection<IdentifyResult>> IdentifyFaceAsync(Guid faceId, int groupId, int? confidenceThreshold, int? maxPersonsToReturn)
        {
            var visionClient = new VisionClient(_webServiceUri.AbsoluteUri);

            return await visionClient.IdentifyAsync(faceId, _groupId.ToString(), null, null);
        }


        public async Task<Person> GetPersonAsync(Guid candidateId, int groupId)
        {
            var visionClient = new AdministrationClient(_webServiceUri.AbsoluteUri);

            return await visionClient.GetPersonAsync(candidateId, _groupId.ToString());
        }

        public async Task<DetectedFace> DetectFaceAsync(Byte[] imageAsBytes)
        {
            var visionClient = new VisionClient(_webServiceUri.AbsoluteUri);

            return await visionClient.DetectAsync(imageAsBytes);
        }

    }
}
