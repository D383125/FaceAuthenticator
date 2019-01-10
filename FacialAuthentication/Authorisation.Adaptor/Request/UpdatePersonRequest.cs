using Authorisation.Adaptor.Request;
using System;
using System.Collections.Generic;
using System.Text;

namespace Authorisation.Adaptor.Response
{
    public class UpdatePersonRequest : IUpdatePersonRequest
    {
        public Guid PersonId { get; set; }

        public int GroupId { get; set; }

        public object UserData { get; set; }
    }
}
