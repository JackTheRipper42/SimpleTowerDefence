using UnityEngine;

namespace Assets.Scripts
{
    public class DirectFireTowerModel : TowerModel<DirectFireTowerLevel>
    {
        public DirectFireTowerModel(
            TowerId id,
            Sprite baseSprite,
            DirectFireTowerLevel[] levels)
            : base(
                id,
                baseSprite,
                levels)
        {
        }
    }
}
