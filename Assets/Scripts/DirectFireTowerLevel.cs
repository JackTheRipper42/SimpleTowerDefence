using UnityEngine;

namespace Assets.Scripts
{
    public class DirectFireTowerLevel : TowerLevel
    {
        private readonly float _damage;

        public DirectFireTowerLevel(
            float range, 
            float fireRate,
            float damage, 
            Sprite towerSprite) 
            : base(range, fireRate, towerSprite)
        {
            _damage = damage;
        }

        public float Damage
        {
            get { return _damage; }
        }
    }
}
