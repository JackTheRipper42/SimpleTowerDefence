using JetBrains.Annotations;

namespace Assets.Scripts.Serialization
{
    public interface IBinarySerializer
    {
        object Deserialize([NotNull] byte[] binaryData);
        byte[] Serialize(object data);
    }
}