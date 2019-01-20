using System;
using System.Collections.Generic;
using System.Text;

namespace Authorisation.Adaptor.Request
{
    public interface IAddGroupRequest
    {
        int Id { get; }

        string Name { get;}

        string UserData { get; }
    }
}
