using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Dynamic;

using ClientProxy;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace FaceAuth.Model
{
    public sealed class IdentifyFaceResult
    {        
        public Guid FaceId { get; }

        public IEnumerable<dynamic> Candidates { get; }

        public IdentifyFaceResult(string identifyFaceResponse)
        {
            dynamic response = JsonConvert.DeserializeObject(identifyFaceResponse);

            FaceId = response.FaceId;

            //todo: convert via select
            Candidates = response["Candidates"];
        }
    }
}
