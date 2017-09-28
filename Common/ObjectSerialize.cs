using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json;
namespace Rabbitmq.Functions
{
    public static class ObjectSerialize
    {

        public static byte[] Serialize(this object obj)
        {
            if (obj == null)
            {
                return null;
            }
            var json = JsonConvert.SerializeObject(obj);
            return Encoding.ASCII.GetBytes(json);
        }
        public static object Deserialize(this byte[] arrbytes,Type type)
        {

            var json = Encoding.Default.GetString(arrbytes);
            return JsonConvert.DeserializeObject(json, type);
        }
        public static string DeserializeText(this byte[] arrbytes)
        {

              
            return Encoding.Default.GetString(arrbytes);
        }
    }
}
