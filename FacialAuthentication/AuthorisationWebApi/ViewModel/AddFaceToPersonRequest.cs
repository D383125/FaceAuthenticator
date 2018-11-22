using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;

namespace AuthorisationWebApi.ViewModel
{
    public class AddFaceToPersonRequest 
    {
        public Guid PersonId { get; set; }

        public String GroupId { get; set; }

        public IFormFile FaceImage { get; set; }

    }
}
