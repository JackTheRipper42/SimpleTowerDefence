using UnityEngine;

namespace Assets.Scripts
{
    public class TowerClickHandler : RaycastHitHandler<ITower>
    {
        protected override void Handle(RaycastHit hit, ITower tower)
        {
            if (tower.CanUpgrade())
            {
                tower.Upgrade();
            }
        }
    }
}
