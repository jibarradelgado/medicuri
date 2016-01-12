using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.IO;

namespace MedNeg.Serializacion
{
    public static class BlXmlSerializacion
    {
        /// <summary>
        /// Serializa cualquier objeto que tiene un constructor sin parámetros
        /// </summary>
        /// <param name="obj"></param>
        /// <returns>Una cadena en formato XML que tiene todas las propiedades y valores del objeto</returns>            
        public static String ToXml(this object obj)
        {
            XmlSerializer s = new XmlSerializer(obj.GetType());
            using (StringWriter writer = new StringWriter())
            {
                s.Serialize(writer, obj);
                return writer.ToString();
            }
        }

        /// <summary>
        /// Deserializa un objeto previamente serializado
        /// </summary>
        /// <typeparam name="Bitacora">tipo del objeto a deserealizar</typeparam>
        /// <param name="data">cadena en formato XML que contiene las propiedades y valores del objeto</param>
        /// <returns>Referencia al objeto</returns>
        public static T ConvertTo<T>(this string data)
        {
            XmlSerializer s = new XmlSerializer(typeof(T));
            using (StringReader reader = new StringReader(data))
            {
                object obj = s.Deserialize(reader);
                return (T)obj;
            }
        }


            

    }
}
