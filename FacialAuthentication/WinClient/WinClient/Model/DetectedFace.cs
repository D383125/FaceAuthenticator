using System;
using System.Dynamic;
using Windows.UI.Xaml.Shapes;
using ClientProxy;
using Newtonsoft.Json.Linq;

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
            return _jsonResponse;
        }

    }
}
