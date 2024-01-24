using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GunSystem
{
    public class GameManager : MonobehaviourSingleton<GameManager>
    {
        public Vector2 MousePosition => _mousePosition;
        [SerializeField] GameObject _TargetPrefab;

        Vector2 _mousePosition;

        void Update()
        {
            _mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            if (Input.GetMouseButtonDown(1))
            {
                Instantiate(_TargetPrefab, MousePosition, Quaternion.identity);
            }
        }
    }
}