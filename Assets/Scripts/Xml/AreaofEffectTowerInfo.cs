using System;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace Assets.Scripts.Xml
{
    [Serializable]
    public class AreaOfEffectTowerInfo : TowerInfo
    {
        [XmlElement("Level", Form = XmlSchemaForm.Unqualified)]
        public AreaOfEffectTowerLevelInfo[] Levels { get; set; }
    }
}
