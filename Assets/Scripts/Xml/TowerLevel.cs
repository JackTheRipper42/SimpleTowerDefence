using System;

namespace Assets.Scripts.Xml
{
    [Serializable]
    public abstract class TowerLevelInfo
    {
        public int Price { get; set; }

        public float Range { get; set; }

        public float FireRate { get; set; }

        public string TowerSprite { get; set; }
    }
}
