using Authorisation.Adaptor.Request;
using Authorisation.Adaptor.Response;


namespace Authorisation.Core.Services
{
    public interface ICognitiveAdminService
    {
        IAddPersonResponse Handle(IAddPersonRequest addPersonRequest);

        IAddFaceToPersonResponse Handle(IAddFaceToPersonRequest addFaceToPersonRequest);

        IGetPersonResponse Handle(IGetPersonRequest getPersonRequest);

        IUpdatePersonResponse Handle(IUpdatePersonRequest updatePersonRequest);

        IDeletePersonResponse Handle(IDeletePersonRequest deletePersonRequest);

        ITrainGroupResponse Handle(ITrainGroupRequest trainGroupRequest);

    }
}
