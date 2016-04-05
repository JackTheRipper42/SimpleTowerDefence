using UnityEngine;

namespace Assets.Scripts
{
    public class DirectFireTowerLevel : TowerLevel
    {
        private readonly float _damage;

        public DirectFireTowerLevel(
            int price,
            float range, 
            float fireRate,
            float damage, 
            Sprite towerSprite) 
            : base(price, range, fireRate, towerSprite)
        {
            _damage = damage;
        }

        public float Damage
        {
            get { return _damage; }
        }
    }
}
