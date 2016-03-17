using System;
using UnityEngine;

namespace Assets.Scripts.Serialization
{
    public class UnityJsonSerializer : IUnityJsonSerializer
    {
        public string Serialize(object data)
        {
            return JsonUtility.ToJson(data);
        }

        public object Deserialize(Type type, string jsonData)
        {
            if (type == null)
            {
                throw new ArgumentNullException("type");
            }
            if (jsonData == null)
            {
                throw new ArgumentNullException("jsonData");
            }

            return JsonUtility.FromJson(jsonData, type);
        }
    }
}
