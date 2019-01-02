using System;
using System.Collections.Generic;
using System.Text;

namespace Authorisation.Adaptor.Request
{
    public class DetectFaceRequest : IDetectFaceRequest
    {
        private readonly byte[] _faceCapture;

        public DetectFaceRequest(byte [] faceCapture)
        {
            _faceCapture = faceCapture;
        }

        public byte[] FaceCapture => _faceCapture;
    }
}
