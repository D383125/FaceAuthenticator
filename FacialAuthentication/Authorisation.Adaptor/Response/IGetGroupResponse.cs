using System;
using System.Collections.Generic;
using System.Text;

namespace Authorisation.Adaptor.Response
{
    public interface IGetGroupResponse
    {
        string  Name { get; }
        string Id { get; }
        object UserData { get;  }
    }
}
