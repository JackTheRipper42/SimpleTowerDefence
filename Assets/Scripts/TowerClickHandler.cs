using UnityEngine;

namespace Assets.Scripts
{
    public class TowerClickHandler : RaycastHitHandler<ITower>
    {
        private readonly GameManager _gameManager;

        public TowerClickHandler(GameManager gameManager)
        {
            _gameManager = gameManager;
        }

        protected override void Handle(RaycastHit hit, ITower tower)
        {
            if (_gameManager.CanUpgrade(tower))
            {
                _gameManager.UpgradeTower(tower);
            }
        }
    }
}
