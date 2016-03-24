using UnityEngine;

namespace Assets.Scripts
{
    public class InputManager : MonoBehaviour
    {
        public LayerMask LayerMask;
        public string PrimaryMouseButtonAxis = "Fire1";
        public string SecondaryMouseButtonAxis = "Fire2";
        public string SelectCannonTypeAxis = "Select Cannon Type";
        public string SelectGattlingTypeAxis = "Select Gattling Type";
        public string SelectRailgunTypeAxis = "Select Railgun Type";
        public string SelectMissileTypeAxis = "Select Missile Type";
        public string SelectMRLSTypeAxis = "Select MRLS Type";

        private TowerId _buildTowerId;
        private GameManager _gameManager;
        private Rasterizer _rasterizer;

        protected virtual void Start()
        {
            _gameManager = GetComponentInParent<GameManager>();
            _rasterizer = new Rasterizer();
        }

        protected virtual void Update()
        {
            if (Input.GetButtonDown(SelectCannonTypeAxis))
            {
                _buildTowerId = TowerId.Cannon;
            }
            if (Input.GetButtonDown(SelectGattlingTypeAxis))
            {
                _buildTowerId = TowerId.Gattling;
            }
            if (Input.GetButtonDown(SelectRailgunTypeAxis))
            {
                _buildTowerId = TowerId.Railgun;
            }
            if (Input.GetButtonDown(SelectMissileTypeAxis))
            {
                _buildTowerId = TowerId.Missile;
            }
            if (Input.GetButtonDown(SelectMRLSTypeAxis))
            {
                _buildTowerId = TowerId.MRLS;
            }

            if (Input.GetButtonDown(PrimaryMouseButtonAxis))
            {
                var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, float.PositiveInfinity, LayerMask.value))
                {
                    var placeableGround = hit.transform.gameObject.GetComponent<PlaceableGround>();
                    var rasterizedPosition = _rasterizer.Rasterize(hit.point);
                    if (placeableGround != null && _gameManager.CanSpawnTower(rasterizedPosition))
                    {
                        _gameManager.SpawnTower(_buildTowerId, rasterizedPosition);
                    }
                }
            }
        }
    }
}
