﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Authorisation.Adaptor.Request
{
    public interface IAddFaceToPersonRequest
    {
        byte[] faceCapture { get; }

        int GroupId { get; }

        Guid PersonId { get; }

    }
}
