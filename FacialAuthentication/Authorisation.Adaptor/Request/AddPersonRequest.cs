using System;
using System.Collections.Generic;
using System.Text;

namespace Authorisation.Adaptor.Request
{
    public class AddPersonRequest : IAddPersonRequest
    {       
        public int GroupId { get; set; }

        public string Name { get; set; }

        public string UserData { get; set; }
    }
}
