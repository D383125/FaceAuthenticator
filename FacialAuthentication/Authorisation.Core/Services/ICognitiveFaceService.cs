using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Authorisation.Adaptor.Request;
using Authorisation.Adaptor.Response;

namespace Authorisation.Core.Services
{
    public interface ICognitiveFaceService
    {
        Task<IDetectFaceResponse> Handle(IDetectFaceRequest detectFaceRequest);

        Task<IVerifyPersonResponse> Handle(IVerifyPersonRequest verifyPersonRequest);

        Task<IEnumerable<IIdentifyFaceResponse>> Handle(IIdentifyFaceRequest identifyFaceRequest);

        Task<IFindSimilarFacesResponse> Handle(IFindSimilarFacesRequest findSimilarFacesRequest);
    }
}
