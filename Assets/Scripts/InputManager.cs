using UnityEngine;

namespace Assets.Scripts
{
    public class InputManager : MonoBehaviour
    {
        public LayerMask LayerMask;
        public string PrimaryMouseButtonAxis = "Fire1";
        public string SecondaryMouseButtonAxis = "Fire2";

        private GameManager _gameManager;

        protected virtual void Start()
        {
            _gameManager = GetComponentInParent<GameManager>();
        }

        protected virtual void Update()
        {
            if (Input.GetButtonDown(PrimaryMouseButtonAxis))
            {
                var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, float.PositiveInfinity, LayerMask.value))
                {
                    var placeableGround = hit.transform.gameObject.GetComponent<PlaceableGround>();
                    if (placeableGround != null)
                    {
                        _gameManager.SpawnTower(TowerId.Cannon, hit.point);
                    }
                }
            }
        }
    }
}
