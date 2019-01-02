using Microsoft.Azure.CognitiveServices.Vision.Face.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Authorisation.Adaptor.Response
{
    public class IdentifyFaceResponse : IIdentifyFaceResponse
    {
        private readonly IdentifyResult _identifyResult;

        public IdentifyFaceResponse(IdentifyResult identifyResult)
        {
            _identifyResult = identifyResult;
        }

        public Guid FaceId => _identifyResult.FaceId;

        public IEnumerable<Candidate> Candidates => _identifyResult.Candidates.Select(c => new Candidate { PersonId = c.PersonId, Confidence = c.Confidence });
    }
}
