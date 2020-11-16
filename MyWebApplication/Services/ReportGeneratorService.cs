using ReportGenerator.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SharedCode.Models.Payslip;
using SharedCode.Models.TempUnemployment;
using Microsoft.Ajax.Utilities;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Net;

namespace ReportGenerator.Services
{
    public class ReportGeneratorService
    {

    //    public HttpResponseMessage GetReport(Payslip message)
    //    {
    //        string outputType = message.Envelope.Payload.GenerateDocument.OutputType;
    //        HttpResponseMessage parameters = default;

    //        switch (outputType)
    //        {
    //            case "XML":
    //                parameters = CreateXmlReport(message);
    //                break;
    //            case "HTML":
    //                parameters = CreateHtmlReport(message);
    //                break;
    //            case "PDF":
    //                break;
    //            default:
    //                break;
    //        }
    //        return parameters;
    //    }


    //    private HttpResponseMessage CreateXmlReport(Payslip message)
    //    {
    //        var response = CreateXmlResponse<Parameters>(message.Payload.GenerateDocument.Parameters);
    //        return response;
    //    }

    //    private HttpResponseMessage CreateHtmlReport(Payslip message)
    //    {
    //        StringBuilder stringcontent = new StringBuilder();

    //        stringcontent.Append("<h1>Payload</h1>");
    //        stringcontent.AppendLine("<div>");
    //        stringcontent.AppendLine("Name: " + message.Envelope.Payload.GenerateDocument.Parameters.Name);
    //        stringcontent.AppendLine("Surname: " + message.Envelope.GenerateDocument.Parameters.Surname);
    //        stringcontent.AppendLine("Street: " + message.Envelope.GenerateDocument.Parameters.Address.Street);
    //        stringcontent.AppendLine("Number: " + message.Envelope.GenerateDocument.Parameters.Address.Number);
    //        stringcontent.AppendLine("City: " + message.Envelope.GenerateDocument.Parameters.Address.City);
    //        stringcontent.AppendLine("PostalCode: " + message.Envelope.GenerateDocument.Parameters.Address.PostalCode);
    //        stringcontent.AppendLine("</div>");

    //        var response = new HttpResponseMessage();
    //        response.Content = new StringContent(stringcontent.ToString());
    //        response.Content.Headers.ContentType = new MediaTypeHeaderValue("text/html");
    //        return response;

    //    }

    //    public static HttpResponseMessage CreateXmlResponse<T>(T objectToSerialize)
    //    {
    //        return new HttpResponseMessage(HttpStatusCode.OK)
    //        {
    //            Content = new ObjectContent<T>(objectToSerialize,
    //                                            new System.Net.Http.Formatting.XmlMediaTypeFormatter
    //                                            {
    //                                                UseXmlSerializer = true
    //                                            })
    //        };
    //    }
    }
}