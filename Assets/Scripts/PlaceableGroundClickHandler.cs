using UnityEngine;

namespace Assets.Scripts
{
    public class PlaceableGroundClickHandler : RaycastHitHandler<PlaceableGround>
    {
        private readonly GameManager _gameManager;
        private readonly Rasterizer _rasterizer;

        public PlaceableGroundClickHandler(GameManager gameManager)
        {
            _gameManager = gameManager;
            _rasterizer = new Rasterizer();
        }

        public TowerId TowerId { get; set; }

        protected override void Handle(RaycastHit hit, PlaceableGround placeableGround)
        {
            var rasterizedPosition = _rasterizer.Rasterize(hit.point);
            if (_gameManager.CanSpawnTower(rasterizedPosition))
            {
                _gameManager.SpawnTower(TowerId, rasterizedPosition);
            }
        }
    }
}
