using System;

namespace Assets.Scripts.Xml
{
    [Serializable]
    public class DirectFireTowerInfo : TowerInfo
    {
        public DirectFireTowerLevelInfo Level1 { get; set; }

        public DirectFireTowerLevelInfo Level2 { get; set; }

        public DirectFireTowerLevelInfo Level3 { get; set; }
    }
}
