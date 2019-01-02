using System;
using System.Collections.Generic;
using System.Text;

namespace Authorisation.Adaptor.Request
{
    public class VerifyPersonRequest : IVerifyPersonRequest
    {
        public Guid FaceId { get; set; }

        public Guid PersonId { get; set; }

        public int GroupId { get; set; }
    }
}
