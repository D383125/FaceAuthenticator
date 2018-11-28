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

        public Task<Person> AddPerson(string personName, int groupId, string userData = null)
        {
            var serviceClient = new Client(_webServiceUri.AbsoluteUri);

            throw new NotImplementedException();
            // return await serviceClient.ApiAdministrationAddpersonAsync(null); // todo: Get latest endpoiunt with swagger

        }

    }
}
