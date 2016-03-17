using System;
using JetBrains.Annotations;

namespace Assets.Scripts.Serialization
{
    public class Formatter : IFormatter
    {
        private readonly IBinarySerializer _binarySerializer;
        private readonly IUnityJsonSerializer _unityJsonSerializer;
        private readonly IFormatSelector _formatSelector;

        public Formatter()
            : this(new BinarySerializer(), new UnityJsonSerializer(), new FormatSelector())
        {
        }

        public Formatter(
            [NotNull] IBinarySerializer binarySerializer,
            [NotNull] IUnityJsonSerializer unityJsonSerializer,
            [NotNull] IFormatSelector formatSelector)
        {
            if (binarySerializer == null)
            {
                throw new ArgumentNullException("binarySerializer");
            }
            if (unityJsonSerializer == null)
            {
                throw new ArgumentNullException("unityJsonSerializer");
            }
            if (formatSelector == null)
            {
                throw new ArgumentNullException("formatSelector");
            }

            _binarySerializer = binarySerializer;
            _unityJsonSerializer = unityJsonSerializer;
            _formatSelector = formatSelector;
        }

        public SerializedValue Serialize<T>(T data)
        {
            return Serialize(typeof(T), data);
        }

        public SerializedValue Serialize(Type type, object data)
        {
            if (type == null)
            {
                throw new ArgumentNullException("type");
            }
            if (type.AssemblyQualifiedName == null)
            {
                throw new ArgumentException("The type has no assembly qualified name.", "type");
            }

            var format = _formatSelector.GetFormat(type, data);
            switch (format)
            {
                case SerializationFormat.Null:
                    return new SerializedValue(type.AssemblyQualifiedName);
                case SerializationFormat.BinaryFormatter:
                    var binaryData = _binarySerializer.Serialize(data);
                    return new SerializedValue(type.AssemblyQualifiedName, binaryData);
                case SerializationFormat.UnityJson:
                    var jsonData = _unityJsonSerializer.Serialize(data);
                    return new SerializedValue(type.AssemblyQualifiedName, jsonData);
                default:
                    throw new NotSupportedException(string.Format(
                        "The serialization format '{0}' is not supported.",
                        format));
            }
        }

        public T Deserialize<T>(SerializedValue serializedValue)
        {
            if (serializedValue == null)
            {
                throw new ArgumentNullException("serializedValue");
            }

            var serializedType = Type.GetType(serializedValue.AssemblyQualifiedName);
            if (serializedType == null)
            {
                throw new InvalidOperationException(string.Format(
                    "The type '{0}' cannot be resolved.",
                    serializedValue.AssemblyQualifiedName));
            }
            if (!typeof(T).IsAssignableFrom(serializedType))
            {
                throw new InvalidOperationException(string.Format(
                    "The type '{0}' cannot be assigned to the type '{1}'",
                    serializedType,
                    typeof(T)));
            }

            return (T) Deserialize(serializedType, serializedValue);
        }

        public object Deserialize(SerializedValue serializedValue)
        {
            if (serializedValue == null)
            {
                throw new ArgumentNullException("serializedValue");
            }

            var serializedType = Type.GetType(serializedValue.AssemblyQualifiedName);
            if (serializedType == null)
            {
                throw new InvalidOperationException(string.Format(
                    "The type '{0}' cannot be resolved.",
                    serializedValue.AssemblyQualifiedName));
            }

            return Deserialize(serializedType, serializedValue);
        }

        private object Deserialize(Type type, SerializedValue serializedValue)
        {
            switch (serializedValue.Format)
            {
                case SerializationFormat.Null:
                    return null;
                case SerializationFormat.BinaryFormatter:
                    return _binarySerializer.Deserialize(serializedValue.BinaryData);
                case SerializationFormat.UnityJson:
                    return _unityJsonSerializer.Deserialize(type, serializedValue.JsonData);
                default:
                    throw new NotSupportedException(string.Format(
                        "The serialization format '{0}' is not supported.",
                        serializedValue.Format));
            }
        }
    }
}
