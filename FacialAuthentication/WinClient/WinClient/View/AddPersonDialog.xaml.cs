using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

using Microsoft.Win32;
using System.Text;

using FaceAuth.ViewModel;
using ClientProxy;

// The Content Dialog item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace FaceAuth.View
{
    public sealed partial class AddPersonDialog : ContentDialog
    {
        private readonly Uri _serviceUri;

        // todo: [high] remove. For nmow use as Poc but this should be removed for MVP pattern ie event
        public Person NewlyCreatedPerson { get; private set; }


        public AddPersonDialog(Uri serviceUri)
        {
            _serviceUri = serviceUri;

            this.InitializeComponent();
        }

        private async void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            var personName = this.txtPersonName.Text;

            // todo: MVP or MVVM
            // Raise event
            const int GroupId = 1;

            PersonViewModel addPersonDialogViewModel = new PersonViewModel(_serviceUri, GroupId);

            NewlyCreatedPerson = await addPersonDialogViewModel.AddPerson(personName, GroupId);
            
        }

        private void ContentDialog_SecondaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
        }
    }
}
