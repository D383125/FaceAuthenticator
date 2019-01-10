using Microsoft.Azure.CognitiveServices.Vision.Face.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Authorisation.Adaptor.Response
{
    public class TrainGroupResponse : ITrainGroupResponse
    {
        private readonly TrainingStatus _trainingStatus;

        public DateTime? LastSuccessfulTraining => _trainingStatus.LastSuccessfulTraining;

        public DateTime? LastAction => _trainingStatus.LastAction;

        public string Message => _trainingStatus.Message;

        public string Status => _trainingStatus.Status.ToString();

        public TrainGroupResponse(TrainingStatus trainingStatus)
        {
            trainingStatus.Validate();

            _trainingStatus = trainingStatus;
        }

    }
}
