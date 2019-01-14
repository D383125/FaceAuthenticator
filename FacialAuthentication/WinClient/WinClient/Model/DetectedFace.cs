using System;
using System.Dynamic;
using Windows.UI.Xaml.Shapes;
using ClientProxy;
using Newtonsoft.Json.Linq;

namespace FaceAuth.Model
{
    public class DetectedFace
    {
        public Guid FaceId { get; }

        public double Age { get; }

        public Rectangle FaceRectangle { get; }

        public double Smile { get; }

        public dynamic Emotion { get; }


        public DetectedFace(string detectFaceResponse)
        {
            dynamic response = JObject.Parse(detectFaceResponse);

            FaceId = Guid.Parse(response["faceId"].ToString());

            Age = Convert.ToDouble(response["age"].ToString());

            Smile = Convert.ToDouble(response["smile"].ToString());

            Emotion = response["emotion"];

            FaceRectangle = new Rectangle()
            {
                Width = Convert.ToDouble(response["rectangle"].Value.Split(',')[0]),
                Height = Convert.ToDouble(response["rectangle"].Value.Split(',')[1])
        };
        }
    }
}
