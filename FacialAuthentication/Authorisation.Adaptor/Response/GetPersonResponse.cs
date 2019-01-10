using Microsoft.Azure.CognitiveServices.Vision.Face.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Authorisation.Adaptor.Response
{
    public class GetPersonResponse : IGetPersonResponse
    {
        private readonly Person _person;

        public string Name => _person.Name;

        public Guid PersonId => _person.PersonId;

        public IEnumerable<Guid> PersistedFaceIds => _person.PersistedFaceIds;

        public GetPersonResponse(Person person)
        {
            person.Validate();

            _person = person;
        }
    }
}
