using JetBrains.Annotations;
using System;
using UnityEngine;

namespace Assets.Scripts.Serialization
{
    [Serializable]
    public class SerializedMember
    {
        // ReSharper disable FieldCanBeMadeReadOnly.Local
        [SerializeField] private string _name;
        [SerializeField] private MemberType _memberType;
        [SerializeField] private SerializedValue _serializedValue;
        // ReSharper restore FieldCanBeMadeReadOnly.Local

        public SerializedMember([NotNull] string name, MemberType memberType, [NotNull] SerializedValue serializedValue)
        {
            if (name == null)
            {
                throw new ArgumentNullException("name");
            }
            if (serializedValue == null)
            {
                throw new ArgumentNullException("serializedValue");
            }

            _name = name;
            _memberType = memberType;
            _serializedValue = serializedValue;
        }

        public string Name
        {
            get { return _name; }
        }

        public MemberType MemberType
        {
            get { return _memberType; }
        }

        public SerializedValue SerializedValue
        {
            get { return _serializedValue; }
        }
    }
}
