using System;
using System.Collections.Generic;
using System.Text;

namespace Authorisation.Adaptor.Response
{
    public interface IAddPersonResponse
    {
        string Name  { get; }

        IEnumerable<Guid> PersistedFaceIds { get; }

        Guid PersonId { get; }

        object UserData { get; }
    }
}
