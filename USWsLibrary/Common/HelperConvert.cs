using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace USWsLibrary.Common
{
    public static class HelperConvert<T> where T : class
    {
        public static string E2Xml(T entity)
        {
            string resultado = "";

            try
            {
                //var serializer = new XmlSerializer(typeof(T));
                //var stream = new MemoryStream();
                //serializer.Serialize(stream, entity);
                //stream.Flush();
                //resultado = new string(Encoding.UTF8.GetChars(stream.GetBuffer()));
                var stringwriter = new System.IO.StringWriter();
                var serializer = new XmlSerializer(typeof(T));
                serializer.Serialize(stringwriter, entity);

                return stringwriter.ToString();

            }
            catch (Exception eexx)
            {
                throw;
            }
            return resultado;
        }
    }
}
