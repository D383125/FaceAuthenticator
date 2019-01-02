using Microsoft.Azure.CognitiveServices.Vision.Face.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Authorisation.Adaptor.Response
{
    public class AddPersonResponse : IAddPersonResponse
    {
        private readonly Person _person;
        public AddPersonResponse(Person person)
        {
            person.Validate();

            _person = person;
        }

        public string Name => _person.Name;

        public IEnumerable<Guid> PersistedFaceIds => _person.PersistedFaceIds;

        public Guid PersonId => _person.PersonId;

        public object UserData => _person.UserData;
    }
}
