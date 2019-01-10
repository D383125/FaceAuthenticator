using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ClientProxy;
using FaceAuth.Model;

namespace FaceAuth.ViewModel
{
    class PersonViewModel
    {
        private readonly Uri _serviceUri;

        private readonly int _groupId;

        public PersonViewModel(Uri serviceUri, int groupId)
        {
            _serviceUri = serviceUri;

            _groupId = groupId;
        }

        public async Task<Person> AddPerson(string personName, int groupId, string userData = null)
        {
            var serviceClient = new AdministrationClient(_serviceUri.AbsoluteUri);

            var addPersonResponse = await serviceClient.AddPersonAsync(personName, groupId, userData);

            return new Person(addPersonResponse);
        }

    }
}
