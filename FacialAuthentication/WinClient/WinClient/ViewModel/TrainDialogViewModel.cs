using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;

using ClientProxy;
using Newtonsoft.Json.Linq;

namespace FaceAuth.ViewModel
{
    class TrainDialogViewModel
    {
        private readonly Uri _webServiceUri;

        public TrainDialogViewModel(Uri webServiceUri)
        {
            _webServiceUri = webServiceUri;
        }

        public async void Train(Guid personId, int groupId, string photosPath)
        {
            var adminClient = new AdministrationClient(_webServiceUri.AbsoluteUri);

            var photos = Directory.EnumerateFiles(photosPath, "*.jpg");

            foreach (var photo in photos)
            {
                // Add face to person.
                byte[] image = File.ReadAllBytes(photo);

                var asBase64 = Convert.ToBase64String(image);

                string json = @"{ personId:" + personId + ", groupId: " + groupId + ", faceCapture:" + asBase64 + "}";

                var request = JObject.Parse(json);

                var persistedFace = await adminClient.AddFaceToPersonAsync(request);

                Debug.WriteLine($"{persistedFace.PersistedFaceId} added.");
            }

            var trainingClient = new TrainingClient(_webServiceUri.AbsoluteUri); // todo: AutoFac

            await trainingClient.TrainAsync(groupId.ToString());
        }


    }
}
