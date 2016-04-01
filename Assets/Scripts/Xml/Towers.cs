using System;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace Assets.Scripts.Xml
{
    [Serializable]
    public class Towers
    {
        [XmlElement("DirectFireTower", Form = XmlSchemaForm.Unqualified)]
        public DirectFireTower[] DirectFireTower { get; set; }

        [XmlElement("AreaOfEffectTower", Form = XmlSchemaForm.Unqualified)]
        public AreaOfEffectTower[] AreaOfEffectTower { get; set; }
    }
}
