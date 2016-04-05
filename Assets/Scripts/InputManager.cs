using System.Linq;
using UnityEngine;

namespace Assets.Scripts
{
    public class InputManager : MonoBehaviour
    {
        public string PrimaryMouseButtonAxis = "Fire1";
        public string SecondaryMouseButtonAxis = "Fire2";
        public string TowerType1Axis = "Select Tower Type 1";
        public string TowerType2Axis = "Select Tower Type 2";
        public string TowerType3Axis = "Select Tower Type 3";
        public string TowerType4Axis = "Select Tower Type 4";
        public string TowerType5Axis = "Select Tower Type 5";

        private InputHandler _inputHandler;
        private PlaceableGroundClickHandler _placeableGroundClickHandler;
        private string[] _towerIds;

        protected virtual void Start()
        {
            var gameManager = GetComponentInParent<GameManager>();
            _towerIds = gameManager.GetTowerIds().ToArray();
            _placeableGroundClickHandler = new PlaceableGroundClickHandler(gameManager);
            _inputHandler = new InputHandler(
                _placeableGroundClickHandler,
                new TowerClickHandler(gameManager));
            _placeableGroundClickHandler.TowerId = _towerIds[0];
        }

        protected virtual void Update()
        {
            if (Input.GetButtonDown(TowerType1Axis))
            {
                _placeableGroundClickHandler.TowerId = _towerIds[0];
            }
            if (Input.GetButtonDown(TowerType2Axis))
            {
                _placeableGroundClickHandler.TowerId = _towerIds[1];
            }
            if (Input.GetButtonDown(TowerType3Axis))
            {
                _placeableGroundClickHandler.TowerId = _towerIds[2];
            }
            if (Input.GetButtonDown(TowerType4Axis))
            {
                _placeableGroundClickHandler.TowerId = _towerIds[3];
            }
            if (Input.GetButtonDown(TowerType5Axis))
            {
                _placeableGroundClickHandler.TowerId = _towerIds[4];
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
    }
}
