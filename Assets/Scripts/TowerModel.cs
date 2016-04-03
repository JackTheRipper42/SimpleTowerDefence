using UnityEngine;

namespace Assets.Scripts
{
    public class TowerModel
    {
        private readonly TowerId _id;
        private readonly float _range;
        private readonly float _fireRate;
        private readonly Sprite _baseSprite;
        private readonly Sprite _towerSprite;

        public TowerModel(
            TowerId id,
            float range,
            float fireRate,
            Sprite baseSprite,
            Sprite towerSprite)
        {
            _id = id;
            _range = range;
            _fireRate = fireRate;
            _baseSprite = baseSprite;
            _towerSprite = towerSprite;
        }

        public TowerId Id
        {
            get { return _id; }
        }

        public float Range
        {
            get { return _range; }
        }

        public float FireRate
        {
            get { return _fireRate; }
        }

        public Sprite BaseSprite
        {
            get { return _baseSprite; }
        }

        public Sprite TowerSprite
        {
            get { return _towerSprite; }
        }
    }
}
