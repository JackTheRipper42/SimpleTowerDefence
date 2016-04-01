using System;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace Assets.Scripts.Xml
{
    [Serializable]
    public class Enemies
    {
        [XmlElement("Enemy", Form = XmlSchemaForm.Unqualified)]
        public EnemyInfo[] Enemy { get; set; }
    }
}
