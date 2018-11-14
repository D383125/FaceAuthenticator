using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Authorization.Contracts;

namespace AuthorisationWebApi.Controllers
{
   [Route("api/[controller]")]
    public class TrainingController : Controller 
    {
        private readonly ITrainingVisionService _trainingVisionService;


        public TrainingController()
        {
            
        }
        //public TrainingController(ITrainingVisionService trainingVisionService)
        //{
        //    _trainingVisionService = trainingVisionService;
        //}

        public async Task<string> Get()
        {
            return await Task<string>.Factory.StartNew(() => "ACK");
        }
    }
}
