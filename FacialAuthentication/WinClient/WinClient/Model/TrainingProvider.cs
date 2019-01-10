using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;

using ClientProxy;
using Newtonsoft.Json.Linq;
using Autofac;

namespace FaceAuth.Model
{
    public sealed class TrainingProvider
    {
        private readonly Uri _serviceUri;

        public TrainingProvider(Uri serviceUri)
        {
            _serviceUri = serviceUri;
        }

        public async void Train(Guid personId, int groupId, Byte[] image)
        {
            var adminClient = App.Container.Resolve<AdministrationClient>();
            
            // Add face to person.
            var asBase64 = Convert.ToBase64String(image);

            string json = @"{ personId:" + personId + ", groupId: " + groupId + ", faceCapture:" + asBase64 + "}";

            var request = JObject.Parse(json);

            var persistedFaceResponse = await adminClient.AddFaceToPersonAsync(request);

            var persistedFace = new PersistedFace(persistedFaceResponse);

            Debug.WriteLine($"{persistedFace.PersistedFaceId} added.");

            var trainingClient = App.Container.Resolve<TrainingClient>();

            await trainingClient.TrainAsync(groupId.ToString());
        }

        public async void Train(Guid personId, int groupId, string photosPath)
        {
            var adminClient = App.Container.Resolve<AdministrationClient>();

            var photos = Directory.EnumerateFiles(photosPath, "*.jpg");

            foreach (var photo in photos)
            {
                // Add face to person.
                byte[] image = File.ReadAllBytes(photo);

                var asBase64 = Convert.ToBase64String(image);

                string json = @"{ personId:" + personId + ", groupId: " + groupId + ", faceCapture:" + asBase64 + "}";

                var request = JObject.Parse(json);

                var persistedFaceResponse = await adminClient.AddFaceToPersonAsync(request);

                var persistedFace = new PersistedFace(persistedFaceResponse);

                Debug.WriteLine($"{persistedFace.PersistedFaceId} added.");
            }

            var trainingClient = App.Container.Resolve<TrainingClient>();

            await trainingClient.TrainAsync(groupId.ToString());
        }

    }
}
