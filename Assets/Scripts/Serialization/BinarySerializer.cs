using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Assets.Scripts.Serialization
{
    public sealed class BinarySerializer : IBinarySerializer
    {
        private readonly BinaryFormatter _binaryFormatter;
        private readonly MemoryStream _stream;

        public BinarySerializer()
        {
            _binaryFormatter = new BinaryFormatter();
            _stream = new MemoryStream();
        }

        public byte[] Serialize(object data)
        {
            _stream.Position = 0;
            _binaryFormatter.Serialize(_stream, data);
            var binaryData = _stream.ToArray();
            return binaryData;
        }

        public object Deserialize(byte[] binaryData)
        {
            if (binaryData == null)
            {
                throw new ArgumentNullException("binaryData");
            }

            _stream.Position = 0;
            _stream.Write(binaryData, 0, binaryData.Length);
            _stream.Position = 0;
            return _binaryFormatter.Deserialize(_stream);
        }
    }
}
