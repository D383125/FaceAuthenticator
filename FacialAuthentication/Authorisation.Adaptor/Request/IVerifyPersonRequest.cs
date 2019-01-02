using System;
using System.Collections.Generic;
using System.Text;

namespace Authorisation.Adaptor.Request
{
    public interface IVerifyPersonRequest
    {
        Guid FaceId { get; }

        Guid PersonId { get; }

        int GroupId { get; }

    }
}
