using System;
using JetBrains.Annotations;
using UnityEngine;

namespace Assets.Scripts.Serialization
{
    [Serializable]
    public class SerializedValue
    {
        // ReSharper disable FieldCanBeMadeReadOnly.Local
        [SerializeField] private SerializationFormat _format;
        [SerializeField] private string _assemblyQualifiedName;
        [SerializeField] private string _jsonData;
        [SerializeField] private byte[] _binaryData;
        // ReSharper restore FieldCanBeMadeReadOnly.Local

        public SerializedValue([NotNull] string assemblyQualifiedName, [NotNull] string jsonData)
        {
            if (assemblyQualifiedName == null)
            {
                throw new ArgumentNullException("assemblyQualifiedName");
            }
            if (jsonData == null)
            {
                throw new ArgumentNullException("jsonData");
            }

            _assemblyQualifiedName = assemblyQualifiedName;
            _jsonData = jsonData;
            _format = SerializationFormat.UnityJson;
        }

        public SerializedValue([NotNull] string assemblyQualifiedName, [NotNull] byte[] binaryData)
        {
            if (assemblyQualifiedName == null)
            {
                throw new ArgumentNullException("assemblyQualifiedName");
            }
            if (binaryData == null)
            {
                throw new ArgumentNullException("binaryData");
            }

            _assemblyQualifiedName = assemblyQualifiedName;
            _binaryData = binaryData;
            _format = SerializationFormat.BinaryFormatter;
        }

        public SerializedValue([NotNull] string assemblyQualifiedName)
        {
            if (assemblyQualifiedName == null)
            {
                throw new ArgumentNullException("assemblyQualifiedName");
            }

            _assemblyQualifiedName = assemblyQualifiedName;
            _format = SerializationFormat.Null;
        }

        public SerializationFormat Format
        {
            get { return _format; }
        }

        public string AssemblyQualifiedName
        {
            get { return _assemblyQualifiedName; }
        }

        public string JsonData
        {
            get { return _jsonData; }
        }

        public byte[] BinaryData
        {
            get { return _binaryData; }
        }
    }
}
