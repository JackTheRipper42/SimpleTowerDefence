using System;

namespace Assets.Scripts.Xml
{
    [Serializable]
    public class AreaOfEffectTowerInfo : TowerInfo
    {
        public float AreaDamage { get; set; }
        public float DamageRange { get; set; }
    }
}
