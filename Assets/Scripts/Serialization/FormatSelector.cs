using System;

namespace Assets.Scripts.Serialization
{
    public class FormatSelector : IFormatSelector
    {
        public SerializationFormat GetFormat(Type type, object data)
        {
            if (type == null)
            {
                throw new ArgumentNullException("type");
            }

            if (ReferenceEquals(data, null))
            {
                return SerializationFormat.Null;
            }

            if (type.Namespace != null && type.Namespace.StartsWith("UnityEngine"))
            {
                return SerializationFormat.UnityJson;
            }

            return SerializationFormat.BinaryFormatter;
        }
    }
}
