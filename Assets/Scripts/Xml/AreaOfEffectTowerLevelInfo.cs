using System;

namespace Assets.Scripts.Xml
{
    [Serializable]
    public class AreaOfEffectTowerLevelInfo : TowerLevelInfo
    {
        public float AreaDamage { get; set; }

        public float DamageRange { get; set; }
    }
}
