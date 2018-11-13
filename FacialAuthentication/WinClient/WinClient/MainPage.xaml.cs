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
using Windows.Media.Capture;
using Windows.UI.Xaml.Media.Imaging;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.Storage.Streams;
using Windows.UI.Popups;
//using System.Windows.Media.Imaging;
using System.Threading.Tasks;
using ClientProxy;
using Windows.UI.ViewManagement;
using Windows.Graphics.Imaging;
using System.Reflection;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace FaceAuth
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private StorageFile _storeFile;

        private IRandomAccessStream _stream;

        public MainPage()
        {
            this.InitializeComponent();
        }

        // todo: Recuce size of image. Either save or see https://stackoverflow.com/questions/23926454/reducing-byte-size-of-jpeg-file

        private async void captureBtn_Click(object sender, RoutedEventArgs e)
        {
            CameraCaptureUI capture = new CameraCaptureUI();
            capture.PhotoSettings.Format = CameraCaptureUIPhotoFormat.Jpeg;
            //capture.PhotoSettings.CroppedAspectRatio = new Size(3, 5);
            capture.PhotoSettings.MaxResolution = CameraCaptureUIMaxPhotoResolution.HighestAvailable;
            _storeFile = await capture.CaptureFileAsync(CameraCaptureUIMode.Photo);

            if (_storeFile != null)
            {

                var bytes = await SaveToFileAsync(_storeFile);

                var appView = Windows.UI.ViewManagement.ApplicationView.GetForCurrentView();

                appView.Title = "Detecting...";

                await WebApiProxy(bytes).ContinueWith((continutionTask) =>
                {
                    if (continutionTask.IsCompleted)
                    {
                        if (continutionTask.IsFaulted)
                        {
                            appView.Title = continutionTask.Exception.InnerException.Message;
                        }
                        else
                        {
                            appView.Title = $"Done. Individual is {continutionTask.Result.ToJson()}"; // todo: put result name
                        }
                    }

                });
            }          
        }

        private async void saveBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                FileSavePicker fs = new FileSavePicker();
                fs.FileTypeChoices.Add("Image", new List<string>() { ".jpeg" });
                fs.DefaultFileExtension = ".jpeg";
                fs.SuggestedFileName = "Image" + DateTime.Today.ToString();
                fs.SuggestedStartLocation = PickerLocationId.PicturesLibrary;
                fs.SuggestedSaveFile = _storeFile;
                // Saving the file
                var s = await fs.PickSaveFileAsync();
                if (s != null)
                {
                    using (var dataReader = new DataReader(_stream.GetInputStreamAt(0)))
                    {
                        await dataReader.LoadAsync((uint)_stream.Size);
                        byte[] buffer = new byte[(int)_stream.Size];
                        dataReader.ReadBytes(buffer);
                        await FileIO.WriteBytesAsync(s, buffer);
                    }
                }
            }
            catch (Exception ex)
            {
                var messageDialog = new MessageDialog("Unable to save now.");
                await messageDialog.ShowAsync();
            }
        }

        private async Task<Byte[]> SaveToFileAsync(StorageFile file)
        {
            Byte[] bytes = null;
            //var picker = new FileOpenPicker();
            //picker.ViewMode = PickerViewMode.Thumbnail;
            //picker.SuggestedStartLocation = PickerLocationId.PicturesLibrary;
            //picker.FileTypeFilter.Add(".jpeg");
            //picker.FileTypeFilter.Add(".jpg");
            //picker.FileTypeFilter.Add(".png");
            if (file != null)
            {
                var stream = await file.OpenStreamForReadAsync();
                bytes = new byte[(int)stream.Length];
                stream.Read(bytes, 0, (int)stream.Length);
                
            }

            return bytes;
        }

        private async Task<DetectedFace> WebApiProxy(Byte [] imageAsBytes)
        {
            var controllerUri = new Uri(@"http://localhost:5000/");

            var visionClient = new VisionClient(controllerUri.AbsoluteUri);

            var detectedFaces = await visionClient.IdentifyIndividualAsync(imageAsBytes);

            System.Diagnostics.Debug.Assert(detectedFaces == null);

            return detectedFaces;
        }

        private void clearBtn_Click(object sender, RoutedEventArgs e)
        {
            FacePhoto.Source = null;

            File.Delete(_storeFile.Path);
        }

        private void addPersonBtn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void trainBtn_Click(object sender, RoutedEventArgs e)
        {

        }

        private async void RenderResult(DetectedFace detectedFace)
        {
            if (detectedFace != null)
            {
                //var faceBitmap = new Bitmap imgBox.Image);

                //using (var g = Graphics.FromImage(faceBitmap))
                //{
                //    // Alpha-black rectangle on entire image
                //    g.FillRectangle(new SolidBrush(Color.FromArgb(200, 0, 0, 0)), g.ClipBounds);

                //    var br = new SolidBrush(Color.FromArgb(200, Color.LightGreen));

                //    // Loop each face recognized

                //        var fr = detectedFace.FaceRectangle;
                //        var fa = detectedFace.FaceAttributes;

                //        // Get original face image (color) to overlap the grayed image
                //        var faceRect = new Rectangle(fr.Left, fr.Top, fr.Width, fr.Height);
                //        g.DrawImage(imgBox.Image, faceRect, faceRect, GraphicsUnit.Pixel);
                //        g.DrawRectangle(Pens.LightGreen, faceRect);

                //        // Loop face.FaceLandmarks properties for drawing landmark spots
                //        var pts = new List<Point>();
                //        Type type = detectedFace.FaceLandmarks.GetType();
                //        foreach (PropertyInfo property in type.GetProperties())
                //        {
                //            g.DrawRectangle(Pens.LightGreen, GetRectangle((FeatureCoordinate)property.GetValue(detectedFace.FaceLandmarks, null)));
                //        }

                //        // Calculate where to position the detail rectangle
                //        int rectTop = fr.Top + fr.Height + 10;
                //        if (rectTop + 45 > faceBitmap.Height) rectTop = fr.Top - 30;

                //        // Draw detail rectangle and write face informations                     
                //        g.FillRectangle(br, fr.Left - 10, rectTop, fr.Width < 120 ? 120 : fr.Width + 20, 25);
                //        g.DrawString(string.Format("{0:0.0} / {1} / {2}", fa.Age, fa.Gender, fa.Emotion.OrderByDescending(x => x.Value).First().Key),
                //                     this.Font, Brushes.Black,
                //                     fr.Left - 8,
                //                     rectTop + 4);
                    
                //}

                //imgBox.Image = faceBitmap;
            }
        }
    }
}
