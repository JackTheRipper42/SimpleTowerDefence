using Assets.Scripts.Xml;
using UnityEngine;

namespace Assets.Scripts
{
    public class DirectFireTower : Tower<DirectFireTowerInfo>
    {
        public float Damage { get; private set; }

        public override void Initialize(DirectFireTowerInfo directFireTowerInfo, Sprite baseSprite, Sprite towerSprite)
        {
            Damage = directFireTowerInfo.Damage;
            base.Initialize(directFireTowerInfo, baseSprite, towerSprite);
        }

        protected override void Fire(Enemy target)
        {
            target.SetHit(Damage);
        }
    }
}
