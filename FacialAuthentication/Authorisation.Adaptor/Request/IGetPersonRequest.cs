using System;
using System.Collections.Generic;
using System.Text;

namespace Authorisation.Adaptor.Request
{
    public interface IGetPersonRequest
    {
        int GroupId { get; }

        Guid PersonId { get; }
    }
}
