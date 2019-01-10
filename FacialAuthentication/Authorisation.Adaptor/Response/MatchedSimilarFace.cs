using Microsoft.Azure.CognitiveServices.Vision.Face.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Authorisation.Adaptor.Response
{
    public class MatchedSimilarFace
    {
        public double Confidence { get; }

        public Guid? FaceId { get; }

        public Guid? PersistedFaceId { get; }

        public MatchedSimilarFace(SimilarFace similarFace)
        {
            similarFace.Validate();

            FaceId = similarFace.FaceId;

            PersistedFaceId = similarFace.PersistedFaceId;

            Confidence = similarFace.Confidence;
        }

    }
}
