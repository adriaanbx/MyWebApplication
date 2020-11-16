using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using RabbitMQ.Client;
using SharedCode;
using SharedCode.Models.Payslip;
using SharedCode.Models.TempUnemployment;


namespace ReportGenerator.RabbitMQ
{
    public class RabbitMQClient
    {
        private static ConnectionFactory _factory;
        private static IConnection _connection;
        private static IModel _model;

        private const string ExchangeName = "Topic_Exchange";
        private const string ReportGeneratorQueueName = "ReportGeneratorTopic_Queue";

        public RabbitMQClient()
        {
            CreateConnection();
        }

        private static void CreateConnection()
        {
            _factory = new ConnectionFactory
            {
                HostName = "localhost",
                UserName = "guest",
                Password = "guest"
            };

            _connection = _factory.CreateConnection();
            _model = _connection.CreateModel();

            //Gekozen voor "Topic Exchange" om future proof te zijn: makkelijk scalable naar andere microservices
            _model.ExchangeDeclare(ExchangeName, "topic");

            _model.QueueDeclare(ReportGeneratorQueueName, true, false, false, null);


            _model.QueueBind(ReportGeneratorQueueName, ExchangeName, "reportGenerator");

        }

        public void Close()
        {
            _connection.Close();
        }

        public void SendReport(object message)
        {
            Type type = message.GetType();
            SendMessage(message.SerializeIntoXml(), "reportGenerator", type);

            if (type == typeof(SharedCode.Models.Payslip.Envelope))
            {
                SharedCode.Models.Payslip.Envelope payslip = (SharedCode.Models.Payslip.Envelope)message;
                Console.WriteLine(" Report Sent {0}, from sender {1}", payslip.Payload, payslip.Sender);
            }
            else if (type == typeof(TempUnemployment))
            {
                TempUnemployment tempUnemployment = (TempUnemployment)message;
                Console.WriteLine(" Report Sent {0}, from sender {1}", tempUnemployment.Envelope.Payload, tempUnemployment.Envelope.Sender);
            }




        }

        public void SendMessage(byte[] message, string routingKey, Type type)
        {
            IBasicProperties basicProperties = _model.CreateBasicProperties();
            //basicProperties.SetPersistent(true);
            basicProperties.ContentType = "text/xml";
            basicProperties.Type = type.AssemblyQualifiedName;
            _model.BasicPublish(ExchangeName, routingKey, basicProperties, message);
        }
    }
}