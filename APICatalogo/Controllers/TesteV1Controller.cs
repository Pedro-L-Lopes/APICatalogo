﻿using Asp.Versioning;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace APICatalogo.Controllers
{
    //[ApiVersion("1.0", Deprecated = true)]
    [ApiVersion("1.0")]
    //[Route("api/{v:apiVersion}/teste")]
    [Route("api/teste")]
    [ApiController]
    public class TesteV1Controller : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            return Content("<html><body> <h2>TesteV1Controller - V 1.0</h2> </body></html>", "text/html");
        }

        //[HttpGet, MapToApiVersion("2.0")]
        //public IActionResult GetVersao2()
        //{
        //   return Content("<html><body> <h2>TesteV1Controller - GET V2.0</h2> </body></html>", "text/html");
        //}
    }
}
