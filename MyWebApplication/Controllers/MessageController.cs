using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using MyWebApplication.Models;
using MyWebApplication.Services;

namespace MyWebApplication.Controllers
{
    public class MessageController : ApiController
    {
        private ReportGeneratorService _reportGeneratorService;

        public MessageController()
        {
            _reportGeneratorService = new ReportGeneratorService();
        }


        // GET api/values
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }


        // POST api/message
        public HttpResponseMessage Post([FromBody] Envelope message)
        {
           return _reportGeneratorService.GetReport(message);
        }


    }
}
