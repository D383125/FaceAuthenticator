using System;
using System.Threading.Tasks;
using ClientProxy;
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
            var visionClient = new Client(_webServiceUri.AbsoluteUri);

            // Then using candities fron resulr obrin person with final call 
            return await visionClient.ApiVisionIdentifyAsync(faceId, _groupId.ToString(), null, null);
        }


        public async Task<Person> GetPersonAsync(Guid candidateId, int groupId)
        {
            var visionClient = new Client(_webServiceUri.AbsoluteUri);

            return await visionClient.ApiAdministrationGetpersonAsync(candidateId, _groupId.ToString());
        }

        public async Task<DetectedFace> DetectFaceAsync(Byte[] imageAsBytes)
        {
            var visionClient = new Client(_webServiceUri.AbsoluteUri);

            return await visionClient.ApiVisionDetectAsync(imageAsBytes);
        }

    }
}
