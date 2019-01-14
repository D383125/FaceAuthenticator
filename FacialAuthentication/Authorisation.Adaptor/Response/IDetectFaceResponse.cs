using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;


namespace Authorisation.Adaptor.Response
{
    public interface IDetectFaceResponse
    {
        Guid? FaceId { get; }

        Point Rectangle { get; }

        double? Age { get; }

        double? Smile { get; }

        string Emotion { get; }

        //todo: complete
    }
}
