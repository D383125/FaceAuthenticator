using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ClientProxy;

namespace FaceAuth.Model
{
    public class PersonProvider // : INotifyPropertyChanged
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

            return await visionClient.GetPersonAsync(candidateId, groupId.ToString());
        }




        // public event PropertyChangedEventHandler PropertyChanged;
    }
}
