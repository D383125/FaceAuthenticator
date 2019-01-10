using System;
using System.Collections.Generic;
using System.Text;

namespace Authorisation.Adaptor.Response
{
    public interface IGetPersonResponse
    {
        string Name { get;}

        Guid PersonId { get; }

        IEnumerable<Guid> PersistedFaceIds { get; }
    }
}
