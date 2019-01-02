using System;
using System.Collections.Generic;
using System.Text;

namespace Authorisation.Adaptor.Request
{
    public interface IIdentifyFaceRequest
    {
        Guid FaceId { get; set; }

        string GroupId { get; set; }

        int? MaxNumOfCandidates { get; set; }
            
        double? ConfidenceThreshold { get; set; }       
    }
}
