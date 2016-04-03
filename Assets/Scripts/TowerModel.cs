using Assets.Scripts.Binding;
using UnityEngine;

namespace Assets.Scripts
{
    public abstract class TowerModel<TLevel> : ITowerModel
        where TLevel : TowerLevel
    {
        public readonly NotifyingObject<int> LevelProperty;

        private readonly TowerId _id;
        private readonly Sprite _baseSprite;
        private readonly TLevel[] _levels;

        protected TowerModel(
            TowerId id,
            Sprite baseSprite,
            TLevel[] levels)
        {
            _id = id;
            _baseSprite = baseSprite;
            _levels = levels;
            LevelProperty = new NotifyingObject<int>();
        }

        public TowerId Id
        {
            get { return _id; }
        }

        public Sprite BaseSprite
        {
            get { return _baseSprite; }
        }

        public TLevel[] Levels
        {
            get { return _levels; }
        }
    }
}
