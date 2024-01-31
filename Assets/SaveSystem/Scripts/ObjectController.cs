using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SaveSystem
{
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(SpriteRenderer))]
    public class ObjectController : MonoBehaviour
    {
        public Color ObjectColor => _colors[_colorIndex];
        public int ColorIndex { get { return _colorIndex; } set { _colorIndex = value; RefreshColor(); } }

        [SerializeField, Min(0.1f)] float _MaxSize = 3;
        [SerializeField, Min(0.1f)] float _MinSize = .1f;

        Rigidbody2D _rb;
        SpriteRenderer _sr;
        Color[] _colors = new Color[]
        {   Color.white, Color.red, Color.yellow, Color.green, Color.cyan,
            Color.blue, Color.magenta, Color.gray, Color.black };

        int _colorIndex = 0;

        void Awake()
        {
            if (_MaxSize < _MinSize) _MaxSize = _MinSize;

            _rb = GetComponent<Rigidbody2D>();
            _sr = GetComponent<SpriteRenderer>();

            RefreshColor();
        }

        public void MoveTo(Vector2 target)
        {
            _rb.MovePosition(target);
        }
        public void SetSize(float setTo)
        {
            transform.localScale = Vector2.one * Mathf.Clamp(setTo, _MinSize, _MaxSize);
        }
        public void AddToSize(float addition) => SetSize(transform.localScale.x + addition);
        public void SetNextColor() => ColorIndex = (ColorIndex + 1) % _colors.Length;
        public void SetPreviousColor() => ColorIndex = ColorIndex > 0 ? ColorIndex - 1 : _colors.Length - 1;
        public void SetColorByIndex(int index) => ColorIndex = index;
        void RefreshColor() => _sr.color = _colors[ColorIndex];
    } 
}