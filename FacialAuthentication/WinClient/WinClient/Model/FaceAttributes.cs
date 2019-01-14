using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FaceAuth.Model
{
    public class FaceAttributes
    {
        public double? Age { get; set; }

        public int Confidence { get; set; }

        public double? Smile { get; set; }

        public string Emotion { get; set; }

        public FaceAttributes(string response)
        {

        }

    }
}
