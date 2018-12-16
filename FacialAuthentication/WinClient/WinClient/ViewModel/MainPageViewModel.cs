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

namespace FaceAuth.ViewModel
{
    // Type breches SRP?
    public class MainPageViewModel : ObservableObject
    {
        //private readonly Uri _webServiceUri;
        //private readonly int _groupId;

  //      private const int GroupId = 1;

 //       private const int ConfidenceThreshold = 1;

 //       private const int MaxNoOfCandidates = 1;

   //     private volatile StorageFile _storeFile;

        private const string CachedCaptureKey = "InSessionCapture";

//        private Byte[] _capturedImageBytes;

        private readonly Uri _controllerUri = new Uri(@"http://localhost:5000/");



        public MainPageViewModel()
        {
           // _webServiceUri = webServiceUri;

            //_groupId = groupId;
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
            get { return new DelegateCommand(DetectFace, () => true); }
        }

        public ICommand TrainCommand
        {
            get { return new DelegateCommand(ShowTrainDialogAsync, () => true); }
        }

        public ICommand SaveCommand
        {
            get { return new DelegateCommand(SaveCapture, () => true); }
        }

        public ICommand ClearCommand
        {
            get { return new DelegateCommand(ClearCapture, () => true); }
        }

        public ICommand AddPersonCommand
        {
            get { return new DelegateCommand(ShowAddPersonDialogAsync, () => false); }
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

            PurgeLocalCacheAsync();

            //authBtn.IsEnabled = false;
        }

        private void SaveCapture()
        {

        }

        private async void CaptureFace()
        {
            // todo: move into seperate class
            CameraCaptureUI capture = new CameraCaptureUI();
            capture.PhotoSettings.Format = CameraCaptureUIPhotoFormat.Jpeg;
            //capture.PhotoSettings.CroppedAspectRatio = new Size(3, 5);
            capture.PhotoSettings.MaxResolution = CameraCaptureUIMaxPhotoResolution.HighestAvailable;

            var storeFile = await capture.CaptureFileAsync(CameraCaptureUIMode.Photo);

            CacheCaptureAsync(storeFile);

            if (storeFile != null)
            {
                var capturedImageBytes = await SaveToFileAsync(storeFile);

                BitmapImage bimage = new BitmapImage();

                var stream = await storeFile.OpenAsync(FileAccessMode.Read);

                bimage.SetSource(stream);

                CaptureImage = bimage;
            }
        }

        private void DetectFace()
        {
            System.Diagnostics.Debugger.Break();
            // bind and run
        }

        private async void ShowTrainDialogAsync()
        {
            var trainDialog = new TrainDialog();

            await trainDialog.ShowAsync();
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


        private async Task<Byte[]> SaveToFileAsync(StorageFile file)
        {
            Byte[] bytes = null;

            if (file != null)
            {
                var stream = await file.OpenStreamForReadAsync();
                bytes = new byte[(int)stream.Length];
                stream.Read(bytes, 0, (int)stream.Length);
            }

            return bytes;
        }

        // Place in capturte

        private async void CacheCaptureAsync(StorageFile file)
        {
            //Create dataFile.txt in LocalFolder and write “My text” to it 
            StorageFolder localFolder = ApplicationData.Current.LocalFolder;
            await file.CopyAsync(ApplicationData.Current.LocalFolder, CachedCaptureKey, NameCollisionOption.ReplaceExisting);
        }

        private async void PurgeLocalCacheAsync()
        {
            var localFolder = ApplicationData.Current.LocalFolder;

            await localFolder.DeleteAsync(StorageDeleteOption.PermanentDelete);
        }

        private async Task<StorageFile> GetCachedFileAsync()
        {
            var localFolder = ApplicationData.Current.LocalFolder;

            return await localFolder.GetFileAsync(CachedCaptureKey);
        }

        // place in capture

    }
}
