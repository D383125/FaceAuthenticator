using Microsoft.Azure.CognitiveServices.Vision.Face.Models;
using System;


namespace Authorisation.Adaptor.Response
{
    public class AddFaceToPersonResponse : IAddFaceToPersonResponse
    {
        private readonly PersistedFace _persistedFace;

        public AddFaceToPersonResponse(PersistedFace persistedFace)
        {            
            persistedFace.Validate();

            _persistedFace = persistedFace;             
        }

        public Guid PersistedFaceId => _persistedFace.PersistedFaceId;

        public string UserData => _persistedFace.UserData;

    }
}
