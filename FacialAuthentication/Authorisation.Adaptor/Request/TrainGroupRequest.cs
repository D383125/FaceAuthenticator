using System;
using System.Collections.Generic;
using System.Text;

namespace Authorisation.Adaptor.Request
{
    public class TrainGroupRequest : ITrainGroupRequest
    {
        public int GroupId { get; set; }
    }
}
