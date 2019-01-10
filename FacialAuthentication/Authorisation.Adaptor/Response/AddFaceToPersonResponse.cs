using Microsoft.Azure.CognitiveServices.Vision.Face.Models;
using System;


namespace Authorisation.Adaptor.Response
{
    public class AddFaceToPersonResponse : IAddFaceToPersonResponse
    {
        private readonly PersistedFace _persistedFace;

        public AddFaceToPersonResponse(PersistedFace persistedFace)
        {            
            _persistedFace.Validate();

            _persistedFace = persistedFace;
        }

        public Guid PersistedFaceId => _persistedFace.PersistedFaceId;

        public object UserData => _persistedFace.UserData;

    }
}
