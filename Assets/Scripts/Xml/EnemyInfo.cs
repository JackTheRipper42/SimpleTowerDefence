using System;

namespace Assets.Scripts.Xml
{
    [Serializable]
    public class EnemyInfo
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public float Speed { get; set; }

        public float Health { get; set; }

        public float Size { get; set; }

        public string Animator { get; set; }
    }
}
