using UnityEngine;

namespace Assets.Scripts
{
    public class InputManager : MonoBehaviour
    {
        public string PrimaryMouseButtonAxis = "Fire1";
        public string SecondaryMouseButtonAxis = "Fire2";
        public string SelectCannonTypeAxis = "Select Cannon Type";
        public string SelectGattlingTypeAxis = "Select Gattling Type";
        public string SelectRailgunTypeAxis = "Select Railgun Type";
        public string SelectMissileTypeAxis = "Select Missile Type";
        public string SelectMRLSTypeAxis = "Select MRLS Type";

        private InputHandler _inputHandler;
        private PlaceableGroundClickHandler _placeableGroundClickHandler;

        protected virtual void Start()
        {
            var gameManager = GetComponentInParent<GameManager>();
            _placeableGroundClickHandler = new PlaceableGroundClickHandler(gameManager);
            _inputHandler = new InputHandler(
                _placeableGroundClickHandler,
                new TowerClickHandler());
        }

        protected virtual void Update()
        {
            if (Input.GetButtonDown(SelectCannonTypeAxis))
            {
                SetBuildTowerId(TowerId.Cannon);
            }
            if (Input.GetButtonDown(SelectGattlingTypeAxis))
            {
                SetBuildTowerId(TowerId.Gattling);
            }
            if (Input.GetButtonDown(SelectRailgunTypeAxis))
            {
                SetBuildTowerId(TowerId.Railgun);
            }
            if (Input.GetButtonDown(SelectMissileTypeAxis))
            {
                SetBuildTowerId(TowerId.Missile);
            }
            if (Input.GetButtonDown(SelectMRLSTypeAxis))
            {
                SetBuildTowerId(TowerId.MRLS);
            }

            if (Input.GetButtonDown(PrimaryMouseButtonAxis))
            {
                var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, float.PositiveInfinity))
                {
                    _inputHandler.HandleRaycastHit(hit);
                }
            }
        }

        private void SetBuildTowerId(TowerId id)
        {
            _placeableGroundClickHandler.TowerId = id;
        }
    }
}
