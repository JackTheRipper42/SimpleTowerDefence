using UnityEngine;

namespace Assets.Scripts
{
    public class AreaOfEffectTowerModel : TowerModel<AreaOfEffectTowerLevel>
    {
        public AreaOfEffectTowerModel(
            TowerId id,
            Sprite baseSprite,
            AreaOfEffectTowerLevel[] levels
            )
            : base(id, baseSprite, levels)
        {
        }
    }
}
