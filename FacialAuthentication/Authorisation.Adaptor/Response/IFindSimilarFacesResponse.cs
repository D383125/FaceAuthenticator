using System;
using System.Collections.Generic;
using System.Text;

namespace Authorisation.Adaptor.Response
{
    public interface IFindSimilarFacesResponse
    {
        IEnumerable<MatchedSimilarFace> SimilarMatchedFaces { get; }
    }
}
