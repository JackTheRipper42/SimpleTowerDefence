using UnityEngine;

namespace Assets.Scripts
{
    [RequireComponent(typeof(DirectFireTower))]
    public class DirectFireTowerView : TowerView<DirectFireTower, DirectFireTowerModel, DirectFireTowerLevel>
    {
    }
}
