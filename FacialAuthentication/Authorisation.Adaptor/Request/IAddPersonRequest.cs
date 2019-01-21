using System;
using System.Collections.Generic;
using System.Text;

namespace Authorisation.Adaptor.Request
{
    public interface IAddPersonRequest
    {
        int GroupId { get; }

        string Name { get;}

        string UserData { get; }
    }
}
