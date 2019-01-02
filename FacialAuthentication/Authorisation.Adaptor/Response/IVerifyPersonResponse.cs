﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Authorisation.Adaptor.Response
{
    public interface IVerifyPersonResponse
    {
        double Confidence { get; }

        bool IsIdentical { get; }
    }
}
