using UnityEngine;

namespace Assets.Scripts
{
    public class AreaOfEffectTowerLevel : TowerLevel
    {
        private readonly float _areaDamage;
        private readonly float _damageRange;

        public AreaOfEffectTowerLevel(
            int price,
            float range,
            float fireRate,
            float areaDamage,
            float damageRange,
            Sprite towerSprite)
            : base(price, range, fireRate, towerSprite)
        {
            _areaDamage = areaDamage;
            _damageRange = damageRange;
        }

        public float AreaDamage
        {
            get { return _areaDamage; }
        }

        public float DamageRange
        {
            get { return _damageRange; }
        }
    }
}
