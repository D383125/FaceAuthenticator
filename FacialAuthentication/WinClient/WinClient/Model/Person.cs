using System;
using System.Collections.Generic;
using System.Linq;

using Newtonsoft.Json.Linq;

namespace FaceAuth.Model
{
    public sealed class Person
    {        
        public string Name { get; }

        public Guid PersonId { get; }

        public IEnumerable<Guid> PersistedFaceIds { get; }

        public object UserData { get; }

        public Person(string responsePayload)
        {
            var response = JObject.Parse(responsePayload);

            Name = response["name"].ToString();

            PersonId = Guid.Parse(response["personId"].ToString());

            PersistedFaceIds = response["persistedFaceIds"].Select(fid => Guid.Parse(fid.ToString()));
        }
    }
}
