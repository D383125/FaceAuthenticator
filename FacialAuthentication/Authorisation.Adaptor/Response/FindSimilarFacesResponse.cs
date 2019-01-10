using Microsoft.Azure.CognitiveServices.Vision.Face.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Linq;

namespace Authorisation.Adaptor.Response
{
    public class FindSimilarFacesResponse : IFindSimilarFacesResponse        
    {
        public IEnumerable<MatchedSimilarFace> SimilarMatchedFaces { get; }

        public FindSimilarFacesResponse(IEnumerable<SimilarFace> similarFaces)
        {
            SimilarMatchedFaces = similarFaces.Select(sf => new MatchedSimilarFace(sf));
        }
    }
}
