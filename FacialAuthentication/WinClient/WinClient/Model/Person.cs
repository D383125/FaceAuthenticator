using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FaceAuth.Model
{
    public class Person // : INotifyPropertyChanged
    {
        Guid? Id { get; }
        public string Name { get;}

        public int GroupId { get; }


        public Person(string personName, int groupId, Guid? id = null)
        {
            Id = id;

            Name = personName;

            GroupId = groupId;
        }

        // public event PropertyChangedEventHandler PropertyChanged;
    }
}
