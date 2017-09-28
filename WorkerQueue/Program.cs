using Rabbitmq.Functions;
using Rabbitmq.Models;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkerQueue
{
    class Program
    {
        private static ConnectionFactory _factory;
        private static IConnection _connection;
        private static IModel _model;

        private const string Qname = "WorkerQueue";
        static void Main(string[] args)
        {
            var payment1 = new payment { AmountToPay = 10.0m, CardNo = "1561 9632 4789 6542" };
            var payment2 = new payment { AmountToPay = 20.0m, CardNo = "2423 3654 1234 7965" };
            var payment3 = new payment { AmountToPay = 30.0m, CardNo = "3423 3654 1234 7965" };
            var payment4 = new payment { AmountToPay = 40.0m, CardNo = "4423 3654 1234 7965" };

            var payment5 = new payment { AmountToPay = 10.0m, CardNo = "5561 9632 4789 6542" };
            var payment6 = new payment { AmountToPay = 20.0m, CardNo = "6423 3654 1234 7965" };
            var payment7 = new payment { AmountToPay = 30.0m, CardNo = "7423 3654 1234 7965" };
            var payment8 = new payment { AmountToPay = 40.0m, CardNo = "8423 3654 1234 7965" };

            CreateQueue();
            SendMessage(payment1);
            SendMessage(payment2);
            SendMessage(payment3);
            SendMessage(payment4);
            SendMessage(payment5);
            SendMessage(payment6);
            SendMessage(payment7);
            SendMessage(payment8);
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
            _model.BasicPublish("", Qname, null, msg.Serialize());
            Console.WriteLine(" [x] payment message sent :{0}: {1}: {2}", msg.AmountToPay, msg.CardNo, msg.Name);
        }

    }
}
