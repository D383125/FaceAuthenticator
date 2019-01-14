using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
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
            var adminClient = App.Container.Resolve<AdministrationClient>();

            var getPersonResponse = await adminClient.GetPersonAsync(candidateId, groupId.ToString());

            return new Person(getPersonResponse);
        }
    }
}
