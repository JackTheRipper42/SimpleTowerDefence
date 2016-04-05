using System.Collections.Generic;

namespace Assets.Scripts.Xml
{
    public interface ITowerInfo
    {
        string Id { get; set; }
        string BaseSprite { get; set; }
        IEnumerable<TowerLevelInfo> Levels { get; }
    }
}