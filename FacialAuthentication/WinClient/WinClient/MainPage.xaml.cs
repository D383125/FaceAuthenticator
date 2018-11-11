﻿using System;
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
                BitmapImage bimage = new BitmapImage();
                _stream = await _storeFile.OpenAsync(FileAccessMode.Read);

                // File.WriteAllBytes(path, bytes);

                Windows.Graphics.Imaging.BitmapDecoder decoder = await Windows.Graphics.Imaging.BitmapDecoder.CreateAsync(_stream);
                Windows.Graphics.Imaging.PixelDataProvider pixelData = await decoder.GetPixelDataAsync();
                byte[] bytes = pixelData.DetachPixelData();

                bimage.SetSource(_stream);
                FacePhoto.Source = bimage;

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
                            appView.Title = $"Done. Deteched individual is {continutionTask.Result}"; // todo: put result name
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

        private void SaveToFile(byte [] bytes, string path)
        {
            File.WriteAllBytes( path, bytes);

            //FileSavePicker fs = new FileSavePicker();
            //fs.FileTypeChoices.Add("Image", new List<string>() { ".jpeg" });
            //fs.DefaultFileExtension = ".jpeg";
            //fs.SuggestedFileName = filename; // "Image" + DateTime.Today.ToString();
            //fs.SuggestedStartLocation = PickerLocationId.PicturesLibrary;
            //fs.SuggestedSaveFile = _storeFile;
            //// Saving the file
            //var s = await fs.PickSaveFileAsync();
            //if (s != null)
            //{
            //    using (var dataReader = new DataReader(_stream.GetInputStreamAt(0)))
            //    {
            //        await dataReader.LoadAsync((uint)_stream.Size);
            //        byte[] buffer = new byte[(int)_stream.Size];
            //        dataReader.ReadBytes(buffer);
            //        await FileIO.WriteBytesAsync(s, buffer);
            //    }
            //}
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
    }
}
