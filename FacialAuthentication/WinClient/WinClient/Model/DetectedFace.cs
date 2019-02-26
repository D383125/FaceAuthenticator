using System;
using System.Dynamic;
using Windows.UI.Xaml.Shapes;
using ClientProxy;
using Newtonsoft.Json.Linq;
using System.Text;

namespace FaceAuth.Model
{
    public class DetectedFace
    {
        private readonly dynamic _jsonResponse;

        public Guid FaceId { get; }

        public double Age { get; }

        public Rectangle FaceRectangle { get; }

        public double Smile { get; }

        public dynamic FaceAttributes { get; }

        public dynamic FacialLandmarks { get; }

        public dynamic Emotion { get; }

        public dynamic Hair { get; }

        public dynamic HairColour { get; }

        public dynamic Makeup { get; }

        public dynamic Occlusion { get; }

        public dynamic Accessories { get; }

        public dynamic Blur { get; }

        public dynamic Exposure { get; }

        public dynamic Noise { get; }



        public DetectedFace(string detectFaceResponse)
        {
            _jsonResponse = JObject.Parse(detectFaceResponse);

            FaceId = Guid.Parse(_jsonResponse["faceId"].ToString());

            Age = Convert.ToDouble(_jsonResponse["age"].ToString());

            Smile = Convert.ToDouble(_jsonResponse["smile"].ToString());

            var faceAttrJson = _jsonResponse["faceAttributes"];

            FaceAttributes = new
            {
                Age = Convert.ToDouble(faceAttrJson["age"]),
                Gender = Convert.ToString(faceAttrJson["gender"]),
                Smile = Convert.ToDouble(faceAttrJson["smile"]),
                FacialHair = Convert.ToString(faceAttrJson["facialHair"]),
                Glasses = Convert.ToString(faceAttrJson["glasses"]),
                HeadPose = Convert.ToString(faceAttrJson["headPose"])
            };

            Emotion = new
            {
                Anger = Convert.ToDouble(faceAttrJson["emotion"]["anger"]),
                Contempt = Convert.ToDouble(faceAttrJson["emotion"]["contempt"]),
                Disgust = Convert.ToDouble(faceAttrJson["emotion"]["disgust"]),
                Fear = Convert.ToDouble(faceAttrJson["emotion"]["fear"]),
                Happiness = Convert.ToDouble(faceAttrJson["emotion"]["happiness"]),
                Neutral = Convert.ToDouble(faceAttrJson["emotion"]["neutral"]),
                Sadness = Convert.ToDouble(faceAttrJson["emotion"]["sadness"]),
                Suprise = Convert.ToDouble(faceAttrJson["emotion"]["surprise"])

            };

            Makeup = faceAttrJson["makeup"] ?? 0;
            Occlusion = faceAttrJson["occlusion"] ?? 0;
            Accessories = faceAttrJson["accessories"];
            Blur = faceAttrJson["blur"] ?? 0;
            Exposure = faceAttrJson["exposure"] ?? 0;
            Noise = faceAttrJson["noise"] ?? 0;

            Hair = new
            {
                Bald = Convert.ToDouble(faceAttrJson["hair"]["bald"]),
                Invisible = Convert.ToBoolean(faceAttrJson["hair"]["invisible"])
                // todo: hair colour
            };

            FacialLandmarks = _jsonResponse["facialLandmarks"];

            FaceRectangle = new Rectangle()
            {
                Width = Convert.ToDouble(_jsonResponse["rectangle"].Value.Split(',')[0]),
                Height = Convert.ToDouble(_jsonResponse["rectangle"].Value.Split(',')[1])
            };
        }

        public string ToJson()
        {
            return _jsonResponse.ToString();
        }

        public override string ToString()
        {
            // todo: finish mapping abov
            // see https://docs.microsoft.com/en-us/azure/cognitive-services/Face/Tutorials/FaceAPIinCSharpTutorial
            var sb = new StringBuilder();

            sb.Append("Face: ");

            // Add the gender, age, and smile.
            sb.Append(FaceAttributes.Gender);
            sb.Append(", ");
            sb.Append(FaceAttributes.Age);
            sb.Append(", ");
            sb.Append(String.Format("smile {0:F1}%, ", FaceAttributes.Smile * 100));

            try
            {
                // Add the emotions. Display all emotions over 10%.
                //sb.Append("Emotion: ");
                //var emotionScores = FaceAttributes.Emotion;
                //if (emotionScores.Anger >= 0.1f) sb.Append(
                //    String.Format("anger {0:F1}%, ", emotionScores.Anger * 100));
                //if (emotionScores.Contempt >= 0.1f) sb.Append(
                //    String.Format("contempt {0:F1}%, ", emotionScores.Contempt * 100));
                //if (emotionScores.Disgust >= 0.1f) sb.Append(
                //    String.Format("disgust {0:F1}%, ", emotionScores.Disgust * 100));
                //if (emotionScores.Fear >= 0.1f) sb.Append(
                //    String.Format("fear {0:F1}%, ", emotionScores.Fear * 100));
                //if (emotionScores.Happiness >= 0.1f) sb.Append(
                //    String.Format("happiness {0:F1}%, ", emotionScores.Happiness * 100));
                //if (emotionScores.Neutral >= 0.1f) sb.Append(
                //    String.Format("neutral {0:F1}%, ", emotionScores.Neutral * 100));
                //if (emotionScores.Sadness >= 0.1f) sb.Append(
                //    String.Format("sadness {0:F1}%, ", emotionScores.Sadness * 100));
                //if (emotionScores.Surprise >= 0.1f) sb.Append(
                //    String.Format("surprise {0:F1}%, ", emotionScores.Surprise * 100));

                // Add glasses.
                sb.Append(FaceAttributes.Glasses);
                sb.Append(", ");

                // Add hair.
                sb.Append("Hair: ");

                // Display baldness confidence if over 1%.
                if (FaceAttributes.Hair.Bald >= 0.01f)
                    sb.Append(String.Format("bald {0:F1}% ", FaceAttributes.Hair.Bald * 100));

                // Display all hair color attributes over 10%.
                var hairColors = FaceAttributes.Hair.HairColor;
                //foreach (var hairColor in hairColors)
                //{
                //    if (hairColor.Confidence >= 0.1f)
                //    {
                //        sb.Append(hairColor.Color.ToString());
                //        sb.Append(String.Format(" {0:F1}% ", hairColor.Confidence * 100));
                //    }
                //}

            }                        
            catch (Exception)
            {

            }
            // Return the built string.
            return sb.ToString();
        }

    }
}
