using System;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace Assets.Scripts.Xml
{
    [Serializable]
    public class DirectFireTowerInfo : TowerInfo
    {
        [XmlElement("Level", Form = XmlSchemaForm.Unqualified)]
        public DirectFireTowerLevelInfo[] Levels { get; set; }
    }
}
