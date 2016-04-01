using System;

namespace Assets.Scripts.Xml
{
    [Serializable]
    public class AreaofEffectTowerInfo : TowerInfo
    {
        public float AreaDamage { get; set; }
        public float DamageRange { get; set; }
    }
}
