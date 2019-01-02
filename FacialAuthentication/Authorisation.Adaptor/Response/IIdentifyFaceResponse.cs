using System;
using System.Collections.Generic;
using System.Text;

namespace Authorisation.Adaptor.Response
{
    public interface IIdentifyFaceResponse
    {
        Guid FaceId { get; }

        IEnumerable<Candidate> Candidates { get; }        
    }
}
