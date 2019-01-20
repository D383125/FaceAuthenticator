using System;
using System.Collections.Generic;
using System.Text;

namespace Authorisation.Adaptor.Request
{
    public class GetGroupRequest : IGetGroupRequest
    {
        public int Id {get; set;}
    }
}
