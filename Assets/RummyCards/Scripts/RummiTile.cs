using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using static Rummikub.RummiTile;

namespace Rummikub
{
    [RequireComponent(typeof(Collider2D))]
    public class RummiTile : MonoBehaviour
    {
        public enum TileColor { Red = 0, Black = 1, Yellow = 2, Blue = 3 };
        public const int MaxNumber = 13;
        public static readonly Color[] TileColors = { new Color(1, 0, 0), new Color(0, 0, 0), new Color(1, 1, 0), new Color(0, 0, 1), };

        public static Color GetActualColor(TileColor color) => TileColors[(int)color];

        public int TileNumber
        {
            get => _isMonkey ? _tileNumber : 0;
            set
            {
                if (_isMonkey) _tileNumber = 0;
                else if (value <= 0) _tileNumber = MaxNumber - (Mathf.Abs(value) % MaxNumber);
                else if (value > MaxNumber) _tileNumber = value % MaxNumber;
                else _tileNumber = value;
            }
        }

        [SerializeField] TextMeshProUGUI _NumberText;
        [SerializeField] GameObject _MonkeyVisual;

        List<Tuple<SpriteRenderer, int>> _spriteRenderers;
        Tuple<Canvas, int> _canvasRenderer;
        Collider2D _collider;
        Vector2 _launchPoint;
        TileColor _tileColor;
        int _tileNumber;
        bool _isMonkey;

        /// <summary>
        /// Initialize this tile
        /// </summary>
        /// <param name="number">Number of the tile. Any number outside of 1 to 13 will be marked as monkey</param>
        /// <param name="color">Color of the tile. Actual color value can be set from this class</param>
        public RummiTile(int number, TileColor color) => Initialize(number, color);

        /// <summary>
        /// Initialize this tile
        /// </summary>
        /// <param name="number">Number of the tile. Any number outside of 1 to 13 will be marked as monkey</param>
        /// <param name="color">Color of the tile. Actual color value can be set from this class</param>
        public void Initialize(int number, TileColor color)
        {
            _tileColor = color;
            Color actualColor = GetActualColor(color);

            _isMonkey = number <= 0 || number > MaxNumber;
            _tileNumber = number;

            _NumberText.gameObject.SetActive(!_isMonkey);
            _NumberText.color = actualColor;
            _NumberText.text = number.ToString();

            _MonkeyVisual.SetActive(_isMonkey);

            _collider = GetComponent<Collider2D>();
            _launchPoint = transform.position;

            var spriteRenderers = GetComponentsInChildren<SpriteRenderer>();
            _spriteRenderers = new List<Tuple<SpriteRenderer, int>>();
            for (int i = 0; i < spriteRenderers.Length; i++)
            {
                var sr = spriteRenderers[i];
                _spriteRenderers.Add(new(sr, sr.sortingOrder));
            }

            var canvas = GetComponentInChildren<Canvas>();
            _canvasRenderer = new(canvas, canvas.sortingOrder);
        }
        public void StartHolding()
        {
            int incrementAmount = 15;

            for (int i = 0; i < _spriteRenderers.Count; i++)
            {
                _spriteRenderers[i].Item1.sortingOrder += incrementAmount;
            }

            _canvasRenderer.Item1.sortingOrder += incrementAmount;
            _launchPoint = transform.position;
        }
        public void StopHolding()
        {
            ContactFilter2D contFilter = new();
            if (_collider.OverlapCollider(contFilter, new Collider2D[1]) != 0)
            {
                transform.position = _launchPoint;
            }

            for (int i = 0; i < _spriteRenderers.Count; i++)
            {
                _spriteRenderers[i].Item1.sortingOrder = _spriteRenderers[i].Item2;
            }

            _canvasRenderer.Item1.sortingOrder = _canvasRenderer.Item2;
        }
    }
}
