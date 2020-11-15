using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Newtonsoft.Json;
using SharedCode.Models;

namespace SharedCode
{
    public static class ObjectSerialize
    {
        #region Serialize
        //default = json
        public static byte[] Serialize(this object obj)
        {
            if (obj == null)
            {
                return null;
            }

            var json = JsonConvert.SerializeObject(obj);
            return Encoding.ASCII.GetBytes(json);
        }

        public static byte[] SerializeIntoXml(this object obj)
        {
            MemoryStream memoryStream = new MemoryStream();
            XmlSerializer xmlSerialiser = new XmlSerializer(obj.GetType());
            xmlSerialiser.Serialize(memoryStream, obj);
            memoryStream.Flush();
            memoryStream.Seek(0, SeekOrigin.Begin);
            return memoryStream.GetBuffer();
        }
        #endregion


        #region Deserialize
        //default = json
        public static object DeSerialize(this byte[] arrBytes, Type type)
        {
            var json = Encoding.Default.GetString(arrBytes);
            return JsonConvert.DeserializeObject(json, type);
        }


        public static string DeSerializeText(this byte[] arrBytes)
        {
            return Encoding.Default.GetString(arrBytes);
        }

        private static object DeserializeFromXml(this byte[] arrBytes, Envelope message)
        {
            MemoryStream memoryStream = new MemoryStream();
            memoryStream.Write(arrBytes, 0, arrBytes.Length);
            memoryStream.Seek(0, SeekOrigin.Begin);
            XmlSerializer xmlSerialiser = new XmlSerializer(typeof(Envelope));
            return xmlSerialiser.Deserialize(memoryStream) as Envelope;
        }

        #endregion
    }
}
