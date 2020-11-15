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
                        var ea = consumer.Queue.Dequeue();
                        var message = (Envelope)ea.Body.DeSerializeFromXml(typeof(Envelope));
                        var routingKey = ea.RoutingKey;
                        channel.BasicAck(ea.DeliveryTag, false);
                        Console.WriteLine("--- Report Generation - Routing Key <{0}> : ReportType <{1}> : Sender <{2}>", routingKey, message.Payload.GenerateDocument.OutputType, message.Sender.Name);
                    }
                }
            }
        }
    }
}
