using System;
using JetBrains.Annotations;

namespace Assets.Scripts.Serialization
{
    public interface IUnityJsonSerializer
    {
        object Deserialize([NotNull] Type type,[NotNull] string jsonData);
        string Serialize(object data);
    }
}