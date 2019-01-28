using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Autofac;
using ClientProxy;
using FaceAuth.Model;

namespace FaceAuth.ViewModel
{
    class AddPersonViewModel : ObservableObject
    {                
        //public ICommand AddPersonCommand
        //{
        //    get
        //    {
        //        return new DelegateCommand(AddPerson, () => true);
        //    }
        //}

        public AddPersonViewModel()
        {
        }

        public async Task<Person> AddPerson(string personName, int groupId, string userData = null)
        {
            var serviceClient = App.Container.Resolve<AdministrationClient>();

            var addPersonResponse = await serviceClient.AddPersonAsync(personName, groupId, userData);

            return new Person(addPersonResponse);
        }

    }
}
