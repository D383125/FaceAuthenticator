using Microsoft.Azure.CognitiveServices.Vision.Face.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Authorisation.Adaptor.Response
{
    public class VerifyPersonResponse : IVerifyPersonResponse
    {
        private readonly VerifyResult _verifyResult;

        public VerifyPersonResponse(VerifyResult verifyResult)
        {
            _verifyResult = verifyResult;
        }

        public double Confidence => _verifyResult.Confidence;

        public bool IsIdentical => _verifyResult.IsIdentical;
    }
}
