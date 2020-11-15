using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using SharedCode.Models;
using ReportGenerator.RabbitMQ;


namespace ReportGenerator.Controllers
{
    public class QueeReportGeneratorController : ApiController
    {
        // POST api/message
        public IHttpActionResult Post([FromBody] Envelope message)
        {
            try
            {
                RabbitMQClient client = new RabbitMQClient();
                client.SendReport(message);
                client.Close();
            }
            catch (Exception)
            {
                return StatusCode(HttpStatusCode.BadRequest);
            }

            return Ok(message);
        }
    }


}

