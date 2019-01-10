using Authorisation.Adaptor.Request;
using Authorisation.Adaptor.Response;
using System.Threading.Tasks;

namespace Authorisation.Core.Services
{
    public interface ICognitiveAdminService
    {
        Task<IAddPersonResponse> Handle(IAddPersonRequest addPersonRequest);

        Task<IAddFaceToPersonResponse> Handle(IAddFaceToPersonRequest addFaceToPersonRequest);

        Task<IGetPersonResponse> Handle(IGetPersonRequest getPersonRequest);

        Task Handle(IUpdatePersonRequest updatePersonRequest);

        Task<IDeletePersonResponse> Handle(IDeletePersonRequest deletePersonRequest);

        Task<ITrainGroupResponse> Handle(ITrainGroupRequest trainGroupRequest);

        Task<IGetGroupResponse> Handle(IGetGroupRequest getGroupRequest);

    }
}
