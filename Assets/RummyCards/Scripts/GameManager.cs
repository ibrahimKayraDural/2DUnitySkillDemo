using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rummikub
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] GameObject _TilePrefab;
        [SerializeField] Transform _TileParent;
        [SerializeField] PlayerInputMouse _PlayerInput;
        [SerializeField] Camera _GameplayCamera;

        List<RummiTile> _tilesAtPlay = new List<RummiTile>();
        RummiTile _heldTile = null;
        Vector2 _tileHoldOffset = Vector2.zero;

        void Start()
        {
            PopulateWithDefaultTiles();

            if (_GameplayCamera == null) _GameplayCamera = Camera.main;

            int i = 0;
            for (int y = 0; y < 8; y++)
            {
                for (int x = 0; x < 16; x++)
                {
                    _tilesAtPlay[i].transform.localPosition = new Vector2(x, y * 1.5f);

                    i++;
                    if (i >= _tilesAtPlay.Count - 1) break;
                }
            }
        }
        void Update()
        {
            if (_PlayerInput.IsLeftClickDown())
            {
                var mousePos = _PlayerInput.GetMousePosition();
                var point = _GameplayCamera.ScreenToWorldPoint(mousePos);
                var cols = Physics2D.RaycastAll(point, Vector3.forward);
                foreach (var c in cols)
                {
                    var target = c.transform;
                    if (target.TryGetComponent(out RummiTile tile))
                    {
                        _heldTile = tile;
                        _heldTile.StartHolding();
                        _tileHoldOffset = target.position - point;
                        break;
                    }
                }
            }
            else if (_PlayerInput.IsLeftClickUp() && _heldTile != null)
            {
                _heldTile.StopHolding();
                _heldTile = null;
                _tileHoldOffset = Vector2.zero;
            }

            if (_heldTile != null)
            {
                Vector2 pos = _GameplayCamera.ScreenToWorldPoint(_PlayerInput.GetMousePosition());
                _heldTile.transform.position = pos + _tileHoldOffset;
            }
        }

        void PopulateWithDefaultTiles()
        {
            //Add two series of all
            for (int repetition = 0; repetition < 2; repetition++)
            {
                //Add a monkey
                _tilesAtPlay.Add(InstantiateTile(0, 0));

                //Add one of each number and color
                for (int tileNumber = 1; tileNumber < RummiTile.MaxNumber + 1; tileNumber++)
                {
                    for (int colorIDX = 0; colorIDX < 4; colorIDX++)
                    {
                        int num2 = tileNumber;
                        int color2 = colorIDX;
                        _tilesAtPlay.Add(InstantiateTile(num2, color2));
                    }
                }
            }

            RummiTile InstantiateTile(int number, int colorInt)
            {
                var go = Instantiate(_TilePrefab, _TileParent);
                go.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
                var tile = go.GetComponent<RummiTile>();
                tile.Initialize(number, (RummiTile.TileColor)colorInt);
                return tile;
            }
        }
    }
}
