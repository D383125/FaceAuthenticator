using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

using Authorization.Contracts;

namespace AuthorisationWebApi.Controllers
{
    [Route("api/[controller]")]
    public class AudioController : Controller
    {

        private readonly ISpeechService _speechService;

        public AudioController()
        {

        }

        //public AudioController(ISpeechService speechService)
        //{
        //    _speechService = speechService;
        //}

        public async Task<string> Get()
        {
            return await Task<string>.Factory.StartNew(() => "ACK");
        }
    }
}
