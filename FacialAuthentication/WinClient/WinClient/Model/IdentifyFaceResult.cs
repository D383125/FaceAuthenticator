using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Dynamic;

using ClientProxy;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Converters;

namespace FaceAuth.Model
{
    public sealed class IdentifyFaceResult
    {        
        public Guid FaceId { get; }

        public IEnumerable<dynamic> Candidates { get; }

        public IdentifyFaceResult(string identifyFaceResponse)
        {            
            var jsonResponse = JObject.Parse(identifyFaceResponse);

            var json = jsonResponse.ToString(Formatting.None);

            var parsedResponse = JObject.Parse(json);

            FaceId = Guid.Parse(parsedResponse["faceId"].ToString());

            Candidates = parsedResponse["candidates"].Select(c => new { PersonId = Guid.Parse(c["personId"].ToString()), Confidence = Convert.ToDouble(c["confidence"].ToString()) } );
        }
    }
}
