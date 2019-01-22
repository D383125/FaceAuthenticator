using Microsoft.Azure.CognitiveServices.Vision.Face.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace Authorisation.Adaptor.Response
{
    public class DetectFaceResponse : IDetectFaceResponse
    {
        private readonly DetectedFace _detectedFace;

        public DetectFaceResponse(DetectedFace detectedFace)
        {            
            _detectedFace = detectedFace;           
        }

        public Guid? FaceId => _detectedFace.FaceId;

        public Point Rectangle => new Point(_detectedFace.FaceRectangle.Height, _detectedFace.FaceRectangle.Width);

        public double? Age => _detectedFace.FaceAttributes.Age;

        public double? Smile => _detectedFace.FaceAttributes.Smile;

        public dynamic FaceAttributes => _detectedFace.FaceAttributes;

        public dynamic FacialLandmarks => _detectedFace.FaceLandmarks;        
    }
}
