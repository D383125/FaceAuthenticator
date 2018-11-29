using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ClientProxy;

namespace FaceAuth.ViewModel
{
    class AddPersonDialogViewModel
    {
        private readonly Uri _webServiceUri;

        private readonly int _groupId;

        public AddPersonDialogViewModel(Uri webServiceUri, int groupId)
        {
            _webServiceUri = webServiceUri;

            _groupId = groupId;
        }

        public async Task<Person> AddPerson(string personName, int groupId, string userData = null)
        {
            var serviceClient = new AdministrationClient(_webServiceUri.AbsoluteUri);

            return await serviceClient.AddPersonAsync(personName, groupId, userData);
        }

    }
}
