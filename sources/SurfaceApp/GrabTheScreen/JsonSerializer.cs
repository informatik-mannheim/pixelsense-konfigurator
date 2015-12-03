using System.IO;
using System.Text;
using System.Runtime.Serialization.Json;

namespace GrabTheScreen
{
    class JsonSerializer<T>
    {
        DataContractJsonSerializer _serializer;

        public JsonSerializer() 
        {
            _serializer = new DataContractJsonSerializer(typeof(T));
        }

        public T Deserialize(string json)
        {
            using(MemoryStream stream = new MemoryStream(Encoding.Unicode.GetBytes(json)))
            {
                return (T) _serializer.ReadObject(stream);
            }
        }

        public string Serialize(T instance)
        {
            using(MemoryStream stream = new MemoryStream())
            {
                _serializer.WriteObject(stream, instance);
                stream.Position = 0;
                return new StreamReader(stream).ReadToEnd();
            }
        }
    }
}
