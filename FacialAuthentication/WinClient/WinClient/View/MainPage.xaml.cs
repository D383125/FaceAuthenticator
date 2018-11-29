﻿using ClientProxy;
using FaceAuth.ViewModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
//using System.Windows.Media.Imaging;
using System.Threading.Tasks;
using Windows.Media.Capture;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.Storage.Streams;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;


using FaceAuth.View;
// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace FaceAuth
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private const int GroupId = 1;

        private const int ConfidenceThreshold = 1;

        private const int MaxNoOfCandidates = 1;

        private StorageFile _storeFile;

        private Byte[] _capturedImageBytes;

        private readonly Uri _controllerUri = new Uri(@"http://localhost:5000/");



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
                _capturedImageBytes = await SaveToFileAsync(_storeFile);

                BitmapImage bimage = new BitmapImage();

                var stream = await _storeFile.OpenAsync(FileAccessMode.Read);

                bimage.SetSource(stream);

                FacePhoto.Source = bimage;
                
                //string result = System.Text.Encoding.UTF8.GetString(bytes);

                authBtn.IsEnabled = true;
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
                    IRandomAccessStream stream = null;

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

        private void clearBtn_Click(object sender, RoutedEventArgs e)
        {
            FacePhoto.Source = null;

            File.Delete(_storeFile.Path);

            authBtn.IsEnabled = false;
        }

        private async void addPersonBtn_Click(object sender, RoutedEventArgs e)
        {
            AddPersonDialog addPersonDialog = new AddPersonDialog();

            await addPersonDialog.ShowAsync();
        }

        private async void trainBtn_Click(object sender, RoutedEventArgs e)
        {
            TrainDialog trainDialog = new TrainDialog();

            await trainDialog.ShowAsync();
        }


        private async void ShowErrorAsync(string message)
        {
            var messageDialog = new MessageDialog(message);

            await messageDialog.ShowAsync();
        }


        private async void authBtn_Click(object sender, RoutedEventArgs e)
        {
            var appView = Windows.UI.ViewManagement.ApplicationView.GetForCurrentView();

            appView.Title = "Detecting...";

            string message;

            var mainPageViewModel = new MainPageViewModel(_controllerUri, GroupId);

            var detectFace = await mainPageViewModel.DetectFaceAsync(_capturedImageBytes);

            if(detectFace.FaceId == Guid.Empty)
            {
                message = "Failed to detect person";

                appView.Title = message;

                ShowErrorAsync(message);

                return;
            }

            // var identifyResultTask = visionClient.ApiVisionIdentifyAsync(detectedFaceId.Value, GroupId.ToString(), null, null);
            var identifyResult = await mainPageViewModel.IdentifyFaceAsync(detectFace.FaceId.Value, GroupId, null, null);

            // if(inneridentifyResultTask.Result.Any() && inneridentifyResultTask.Result.First().Candidates.Any())
            if(identifyResult == null || !identifyResult.Any() || !identifyResult.FirstOrDefault().Candidates.Any())
            {
                message = $"Failed to identify person with FaceId {detectFace.FaceId.Value}";

                appView.Title = message;

                ShowErrorAsync(message);

                return;
            }

            var candidateId = identifyResult.First().Candidates.First().PersonId;

            var detectedPerson = await mainPageViewModel.GetPersonAsync(candidateId, GroupId);

            if(detectedPerson == null || detectedPerson.PersonId == Guid.Empty)
            {
                message = $"Failed to find PersonId {candidateId} in the system.";

                appView.Title = message;

                ShowErrorAsync(message);

                return;
            }


            var result = $"Welcome {detectedPerson.Name}. You were identified with {identifyResult.First().Candidates.First().Confidence * 100} confidence.";

            appView.Title = result;

            var messageDialog = new MessageDialog(result, "IDENTIFICATION COMPLETE");

            await messageDialog.ShowAsync();
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


        private string EncodeImageAsBase64(byte[] image)
        {
            //byte[] imageArray = System.IO.File.ReadAllBytes(@"image file path");
            return Convert.ToBase64String(image);
        }

        private static byte[] DecodeBase64AsBytesArray(string base64String)
        {
            // var img = Image.FromStream(new MemoryStream(Convert.FromBase64String(base64String)));
            return Convert.FromBase64String(base64String);
        }

    }
}