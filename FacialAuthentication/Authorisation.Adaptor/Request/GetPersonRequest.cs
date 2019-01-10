using System;
using System.Collections.Generic;
using System.Text;

namespace Authorisation.Adaptor.Request
{
    public class GetPersonRequest : IGetPersonRequest
    {
        public int GroupId { get; set; }

        public Guid PersonId { get; set; }
    }
}
