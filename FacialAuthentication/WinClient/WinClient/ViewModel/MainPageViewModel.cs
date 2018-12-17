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

using ClientProxy;
using Newtonsoft.Json.Linq;
using System.Windows.Input;
using System.IO;
using FaceAuth.View;
using FaceAuth.Model;
using Windows.Foundation;
using System.Collections.Generic;

namespace FaceAuth.ViewModel
{
    // Type breches SRP?
    public class MainPageViewModel : ObservableObject
    {
        // Alibibaba - Chinese face detection 

  //      private const int GroupId = 1;

 //       private const int ConfidenceThreshold = 1;

 //       private const int MaxNoOfCandidates = 1;

   //     private volatile StorageFile _storeFile;

        
//        private Byte[] _capturedImageBytes;

        private readonly Uri _controllerUri = new Uri(@"http://localhost:5000/");



        public MainPageViewModel()
        {

        }

        private BitmapImage _bitmapImage;
        public BitmapImage CaptureImage
        {
            get { return _bitmapImage; }
            set
            {
                _bitmapImage = value;

                RaisePropertyChangedEvent(nameof(CaptureImage));
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
            var addPersonDialog = new AddPersonDialog(_controllerUri);

            await addPersonDialog.ShowAsync();
        }

        private void ClearCapture()
        {
            CaptureImage = null;
            //FacePhoto.Source = null;

            //var stroageFile = await GetCachedFileAsync();

            //File.Delete(stroageFile.Path);

            StorageFileProvider.PurgeLocalCacheAsync();

            //authBtn.IsEnabled = false;
        }

        private async void SaveCaptureAsync()
        {
            try
            {
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
        }

        private async void CaptureFace()
        {
            // todo: move into seperate class
            var capture = new CameraCaptureUI();

            capture.PhotoSettings.Format = CameraCaptureUIPhotoFormat.Jpeg;
            
            capture.PhotoSettings.MaxResolution = CameraCaptureUIMaxPhotoResolution.HighestAvailable;

            var storeFile = await capture.CaptureFileAsync(CameraCaptureUIMode.Photo);

            StorageFileProvider.CacheCaptureAsync(storeFile);

            if (storeFile != null)
            {
                //var capturedImageBytes = await StorageFileProvider.SaveToFileAsync(storeFile);

                BitmapImage bimage = new BitmapImage();

                var stream = await storeFile.OpenAsync(FileAccessMode.Read);

                bimage.SetSource(stream);

                CaptureImage = bimage;
            }
        }

        private async void DetectFaceAsync()
        {
            //var appView = Windows.UI.ViewManagement.ApplicationView.GetForCurrentView();

            //appView.Title = "Detecting...";
            
            // todo: Use DI to new up model providers
       
            var storeFile = await StorageFileProvider.GetCachedFileAsync();

            var capturedImageBytes = await StorageFileProvider.SaveToFileAsync(storeFile);

            var detectedFace = await InternalDetectFaceAsync(capturedImageBytes);

            var identifyResult = await InternalIdentifyFaceAsync(detectedFace);

            Person detectedPerson = null;

            string message;

            if (identifyResult == null || !identifyResult.Any() || !identifyResult.FirstOrDefault().Candidates.Any())
            {
                message = $"Failed to identify FaceId {detectedFace.FaceId.Value}. Do you wish to be added to the system?";

                //    // todo: log {detectFace.ToJson()} in richtext window

                //    appView.Title = message;

                //    // Customise dialog to ask to add person.
                detectedPerson = await AddUnIdentifiedPersonAsync(message, capturedImageBytes);
            }
            else
            {
                detectedPerson = await InternalIdentifyPerson(identifyResult);

                if (detectedPerson == null || detectedPerson.PersonId == Guid.Empty)
                {
                    message = $"Failed to find PersonId {identifyResult.FirstOrDefault().Candidates.First()} in the system.";

                    //appView.Title = message;

                    ShowErrorAsync(message);
                }
                else
                {
                    message = $"Welcome {detectedPerson.Name}. You were identified with {identifyResult.First().Candidates.First().Confidence * 100} confidence.";

                }
            }
            
            //appView.Title = result;

            var messageDialog = new MessageDialog(message, "IDENTIFICATION COMPLETE");

            await messageDialog.ShowAsync();
        }


        private async Task<DetectedFace> InternalDetectFaceAsync(Byte [] capturedImageBytes)
        {
            // DETECT
            var faceProvider = new FaceProvider(_controllerUri);

            var detectedFace = await faceProvider.DetectAsync(capturedImageBytes);

            if (detectedFace == null || detectedFace.FaceId == Guid.Empty)
            {
                var message = $"Failed to recgonise you";

                //    appView.Title = message;

                var addUnknownPerson = new MessageDialog(message, "Unrecognised Person");

                return null;
            }

            return detectedFace;
        }

        private async Task<ObservableCollection<IdentifyResult>> InternalIdentifyFaceAsync(DetectedFace detectedFace, int groupId = 1)
        {
            // IDENIFTY
            var faceProvider = new FaceProvider(_controllerUri);

            return await faceProvider.IdentifyAsync(detectedFace.FaceId.Value, groupId, null, null);

            // IDENIFTY
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
                AddPersonDialog addPersonDialog = new AddPersonDialog(_controllerUri);

                // todo: invoke voice activation to register name via voice/nat luangue
                ContentDialogResult dialogResult = await addPersonDialog.ShowAsync();

                // todo: use proper MVP to obtain new person
                var bail = 10;
                while (addPersonDialog.NewlyCreatedPerson == null && --bail > 0)
                {
                    await Task.Delay(1000);
                }

                // Add face to pertson and train
                var trainingProvider = new TrainingProvider(_controllerUri);

                trainingProvider.Train(addPersonDialog.NewlyCreatedPerson.PersonId, groupId, capturedImageBytes);

                await Task.Delay(1000);

                var personProvider = new PersonProvider(_controllerUri);

                newlyCreatedPerson = await personProvider.GetPersonAsync(addPersonDialog.NewlyCreatedPerson.PersonId, groupId);

                MessageDialog confirmDialog = new MessageDialog($"{newlyCreatedPerson.Name} added.", "Add Person");

                await confirmDialog.ShowAsync();
            }

            return newlyCreatedPerson;
        }

        private async Task<Person> InternalIdentifyPerson(ObservableCollection<IdentifyResult> identifyResult, int groupId = 1)
        {
            // GET PERSON
            var personProvider = new PersonProvider(_controllerUri);

            var candidateId = identifyResult.First().Candidates.First().PersonId;

            var detectedPerson = await personProvider.GetPersonAsync(candidateId, groupId);
            
            return detectedPerson;

            // GET PERSON

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

        //public async Task<ObservableCollection<IdentifyResult>> IdentifyFaceAsync(Guid faceId, int groupId, int? confidenceThreshold, int? maxPersonsToReturn)
        //{
        //    var visionClient = new VisionClient(_webServiceUri.AbsoluteUri);

        //    return await visionClient.IdentifyAsync(faceId, _groupId.ToString(), null, null);
        //}


        //public async Task<Person> GetPersonAsync(Guid candidateId, int groupId)
        //{
        //    var visionClient = new AdministrationClient(_webServiceUri.AbsoluteUri);

        //    return await visionClient.GetPersonAsync(candidateId, _groupId.ToString());
        //}

        //public async Task<DetectedFace> DetectFaceAsync(Byte[] imageAsBytes)
        //{
        //    var visionClient = new VisionClient(_webServiceUri.AbsoluteUri);

        //    return await visionClient.DetectAsync(imageAsBytes);
        //}

        //public async Task<PersistedFace> AddFaceToPerson(Guid personId, int groupId, byte [] faceCapture)
        //{           
        //    var adminClient = new AdministrationClient(_webServiceUri.AbsoluteUri);

        //    // todo: pass in primitives rather than json


        //    var asBase64 = Convert.ToBase64String(faceCapture);

        //    string json = @"{ personId:" + personId + ", groupId: " + groupId + ", faceCapture:" + asBase64 + "}";

        //    var request = JObject.Parse(json);

        //    var persistedFace = await adminClient.AddFaceToPersonAsync(request);

        //}

    }
}
