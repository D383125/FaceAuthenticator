using ClientProxy;
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
                var messageDialog = new MessageDialog("Unable to save now.");
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

        private async Task<DetectedFace> DetectFace(Byte [] imageAsBytes)
        {
            var visionClient = new Client(_controllerUri.AbsoluteUri);
   
            return await visionClient.ApiVisionDetectAsync(imageAsBytes);
        }

        private void clearBtn_Click(object sender, RoutedEventArgs e)
        {
            FacePhoto.Source = null;

            File.Delete(_storeFile.Path);

            authBtn.IsEnabled = false;
        }

        private void addPersonBtn_Click(object sender, RoutedEventArgs e)
        {
            // tpdp: Collect name etc
        }

        private void trainBtn_Click(object sender, RoutedEventArgs e)
        {

        }


        private async Task<System.Collections.ObjectModel.ObservableCollection<IdentifyResult>> IdentifyFaceAsync(Guid faceId, int groupId, int? confidenceThreshold, int? maxPersonsToReturn)
        {
            var visionClient = new Client(_controllerUri.AbsoluteUri);

            // Then using candities fron resulr obrin person with final call 
            return await visionClient.ApiVisionIdentifyAsync(faceId, GroupId.ToString(), null, null);
        }


        private async Task<Person> GetPersonAsync(Guid candidateId, int groupId = GroupId)
        {
            var visionClient = new Client(_controllerUri.AbsoluteUri);

            return await visionClient.ApiAdministrationGetpersonAsync(candidateId, GroupId.ToString());
        }




        private async void authBtn_Click(object sender, RoutedEventArgs e)
        {
            var appView = Windows.UI.ViewManagement.ApplicationView.GetForCurrentView();

            appView.Title = "Detecting...";

            await DetectFace(_capturedImageBytes).ContinueWith((continutionTask) =>
            {
            if (continutionTask.IsCompleted)
            {
                if (continutionTask.IsFaulted)
                {
                    appView.Title = continutionTask.Exception.InnerException.Message;
                }
                else
                {
                    // Chain. Now we need to identify detected face

                    var detectedFaceId = continutionTask.Result.FaceId;
                    // todo: log other attributes
                    if (detectedFaceId != Guid.Empty)
                    {
                        // Invoke identify
                        var visionClient = new Client(_controllerUri.AbsoluteUri);

                        // Then using candities fron resulr obrin person with final call 
                        var identifyResultTask = visionClient.ApiVisionIdentifyAsync(detectedFaceId.Value, GroupId.ToString(), null, null);

                        identifyResultTask.ContinueWith((inneridentifyResultTask) =>
                            {
                                if(inneridentifyResultTask.Result.Any() && inneridentifyResultTask.Result.First().Candidates.Any())
                                {
                                    var candidateId = inneridentifyResultTask.Result.First().Candidates[0].PersonId;

                                    var personTask = visionClient.ApiAdministrationGetpersonAsync(candidateId, GroupId.ToString());

                                    personTask.ContinueWith((innerPersonTask) => 
                                    {
                                        if(innerPersonTask.Result.PersonId != Guid.Empty)
                                        {
                                            var result = $"Welcome {innerPersonTask.Result.Name}";

                                            appView.Title = result;

                                            MessageDialog messageDialog = new MessageDialog(result, "IDENTIFICATION COMPLETE");

                                            messageDialog.ShowAsync();
                                        }
                                    });
                                }
                                else
                                {
                                    // todo: notify
                                    var result = "Could not idenify person";

                                    appView.Title = $"Done. {result}"; // todo: put result name

                                    MessageDialog messageDialog = new MessageDialog(result, "IDENTIFICATION COMPLETE");

                                    messageDialog.ShowAsync();
                                }

                            });

                            // Train

                        }
                    else
                        {
                            // todo: notify
                            var result = "Could not idenify person";

                            appView.Title = $"Done. {result}"; // todo: put result name

                            MessageDialog messageDialog = new MessageDialog(result, "IDENTIFICATION COMPLETE");


                            messageDialog.ShowAsync();
                        }

                                         }
                }

            });
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
