using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

//using Microsoft.AspNetCore.Http;

namespace Authorisation.Adaptor.Request
{
    public class AddFaceToPersonRequest : IAddFaceToPersonRequest
    {
        public Guid PersonId { get; set; }

        public int GroupId { get; set; }

        public byte[] faceCapture { get; }                
    }
           
}

