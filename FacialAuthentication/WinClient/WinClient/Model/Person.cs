using ClientProxy;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FaceAuth.Model
{
    // see https://briancaos.wordpress.com/2017/09/22/c-using-newtonsoft-and-dynamic-expandoobject-to-convert-one-json-to-another/

    public sealed class Person
    {
        private readonly dynamic _personResponse = new ExpandoObject();

        public string Name => _personResponse.Name;

        public Guid PersonId => _personResponse.PersonId;

        public IEnumerable<Guid> PersistedFaceIds => _personResponse.PersistedFaceIds;

        public object UserData => _personResponse.UserData;

        public Person(string responsePayload)
        {
            dynamic response = JObject.Parse(responsePayload);

            _personResponse.Name = response["name"];

            _personResponse.PersonId = response["PersonId"];

            _personResponse.PersistedFaceIds = response["PersistedFaceIds"];
        }
    }
}
