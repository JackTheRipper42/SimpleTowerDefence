using UnityEngine;

namespace Assets.Scripts
{
    public class DirectFireTowerModel : TowerModel
    {
        private readonly float _damage;

        public DirectFireTowerModel(
            TowerId id,
            float range,
            float fireRate,
            float damage,
            Sprite baseSprite,
            Sprite towerSprite)
            : base(
                id,
                range,
                fireRate,
                baseSprite,
                towerSprite)
        {
            _damage = damage;
        }

        public float Damage
        {
            get { return _damage; }
        }
    }
}
