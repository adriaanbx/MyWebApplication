using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RabbitMQ.Client;
using SharedCode;
using SharedCode.Models;

namespace ReportGeneratorConsumer.RabbitMQ
{
    class RabbitMQPublisher
    {
        private static ConnectionFactory _factory;
        private static IConnection _connection;
        private static IModel _model;

        private const string ExchangeName = "Topic_Exchange";
        private const string GeneratedReportQueueName = "GeneratedReportTopic_Queue";

        public RabbitMQPublisher()
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

            _model.QueueDeclare(GeneratedReportQueueName, true, false, false, null);


            _model.QueueBind(GeneratedReportQueueName, ExchangeName, "generatedReport");

        }

        public void Close()
        {
            _connection.Close();
        }

        public void SendReport(Envelope message)
        {
            SendMessage(message.SerializeIntoXml(), "generatedReport");
            Console.WriteLine("Report Type {0}, from sender {1}", message.Payload.GenerateDocument.OutputType,
                message.Sender.Name);
        }

        public void SendMessage(byte[] message, string routingKey)
        {
            _model.BasicPublish(ExchangeName, routingKey, null, message);
        }
    }
}
