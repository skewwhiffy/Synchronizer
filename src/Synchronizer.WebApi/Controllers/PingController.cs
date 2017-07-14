using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Synchronizer.WebApi.Controllers
{
    [Route("[controller]")]
    public class PingController : Controller
    {
        [HttpGet]
        public string Get()
        {
            return "pong";
        }
    }
}
