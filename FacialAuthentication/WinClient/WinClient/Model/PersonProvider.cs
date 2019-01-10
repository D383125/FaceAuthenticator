using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ClientProxy;

namespace FaceAuth.Model
{
    public sealed class PersonProvider
    {
        Guid? Id { get; }
        public string Name { get;}

        public int GroupId { get; }

        private readonly Uri _serviceUri;


        public PersonProvider(Uri serviceUri)
        {
            _serviceUri = serviceUri;
        }


        public async Task<Person> GetPersonAsync(Guid candidateId, int groupId)
        {
            var visionClient = new AdministrationClient(_serviceUri.AbsoluteUri);

            var getPersonResponse = await visionClient.GetPersonAsync(candidateId, groupId.ToString());

            return new Person(getPersonResponse);
        }
    }
}
