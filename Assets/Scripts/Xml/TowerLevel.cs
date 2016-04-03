using System;

namespace Assets.Scripts.Xml
{
    [Serializable]
    public class TowerLevelInfo
    {
        public float Range { get; set; }
        public float FireRate { get; set; }
        public string TowerSprite { get; set; }
    }
}
