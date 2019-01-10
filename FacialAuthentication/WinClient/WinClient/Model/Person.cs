using ClientProxy;
using Newtonsoft.Json;
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

        private Person(dynamic response)
        {
            _personResponse.Name = response.Name;

            _personResponse.PersonId = response.PersonId;

            _personResponse.PersistedFaceIds = response.PersistedFaceIds;
        }

        public Person(IAddPersonResponse addPersonResponse) : this(JsonConvert.DeserializeObject(addPersonResponse.ToJson()))
        {            
        }

        
        public Person(IGetPersonResponse getPersonResponse) : this(JsonConvert.DeserializeObject(getPersonResponse.ToJson()))
        {
        }
    }
}
