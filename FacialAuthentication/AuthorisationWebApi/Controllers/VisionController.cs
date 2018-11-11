using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.CognitiveServices.Vision.Face;
using Microsoft.Azure.CognitiveServices.Vision.Face.Models;

namespace AuthorisationWebApi.Controllers
{

/// Create ML.Net service using both Deep Learning and Anomaly detection

    [Route("api/[controller]")]
    public class VisionController : Controller
    {
        //const string _baseUri = "https://australiaeast.api.cognitive.microsoft.com/face/v1.0";


        const string _baseUri = "https://australiaeast.api.cognitive.microsoft.com"; // work around for Resource not found


        private const string _subscriptionKey = "77a68897922a41608473f4208b2a3f5c";
/* 
        [HttpGet]
        public void Add()
        {

        }

        
        public Task<int> Get()
        {
            // In faceListListId and persistedFaceId
            throw new NotImplementedException();
        }


        [HttpPatch]
        public void Update()
        {

        }

        [HttpDelete]
        public void Delete()
        {

        }

        // These methods will be ion service
        // Chain of Repso for detech, identify andd Validate
        #region Place In Service/brokerware
        [HttpPost("Verify")]
        public async Task<JsonResult> Verify([FromBody] Byte [] personCaptureAsBytes)
        {
            var client = new HttpClient();
            var queryString = HttpUtility.ParseQueryString(string.Empty);

            // Request headers
            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", _subscriptionKey);

            var uri = "https://westus.api.cognitive.microsoft.com/face/v1.0/identify?" + queryString;

            HttpResponseMessage response;

            // Request body
           // byte[] byteData = Encoding.UTF8.GetBytes("{body}");

            using (var content = new ByteArrayContent(personCaptureAsBytes))
            {
               content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
               response = await client.PostAsync(uri, content);
            }
            
            return Json(response);
        } 

        [HttpPost("IdentifyEx")]
        public async Task<JsonResult> IdentifyEx([FromBody] Byte [] personCaptureAsBytes)
        {
            throw new NotImplementedException();
        }

        [HttpPost("Detect")]
        public async Task<JsonResult> Detect([FromBody] Byte [] personCaptureAsBytes)
        {
            throw new NotImplementedException();
        }

        
        [HttpPost("FindSimilar")]
        public async Task<JsonResult> FindSimilar([FromBody] Byte [] personCaptureAsBytes)
        {
            throw new NotImplementedException();
        }
*/

        [HttpPost()]
        public async Task<DetectedFace> Identify([FromBody] Byte [] personCaptureAsBytes)
        {
            // todo: place in autofaced serviuce
            // if byte array doesnt work pass as fle in uri from client.
           
            // Test
            // personCaptureAsBytes = System.IO.File.ReadAllBytes(@"C:\Users\breen\Desktop\CCapture.jpg.jpeg");
            // Test

             //"https://westcentralus.api.cognitive.microsoft.com/face/v1.0";
            // https://[location].api.cognitive.microsoft.com/face/v1.0/identify
            var faceServiceUri = new Uri(_baseUri);

            DetectedFace detectedFace = null;

            IFaceClient faceClient = new FaceClient(
            new ApiKeyServiceClientCredentials(_subscriptionKey),
            new System.Net.Http.DelegatingHandler[] { });

            faceClient.Endpoint = faceServiceUri.AbsoluteUri;

            // The list of Face attributes to return.
            IList<FaceAttributeType> faceAttributes =
                new FaceAttributeType[]
                {
                    FaceAttributeType.Gender, FaceAttributeType.Age,
                    FaceAttributeType.Smile, FaceAttributeType.Emotion,
                    FaceAttributeType.Glasses, FaceAttributeType.Hair
                };

            // Call the Face API.
            try
            {
                using (Stream imageFileStream = new MemoryStream(personCaptureAsBytes))
                {
                    // The second argument specifies to return the faceId, while
                    // the third argument specifies not to return face landmarks.

                     System.Diagnostics.Debugger.Break();

                    IList<DetectedFace> faceList = 
                        await faceClient.Face.DetectWithStreamAsync(
                            imageFileStream, true, false, faceAttributes);
                    
                    
                     System.Diagnostics.Debugger.Break();


                    var detectedFaces = faceList.Count; 

                    if(detectedFaces > 1)
                    {
                        throw new ApplicationException($"{detectedFaces} people detected.");
                    }

                    detectedFace = faceList.FirstOrDefault();
                }
            }
            catch (APIErrorException aex)
            {
                
                     System.Diagnostics.Debugger.Break();

                System.Diagnostics.Debug.WriteLine(aex);

/* 
                var filename  = "Temp.jpg";

                System.IO.File.WriteAllBytes(filename, personCaptureAsBytes);

                //var t = await MakeAnalysisRequest(filename);

                 IList<DetectedFace> faceList =
                    await faceClient.Face.DetectWithUrlAsync(
                        filename, true, false, faceAttributes);
  */              

                throw aex;
            }
            catch(Exception)
            {
                throw;
            }

            return detectedFace;
        }



/// <summary>
        /// Gets the analysis of the specified image by using the Face REST API.
        /// </summary>
        /// <param name="imageFilePath">The image file.</param>
        static async void MakeAnalysisRequest(string imageFilePath)
        {
            HttpClient client = new HttpClient();

            // Request headers.
            client.DefaultRequestHeaders.Add(
                "Ocp-Apim-Subscription-Key", _subscriptionKey);

            // Request parameters. A third optional parameter is "details".
            string requestParameters = "returnFaceId=true&returnFaceLandmarks=false" +
                "&returnFaceAttributes=age,gender,headPose,smile,facialHair,glasses," +
                "emotion,hair,makeup,occlusion,accessories,blur,exposure,noise";

            // Assemble the URI for the REST API Call.
            string uri = _baseUri + @"/detect" + "?" + requestParameters;

            HttpResponseMessage response;

            // Request body. Posts a locally stored JPEG image.
            byte[] byteData = GetImageAsByteArray(imageFilePath);

            using (ByteArrayContent content = new ByteArrayContent(byteData))
            {
                // This example uses content type "application/octet-stream".
                // The other content types you can use are "application/json"
                // and "multipart/form-data".
                content.Headers.ContentType =
                    new MediaTypeHeaderValue("application/octet-stream");

                // Execute the REST API call.
                response = await client.PostAsync(uri, content);

                // Get the JSON response.
                string contentString = await response.Content.ReadAsStringAsync();

                // Display the JSON response.
                Console.WriteLine("\nResponse:\n");
                Console.WriteLine(JsonPrettyPrint(contentString));
                Console.WriteLine("\nPress Enter to exit...");
            }
        }


        /// <summary>
        /// Returns the contents of the specified file as a byte array.
        /// </summary>
        /// <param name="imageFilePath">The image file to read.</param>
        /// <returns>The byte array of the image data.</returns>
        static byte[] GetImageAsByteArray(string imageFilePath)
        {
            using (FileStream fileStream =
                new FileStream(imageFilePath, FileMode.Open, FileAccess.Read))
            {
                BinaryReader binaryReader = new BinaryReader(fileStream);
                return binaryReader.ReadBytes((int)fileStream.Length);
            }
        }


        /// <summary>
        /// Formats the given JSON string by adding line breaks and indents.
        /// </summary>
        /// <param name="json">The raw JSON string to format.</param>
        /// <returns>The formatted JSON string.</returns>
        static string JsonPrettyPrint(string json)
        {
            if (string.IsNullOrEmpty(json))
                return string.Empty;

            json = json.Replace(Environment.NewLine, "").Replace("\t", "");

            StringBuilder sb = new StringBuilder();
            bool quote = false;
            bool ignore = false;
            int offset = 0;
            int indentLength = 3;

            foreach (char ch in json)
            {
                switch (ch)
                {
                    case '"':
                        if (!ignore) quote = !quote;
                        break;
                    case '\'':
                        if (quote) ignore = !ignore;
                        break;
                }

                if (quote)
                    sb.Append(ch);
                else
                {
                    switch (ch)
                    {
                        case '{':
                        case '[':
                            sb.Append(ch);
                            sb.Append(Environment.NewLine);
                            sb.Append(new string(' ', ++offset * indentLength));
                            break;
                        case '}':
                        case ']':
                            sb.Append(Environment.NewLine);
                            sb.Append(new string(' ', --offset * indentLength));
                            sb.Append(ch);
                            break;
                        case ',':
                            sb.Append(ch);
                            sb.Append(Environment.NewLine);
                            sb.Append(new string(' ', offset * indentLength));
                            break;
                        case ':':
                            sb.Append(ch);
                            sb.Append(' ');
                            break;
                        default:
                            if (ch != ' ') sb.Append(ch);
                            break;
                    }
                }
            }

            return sb.ToString().Trim();
        }

        //private void TryService(Byte [] stream)
        //{

        //    IFaceServiceClient faceServiceClient = new FaceServiceClient(_subscriptionKey);
        //}

      //  #endregion Place In Service/brokerware
    }
}