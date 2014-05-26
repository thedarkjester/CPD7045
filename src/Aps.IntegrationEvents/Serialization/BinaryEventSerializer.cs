using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Aps.IntegrationEvents.Serialization
{
    public class BinaryEventSerializer
    {
        public byte[] SerializeMessage(object message)
        {
            byte[] data;

            var binarySerializer = new BinaryFormatter();
            using (var ms = new MemoryStream())
            {
                binarySerializer.Serialize(ms, message);
                data = ms.ToArray();
            }

            return data;
        }
    }

    public class BinaryEventDeSerializer
    {
        public object DeSerializeMessage(byte[] data)
        {
            object returnObject;

            var binarySerializer = new BinaryFormatter();

            using (var ms = new MemoryStream(data))
            {
                returnObject = binarySerializer.Deserialize(ms);
            }

            return returnObject;
        }
    }
}