using UnityEngine;

namespace Assets.Scripts
{
    [RequireComponent(typeof(AreaOfEffectTower))]
    public class AreaOfEffectTowerView : TowerView<AreaOfEffectTower, AreaOfEffectTowerModel, AreaOfEffectTowerLevel>
    {
    }
}
