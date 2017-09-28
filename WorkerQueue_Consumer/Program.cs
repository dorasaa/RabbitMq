using Rabbitmq.Models;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Rabbitmq.Functions;
namespace WorkerQueue_Consumer
{
    class Program
    {

        private static ConnectionFactory _factory;
        private static IConnection _connection;
        private static IModel _model;

        private const string Qname = "WorkerQueue";
        static void Main(string[] args)
        {
            Receive();
            Console.ReadLine();
        }

        private static void Receive()
        {
            _factory = new ConnectionFactory { HostName = "localhost", UserName = "guest", Password = "guest" };
            using (_connection = _factory.CreateConnection())
            {
                using (var channel   = _connection.CreateModel())
                {
                    channel.QueueDeclare(Qname, true, false, false, null);
                    channel.BasicQos(0, 1, false);

                    var consumer = new QueueingBasicConsumer(channel);
                    channel.BasicConsume(Qname, false, consumer);

                    while (true)
                    {
                        var ea = consumer.Queue.Dequeue();
                        var msg = (payment)ea.Body.Deserialize(typeof(payment));
                        channel.BasicAck(ea.DeliveryTag, false);
                        Console.WriteLine("..Payment processed {0}:{1}", msg.CardNo, msg.AmountToPay);
                    }
                }

            }
            
        }
    }
}
