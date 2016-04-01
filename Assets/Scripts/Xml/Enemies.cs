using System;

namespace Assets.Scripts.Xml
{
    [Serializable]
    public class Enemies
    {
        [System.Xml.Serialization.XmlElement("Enemy", Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public EnemyInfo[] Enemy { get; set; }
    }
}
