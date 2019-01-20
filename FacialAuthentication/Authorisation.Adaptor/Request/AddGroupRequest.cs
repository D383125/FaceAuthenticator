using System;
using System.Collections.Generic;
using System.Text;

namespace Authorisation.Adaptor.Request
{
    public class AddGroupRequest : IAddGroupRequest
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string UserData { get; set; }
    }
}
