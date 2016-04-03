using UnityEngine;

namespace Assets.Scripts
{
    public abstract class TowerLevel
    {
        private readonly float _range;
        private readonly float _fireRate;
        private readonly Sprite _towerSprite;

        protected TowerLevel(float range, float fireRate, Sprite towerSprite)
        {
            _towerSprite = towerSprite;
            _fireRate = fireRate;
            _range = range;
        }
        
        public float Range
        {
            get { return _range; }
        }

        public float FireRate
        {
            get { return _fireRate; }
        }

        public Sprite TowerSprite
        {
            get { return _towerSprite; }
        }
    }
}
