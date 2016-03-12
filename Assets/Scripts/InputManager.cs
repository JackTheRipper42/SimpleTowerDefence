﻿using UnityEngine;

namespace Assets.Scripts
{
    public class InputManager : MonoBehaviour
    {
        public LayerMask LayerMask;
        public string PrimaryMouseButtonAxis = "Fire1";
        public string SecondaryMouseButtonAxis = "Fire2";

        private GameManager _gameManager;
        private Rasterizer _rasterizer;

        protected virtual void Start()
        {
            _gameManager = GetComponentInParent<GameManager>();
            _rasterizer = new Rasterizer();
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
                        var rasterizedPosition = _rasterizer.Rasterize(hit.point);
                        _gameManager.SpawnTower(TowerId.Cannon, rasterizedPosition);
                    }
                }
            }
        }
    }
}
