using System;
using System.Collections.Generic;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace Assets.Scripts.Xml
{
    [Serializable]
    public abstract class TowerInfo<TLevel> : ITowerInfo 
        where TLevel : TowerLevelInfo
    {
        public string Id { get; set; }

        public string BaseSprite { get; set; }

        [XmlElement("Level", Form = XmlSchemaForm.Unqualified)]
        public TLevel[] Levels { get; set; }

        IEnumerable<TowerLevelInfo> ITowerInfo.Levels
        {
            get { return Levels; }
        }
    }
}
