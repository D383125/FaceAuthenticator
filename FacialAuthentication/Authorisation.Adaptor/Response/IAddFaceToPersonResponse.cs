using System;
using System.Collections.Generic;
using System.Text;

namespace Authorisation.Adaptor.Response
{
    public interface IAddFaceToPersonResponse
    {
        Guid PersistedFaceId { get; }

        string UserData { get; }

    }
}
