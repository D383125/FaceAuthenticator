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
using Newtonsoft.Json.Linq;

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

        //public async Task<PersistedFace> AddFaceToPerson(Guid personId, int groupId, byte [] faceCapture)
        //{           
        //    var adminClient = new AdministrationClient(_webServiceUri.AbsoluteUri);

        //    // todo: pass in primitives rather than json

           
        //    var asBase64 = Convert.ToBase64String(faceCapture);

        //    string json = @"{ personId:" + personId + ", groupId: " + groupId + ", faceCapture:" + asBase64 + "}";

        //    var request = JObject.Parse(json);

        //    var persistedFace = await adminClient.AddFaceToPersonAsync(request);
            
        //}


    }
}
