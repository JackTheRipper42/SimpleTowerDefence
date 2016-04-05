using UnityEngine;

namespace Assets.Scripts
{
    public abstract class TowerLevel
    {
        private readonly int _price;
        private readonly float _range;
        private readonly float _fireRate;
        private readonly Sprite _towerSprite;

        protected TowerLevel(
            int price,
            float range,
            float fireRate,
            Sprite towerSprite)
        {
            _price = price;
            _towerSprite = towerSprite;
            _fireRate = fireRate;
            _range = range;
        }

        public int Price
        {
            get { return _price; }
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
