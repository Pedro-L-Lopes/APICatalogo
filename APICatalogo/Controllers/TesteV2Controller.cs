﻿using Asp.Versioning;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace APICatalogo.Controllers
{
    [ApiVersion("2.0")]
    [Route("api/teste2")]
    [ApiController]
    public class TesteV2Controller : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            return Content("<html><body> <h2>TesteV1Controller - V 2.0</h2> </body></html>", "text/html");
        }
    }
}
