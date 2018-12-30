using System;
using System.Collections.Generic;
using System.Text;

using Authorisation.Adaptor.Requests;
using Authorisation.Adaptor.Responses;

namespace Authorisation.Core.Services
{
    public interface ICognitiveFaceService
    {       
        IDetectFaceResponse Handle(IDetectFaceRequest detectFaceRequest);

        IVerifyPersonResponse Handle(IVerifyPersonRequest verifyPersonRequest);

        IIdentifyFaceResponse Handle(IIdentifyFaceRequest identifyFaceRequest);
    }
}
