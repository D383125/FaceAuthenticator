using System;
using Newtonsoft.Json.Linq;

namespace FaceAuth.Model
{
    public sealed class PersistedFace
    {
        private readonly dynamic _jsonResponse;

        public Guid PersistedFaceId { get; }

        public PersistedFace(string addFaceToPersonResponse)
        {
            _jsonResponse = JObject.Parse(addFaceToPersonResponse);

            PersistedFaceId = Guid.Parse(_jsonResponse["persistedFaceId"].ToString());
        }

        public string ToJson()
        {
            return _jsonResponse.ToString();
        }

    }
}
