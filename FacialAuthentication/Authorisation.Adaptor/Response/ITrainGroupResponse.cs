using System;
using System.Collections.Generic;
using System.Text;

namespace Authorisation.Adaptor.Response
{
    public interface ITrainGroupResponse
    {
        DateTime? LastSuccessfulTraining { get; }

        DateTime? LastAction { get; }

        string Message { get; }

        string Status { get; }
    }
}
