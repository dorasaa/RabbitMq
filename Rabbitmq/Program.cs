using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rabbitmq.Models;
using RabbitMQ.Client;
using Newtonsoft.Json;
using Rabbitmq.Functions;
namespace Rabbitmq
{
    class Program
    {
        private static ConnectionFactory _factory;
        private static IConnection _connection;
        private static IModel _model;

        private const string Qname = "StandardQueue";

        static void Main(string[] args)
        {
            var payment1 = new payment { AmountToPay = 10.0m, CardNo = "4561 9632 4789 6542" };
            var payment2 = new payment { AmountToPay = 20.0m, CardNo = "4423 3654 1234 7965" };
            var payment3 = new payment { AmountToPay = 30.0m, CardNo = "4423 3654 1234 7965" };
            var payment4 = new payment { AmountToPay = 40.0m, CardNo = "4423 3654 1234 7965" };

            CreateQueue();
            SendMessage(payment1);
            SendMessage(payment2);
            SendMessage(payment3);
            SendMessage(payment4);

            Receive();
        }

     

        private static void CreateQueue()
        {
            _factory = new ConnectionFactory { HostName = "localhost", UserName = "guest", Password = "guest" };
            _connection = _factory.CreateConnection();
            _model = _connection.CreateModel();
            _model.QueueDeclare(Qname, true, false, false, null);
        }

        private static void SendMessage(payment msg)
        {
            _model.BasicPublish("", Qname,null, msg.Serialize());
            Console.WriteLine(" [x] payment message sent :{0}: {1}: {2}", msg.AmountToPay, msg.CardNo, msg.Name);
        }


        private static void Receive()
        {
            var consumer = new QueueingBasicConsumer(_model);
            uint msgcount = GetMessageCount(_model, Qname);
            _model.BasicConsume(Qname, true, consumer);
            uint count = 0;
            while (count < msgcount)
            {
                var msg = (payment)consumer.Queue.Dequeue().Body.Deserialize(typeof(payment));
                Console.WriteLine("--Received {0}:{1}:{2}", msg.CardNo, msg.AmountToPay, msg.Name);
                count++;
            }
            
        }

        private static uint GetMessageCount(IModel channel, string qname)
        {
            var results = channel.QueueDeclare(qname, true, false, false, null);
            return results.MessageCount;
        }
    }

}
