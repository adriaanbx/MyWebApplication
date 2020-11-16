using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RabbitMQ.Client;
using SharedCode;
using SharedCode.Models.Payslip;
using SharedCode.Models.TempUnemployment;
using SharedCode.Validation;



namespace ReportGeneratorConsumer.RabbitMQ
{
    class RabbitMQConsumer
    {
        private static ConnectionFactory _factory;
        private static IConnection _connection;

        private const string ExchangeName = "Topic_Exchange";
        private const string ReportGeneratorQueueName = "ReportGeneratorTopic_Queue";

        public void CreateConnection()
        {
            _factory = new ConnectionFactory
            {
                HostName = "localhost",
                UserName = "guest",
                Password = "guest"
            };
        }

        public void Close()
        {
            _connection.Close();
        }

        public void ProcessMessages()
        {
            using (_connection = _factory.CreateConnection())
            {
                using (var channel = _connection.CreateModel())
                {
                    Console.WriteLine("Listening for Topic <reportGenerator>");
                    Console.WriteLine("-----------------------------------------");
                    Console.WriteLine();

                    channel.ExchangeDeclare(ExchangeName, "topic");
                    channel.QueueDeclare(ReportGeneratorQueueName,
                        true, false, false, null);

                    channel.QueueBind(ReportGeneratorQueueName, ExchangeName,
                        "reportGenerator");

                    channel.BasicQos(0, 1, false);

                    var consumer = new QueueingBasicConsumer(channel);
                    channel.BasicConsume(ReportGeneratorQueueName, false, consumer);

                    while (true)
                    {

                        var eventArgs = consumer.Queue.Dequeue();
                        string objectType = eventArgs.BasicProperties.Type;
                        Type t = Type.GetType(objectType);
                        var message = eventArgs.Body.DeSerializeFromXmlAndValidate(t);
                        var routingKey = eventArgs.RoutingKey;

                        if (message != null)
                        {
                            CreateReport(message, MessageValidation.OutputType);
                        }

                        channel.BasicAck(eventArgs.DeliveryTag, false);
                    }
                }
            }
        }

        private void CreateReport(object message, string outputType)
        {
            if (message.GetType() == typeof(SharedCode.Models.Payslip.Envelope))
            {
                var payslip = (SharedCode.Models.Payslip.Envelope)message;
                SendGeneratedReport(payslip);
                Console.WriteLine("--- Report Generation - ReportType <{0}> : Sender <{1}>", payslip.Payload.GenerateDocument.OutputType, payslip.Sender.Name);
            }
            else if (message.GetType() == typeof(TempUnemployment))
            {
                var tempUnemployment = (SharedCode.Models.TempUnemployment.Envelope)message;
                SendGeneratedReport(tempUnemployment);
                Console.WriteLine("--- Report Generation - ReportType <{0}> : Sender <{1}>", tempUnemployment.Payload.GenerateDocument.OutputType, tempUnemployment.Sender.Name);
            }


            //TODO implement code
            //Zie ReportGeneratorService
            throw new NotImplementedException();
        }


        private void SendGeneratedReport(SharedCode.Models.Payslip.Envelope message)
        {
            RabbitMQPublisher client = new RabbitMQPublisher();
            client.SendReport(message);
        }

        private void SendGeneratedReport(SharedCode.Models.TempUnemployment.Envelope message)
        {
            RabbitMQPublisher client = new RabbitMQPublisher();
            //client.SendReport(message);
        }
    }
}
