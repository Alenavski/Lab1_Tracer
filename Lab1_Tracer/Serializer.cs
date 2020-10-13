using System.Xml.Serialization;
using System.IO;
using System.Xml;
using Newtonsoft.Json;
using Formatting = Newtonsoft.Json.Formatting;

namespace libTracer
{
    public class Json_Serializer : ISerializer
    {
        private JsonSerializerSettings _settings = new JsonSerializerSettings
        {
            NullValueHandling = NullValueHandling.Include,
            Formatting = Formatting.Indented,
        };
        
        public string Serialize(object Object)
        {
            return JsonConvert.SerializeObject(Object, _settings);
        }
    }

    public class Xml_Serializer : ISerializer
    {
        private XmlSerializerNamespaces _xmlSerializerNamespaces = new XmlSerializerNamespaces(new []{ XmlQualifiedName.Empty });
        public string Serialize(object Object)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(TraceResult));
            using (StringWriter stringWriter = new StringWriter())
            {
                using (XmlWriter xmlWriter = XmlWriter.Create(stringWriter))
                {
                    xmlSerializer.Serialize(stringWriter, Object, _xmlSerializerNamespaces);
                    return stringWriter.ToString();
                }
            }
        }
    }
}