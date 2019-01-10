using System;
using System.Threading.Tasks;
using System.Linq;
using Windows.Media.Capture;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.Storage.Streams;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;
using System.Collections.ObjectModel;

using Autofac;
using System.Windows.Input;
using FaceAuth.View;
using FaceAuth.Model;
using System.Collections.Generic;

namespace FaceAuth.ViewModel
{
    // Type breches SRP?
    public class MainPageViewModel : ObservableObject
    {
        public MainPageViewModel()
        {
        }

        private BitmapImage _bitmapImage;
        public BitmapImage CaptureImage
        {
            get => _bitmapImage; 
            set
            {
                _bitmapImage = value;

                RaisePropertyChangedEvent(nameof(CaptureImage));
            }
        }

        private string _authenticationStatus;
        public string AuthenticationStatus
        {
            get => _authenticationStatus;
            set
            {
                _authenticationStatus = value;

                RaisePropertyChangedEvent(nameof(AuthenticationStatus));
            }
        }


        public ICommand CaptureCommand
        {
            get { return new DelegateCommand(CaptureFace, () => true); }
        }

        public ICommand AuthenticateCommand
        {
            get { return new DelegateCommand(DetectFaceAsync, () => true); }
        }

        public ICommand TrainCommand
        {
            get { return new DelegateCommand(ShowTrainDialogAsync, () => true); }
        }

        public ICommand SaveCommand
        {
            get { return new DelegateCommand(SaveCaptureAsync, () => true); }
        }

        public ICommand ClearCommand
        {
            get { return new DelegateCommand(ClearCapture, () => true); }
        }

        public ICommand AddPersonCommand
        {
            get { return new DelegateCommand(ShowAddPersonDialogAsync, () => false); }
        }

        public ICommand ExitApplicationCommand
        {
            get { return new DelegateCommand(() => Application.Current.Exit(), () => false); }
        }

        private async void ShowAddPersonDialogAsync()
        {
            var addPersonDialog = new AddPersonDialog();

            await addPersonDialog.ShowAsync();
        }

        private void ClearCapture()
        {
            CaptureImage = null;
            
            StorageFileProvider.PurgeLocalCacheAsync();

            //authBtn.IsEnabled = false;
        }

        private async void SaveCaptureAsync()
        {
            try
            {
                AuthenticationStatus = "Saving...";
                var storageFile = await StorageFileProvider.GetCachedFileAsync(); 
                FileSavePicker fs = new FileSavePicker();
                fs.FileTypeChoices.Add("Image", new List<string>() { ".jpeg" });
                fs.DefaultFileExtension = ".jpeg";
                fs.SuggestedFileName = "Image" + DateTime.Today.ToString();
                fs.SuggestedStartLocation = PickerLocationId.PicturesLibrary;
                fs.SuggestedSaveFile = storageFile;

                var s = await fs.PickSaveFileAsync();

                if (s != null)
                {
                    IRandomAccessStream stream = await storageFile.OpenReadAsync();

                    using (var dataReader = new DataReader(stream.GetInputStreamAt(0)))
                    {
                        await dataReader.LoadAsync((uint)stream.Size);
                        byte[] buffer = new byte[(int)stream.Size];
                        dataReader.ReadBytes(buffer);
                        await FileIO.WriteBytesAsync(s, buffer);
                    }
                }
            }
            catch (Exception ex)
            {
                var messageDialog = new MessageDialog($"Unable to save now. {ex.Message}");

                await messageDialog.ShowAsync();
            }

            AuthenticationStatus = "Done";
        }

        private async void CaptureFace()
        {
            AuthenticationStatus = "Capturing...";

            // todo: move into seperate class
            var capture = new CameraCaptureUI();

            capture.PhotoSettings.Format = CameraCaptureUIPhotoFormat.Jpeg;
            
            capture.PhotoSettings.MaxResolution = CameraCaptureUIMaxPhotoResolution.HighestAvailable;

            var storeFile = await capture.CaptureFileAsync(CameraCaptureUIMode.Photo);

            StorageFileProvider.CacheCaptureAsync(storeFile);

            if (storeFile != null)
            {
                BitmapImage bimage = new BitmapImage();

                var stream = await storeFile.OpenAsync(FileAccessMode.Read);

                bimage.SetSource(stream);

                CaptureImage = bimage;
            }

            AuthenticationStatus = "Done";
        }

        private async void DetectFaceAsync()
        {
            //var appView = Windows.UI.ViewManagement.ApplicationView.GetForCurrentView();

            //appView.Title = "Detecting...";
            AuthenticationStatus = "Detecting..."; 
            // todo: Use DI to new up model providers

            var storeFile = await StorageFileProvider.GetCachedFileAsync();

            var capturedImageBytes = await StorageFileProvider.SaveToFileAsync(storeFile);
            
            var detectedFace = await InternalDetectFaceAsync(capturedImageBytes);

            string message;

            if (detectedFace == null || detectedFace.FaceId == Guid.Empty)
            {
                message = $"Failed to detect a person in the capture. Please see log for details.";

                AuthenticationStatus = "Failed";

                var detectionFailedDialog = new MessageDialog(message, "Detection Failed");

                await detectionFailedDialog.ShowAsync();

                return;
            }

            var identifyResult = await InternalIdentifyFaceAsync(detectedFace);

            Person detectedPerson = null;
 
            if (identifyResult == null || !identifyResult.Any() || !identifyResult.FirstOrDefault().Candidates.Any())
            {
                message = $"Failed to identify FaceId {detectedFace.FaceId.Value}. Do you wish to be added to the system?";

                AuthenticationStatus = message;

                // todo [low]: log {detectFace.ToJson()} in richtext window
                //var json = @"{ personId:" + personId + ", groupId: " + groupId + ", faceCapture:" + asBase64 + "}";
                
                detectedPerson = await AddUnIdentifiedPersonAsync(message, capturedImageBytes);

                if (detectedFace == null) // User has decided not to add person or couldnt be done.
                    return;
            }
            else
            {
                detectedPerson = await InternalIdentifyPerson(identifyResult);

                if (detectedPerson?.PersonId == Guid.Empty)
                {
                    message = $"Failed to find PersonId {identifyResult.FirstOrDefault().Candidates.First()} in the system.";

                    ShowErrorAsync(message);
                }
                else
                {
                    message = $"Welcome {detectedPerson.Name}. You were identified with {identifyResult.First().Candidates.First().Confidence * 100} confidence.";
                }
            }

            AuthenticationStatus = message;

            var messageDialog = new MessageDialog(message, "IDENTIFICATION COMPLETE");

            await messageDialog.ShowAsync();
        }


        private async Task<DetectedFace> InternalDetectFaceAsync(Byte [] capturedImageBytes)
        {
            var faceProvider = App.Container.Resolve<FaceProvider>();

            return await faceProvider.DetectAsync(capturedImageBytes);
        }

        private async Task<ObservableCollection<IdentifyFaceResult>> InternalIdentifyFaceAsync(DetectedFace detectedFace, int groupId = 1)
        {
            var faceProvider = App.Container.Resolve<FaceProvider>();

            return await faceProvider.IdentifyAsync(detectedFace.FaceId.Value, groupId, null, null);
        }

        private async Task<Person> AddUnIdentifiedPersonAsync(string message, Byte[] capturedImageBytes, int groupId = 1)
        {
            var addUnknownPersonDialog = new MessageDialog(message, "Add Unrecognised Person");

            addUnknownPersonDialog.Commands.Add(new UICommand { Label = "Yes", Id = 0 });

            addUnknownPersonDialog.Commands.Add(new UICommand { Label = "No", Id = 1 });

            var addPersonChoice = await addUnknownPersonDialog.ShowAsync();

            Person newlyCreatedPerson = null;

            if (((int)addPersonChoice.Id) == 0)
            {
                // save bitmap. Carry forward
                AddPersonDialog addPersonDialog = new AddPersonDialog();

                // todo: invoke voice activation to register name via voice/nat luangue
                ContentDialogResult dialogResult = await addPersonDialog.ShowAsync();

                var bail = 10;
                while (addPersonDialog.NewlyCreatedPerson == null && --bail > 0)
                {
                    await Task.Delay(1000);
                }

                // Add face to pertson and train
                var trainingProvider = App.Container.Resolve<TrainingProvider>();

                trainingProvider.Train(addPersonDialog.NewlyCreatedPerson.PersonId, groupId, capturedImageBytes);

                var personProvider = App.Container.Resolve<PersonProvider>();

                await Task.Delay(1000);

                newlyCreatedPerson = await personProvider.GetPersonAsync(addPersonDialog.NewlyCreatedPerson.PersonId, groupId);

                var confirmDialog = new MessageDialog($"{newlyCreatedPerson.Name} added.", "Add Person");

                await confirmDialog.ShowAsync();
            }

            return newlyCreatedPerson;
        }

        private async Task<Person> InternalIdentifyPerson(ObservableCollection<IdentifyFaceResult> identifyResult, int groupId = 1)
        {
            var personProvider = App.Container.Resolve<PersonProvider>();

            var candidateId = identifyResult.First().Candidates.First().PersonId;

            return await personProvider.GetPersonAsync(candidateId, groupId);
        }


        private async void ShowTrainDialogAsync()
        {
            var trainDialog = new TrainDialog();

            await trainDialog.ShowAsync();
        }

        private async void ShowErrorAsync(string message)
        {
            var messageDialog = new MessageDialog(message);

            await messageDialog.ShowAsync();
        }
    }
}
