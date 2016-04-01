using System;

namespace Assets.Scripts.Xml
{
    [Serializable]
    public class TowerInfo
    {
        public TowerId Id { get; set; }

        public float Range { get; set; }

        public float FireRate { get; set; }
    }
}
