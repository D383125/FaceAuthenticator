using System;
using System.Dynamic;

using ClientProxy;

namespace FaceAuth.Model
{
    public class DetectedFace
    {
        private readonly dynamic _detectedFace = new ExpandoObject();

        public Guid? FaceId => _detectedFace.FaceId;

        public FaceLandmarks FaceLandmarks => _detectedFace.FaceLandmarks;

        public dynamic FaceRectangle { get; set; }

        public FaceAttributes FaceAttributes => _detectedFace.FaceAttributes;

        //public Point Rectangle { get; set; }

        public DetectedFace(IDetectFaceResponse detectFaceResponse)
        {
            dynamic response = detectFaceResponse.ToJson();

            _detectedFace.FaceId = response.FaceId;

            _detectedFace.FaceLandmarks = response.FaceLandmarks;

            _detectedFace.FaceAttributes = response.FaceAttributes;
        }
    }
}
