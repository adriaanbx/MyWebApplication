using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MyWebApplication.Models;
using Microsoft.Ajax.Utilities;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Net;
using SharedCode.Models.Payslip;
using SharedCode.Models.TempUnemployment;

namespace MyWebApplication.Services
{
    public class ReportGeneratorService
    {
      
        public HttpResponseMessage GetReport(Envelope message)
        {
            string outputType = message.Payload.GenerateDocument.OutputType;
            HttpResponseMessage parameters = default;

            switch (outputType)
            {
                case "XML":
                    parameters = CreateXmlReport(message);
                    break;
                case "HTML":
                    parameters = CreateHtmlReport(message);
                    break;
                case "PDF":
                    break;
                default:
                    break;
            }
            return parameters;
        }


        private HttpResponseMessage CreateXmlReport(Envelope message)
        {
            var response = CreateXmlResponse<Parameters>(message.Payload.GenerateDocument.Parameters);
            return response;
        }

        private HttpResponseMessage CreateHtmlReport(Envelope message)
        {
            StringBuilder stringcontent = new StringBuilder();

            stringcontent.Append("<h1>Payload</h1>");
            stringcontent.AppendLine("<div>");
            stringcontent.AppendLine("Name: " + message.Payload.GenerateDocument.Parameters.Name);
            stringcontent.AppendLine("Surname: " + message.Payload.GenerateDocument.Parameters.Surname);
            stringcontent.AppendLine("Street: " + message.Payload.GenerateDocument.Parameters.Address.Street);
            stringcontent.AppendLine("Number: " + message.Payload.GenerateDocument.Parameters.Address.Number);
            stringcontent.AppendLine("City: " + message.Payload.GenerateDocument.Parameters.Address.City);
            stringcontent.AppendLine("PostalCode: " + message.Payload.GenerateDocument.Parameters.Address.PostalCode);
            stringcontent.AppendLine("</div>");

            var response = new HttpResponseMessage();
            response.Content = new StringContent(stringcontent.ToString());
            response.Content.Headers.ContentType = new MediaTypeHeaderValue("text/html");
            return response;

        }

        public static HttpResponseMessage CreateXmlResponse<T>(T objectToSerialize)
        {
            return new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new ObjectContent<T>(objectToSerialize,
                                                new System.Net.Http.Formatting.XmlMediaTypeFormatter
                                                {
                                                    UseXmlSerializer = true
                                                })
            };
        }
    }
}