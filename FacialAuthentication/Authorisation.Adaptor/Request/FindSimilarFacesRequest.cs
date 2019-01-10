using System;
using System.Collections.Generic;
using System.Text;

namespace Authorisation.Adaptor.Request
{
    public class FindSimilarFacesRequest : IFindSimilarFacesRequest
    {
        public Guid PrinciplaFaceId { get; set; }
    }
}
