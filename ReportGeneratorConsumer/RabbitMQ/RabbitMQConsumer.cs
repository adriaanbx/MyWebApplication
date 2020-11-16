using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RabbitMQ.Client;
using SharedCode;
using SharedCode.Models.Payslip;
using SharedCode.Models.TempUnemployment;



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

                        if (message != null && message.GetType() == typeof(SharedCode.Models.Payslip.Envelope))
                        {
                            var payslip = (SharedCode.Models.Payslip.Envelope)message;
                            Console.WriteLine("--- Report Generation - Routing Key <{0}> : ReportType <{1}> : Sender <{2}>", routingKey, payslip.Payload.GenerateDocument.OutputType, payslip.Sender.Name);
                        }
                        else if (message != null && message.GetType() == typeof(TempUnemployment))
                        {
                            TempUnemployment tempUnemployment = (TempUnemployment)message;
                            Console.WriteLine("--- Report Generation - Routing Key <{0}> : ReportType <{1}> : Sender <{2}>", routingKey, tempUnemployment.Envelope.Payload.GenerateDocument.OutputType, tempUnemployment.Envelope.Sender.Name);

                        }

                        channel.BasicAck(eventArgs.DeliveryTag, false);

                    }
                }
            }
        }
    }
}
