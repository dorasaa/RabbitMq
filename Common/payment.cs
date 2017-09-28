using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rabbitmq.Models
{
    [Serializable]
    public class payment
    {
        public decimal AmountToPay;
        public string CardNo;
        public string Name;
    }
    [Serializable]
    public class PurchaseOrder
    {
        public decimal AmountToPay;
        public int PoNumber;
        public string CompanyName;
        public int PaymentDayTerms;
    }
}
