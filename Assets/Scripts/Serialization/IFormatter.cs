using System;
using JetBrains.Annotations;

namespace Assets.Scripts.Serialization
{
    public interface IFormatter
    {
        SerializedValue Serialize<T>([CanBeNull] T data);
        SerializedValue Serialize([NotNull] Type type, [CanBeNull] object data);
        T Deserialize<T>([NotNull] SerializedValue serializedValue);
        object Deserialize([NotNull] SerializedValue serializedValue);
    }
}