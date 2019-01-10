using System;
using System.Collections.Generic;
using System.Text;

namespace Authorisation.Adaptor.Request
{
    public interface IUpdatePersonRequest
    {
        Guid PersonId { get; }

        int GroupId { get; }

        object UserData { get; }
    }
}
