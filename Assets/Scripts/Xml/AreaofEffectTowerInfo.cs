using System;

namespace Assets.Scripts.Xml
{
    [Serializable]
    public class AreaOfEffectTowerInfo : TowerInfo
    {
        public AreaOfEffectTowerLevelInfo Level1 { get; set; }

        public AreaOfEffectTowerLevelInfo Level2 { get; set; }

        public AreaOfEffectTowerLevelInfo Level3 { get; set; }
    }
}
