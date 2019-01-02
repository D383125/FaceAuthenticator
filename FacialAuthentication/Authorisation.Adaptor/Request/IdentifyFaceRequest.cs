using System;
using System.Collections.Generic;
using System.Text;

namespace Authorisation.Adaptor.Request
{
    public class IdentifyFaceRequest : IIdentifyFaceRequest
    {        
        public Guid FaceId { get; set; }

        public string GroupId { get; set; }

        public int? MaxNumOfCandidates { get; set; }

        public double? ConfidenceThreshold { get; set; }
    }
}
