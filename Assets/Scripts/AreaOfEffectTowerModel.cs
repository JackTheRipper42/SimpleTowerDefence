using UnityEngine;

namespace Assets.Scripts
{
    public class AreaOfEffectTowerModel : TowerModel
    {
        private readonly float _areaDamage;
        private readonly float _damageRange;

        public AreaOfEffectTowerModel(
            TowerId id,
            float range,
            float fireRate,
            float areaDamage,
            float damageRange,
            Sprite baseSprite,
            Sprite towerSprite)
            : base(
                id,
                range,
                fireRate,
                baseSprite,
                towerSprite)
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
