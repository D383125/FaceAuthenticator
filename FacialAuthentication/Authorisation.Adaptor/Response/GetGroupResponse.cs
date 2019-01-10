using Microsoft.Azure.CognitiveServices.Vision.Face.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Authorisation.Adaptor.Response
{
    public class GetGroupResponse : IGetGroupResponse
    {
        private readonly PersonGroup _personGroup;

        public GetGroupResponse(PersonGroup personGroup)
        {
            personGroup.Validate();

            _personGroup = personGroup;
        }

        public string Name => _personGroup.Name;

        public string Id => _personGroup.PersonGroupId;

        public object UserData => _personGroup.UserData;
    }
}
