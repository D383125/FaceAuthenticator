using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Dynamic;

using ClientProxy;
using Newtonsoft.Json;

namespace FaceAuth.Model
{
    public sealed class IdentifyFaceResult
    {
        private readonly dynamic _identifyFaceResult = new ExpandoObject();

        public Guid FaceId => _identifyFaceResult.FaceId;

        public IEnumerable<dynamic> Candidates => _identifyFaceResult.Candidates;

        public IdentifyFaceResult(IIdentifyFaceResponse identifyFaceResponse)
        {
            dynamic response = JsonConvert.DeserializeObject(identifyFaceResponse.ToJson());

            _identifyFaceResult.FaceId = response.FaceId;

            //todo: convert
            _identifyFaceResult.Candidates = response.Candidates;
        }
    }
}
