using System;
using System.Collections.Generic;
using System.Text;

namespace Authorisation.Adaptor.Request
{
    public interface IAddFaceToPersonRequest
    {
        byte[] FaceCapture { get; }

        int GroupId { get; }

        Guid PersonId { get; }

    }
}
