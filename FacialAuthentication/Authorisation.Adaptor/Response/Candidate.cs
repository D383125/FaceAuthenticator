using System;
using System.Collections.Generic;
using System.Text;

namespace Authorisation.Adaptor.Response
{
    public struct Candidate
    {
        public Guid PersonId { get; set; }

        public double Confidence { get; set; }

    }
}
