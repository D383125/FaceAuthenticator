﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Authorisation.Adaptor.Request
{
    public interface IDetectFaceRequest
    {
        Byte[] FaceCapture { get; }
    }
}
