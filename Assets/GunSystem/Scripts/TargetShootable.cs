using UnityEngine;
using UnityEngine.Events;

namespace GunSystem
{
    [RequireComponent(typeof(Collider2D))]
    public class TargetShootable : MonoBehaviour, IShootable
    {
        [SerializeField] UnityEvent<float> OnShot;

        void IShootable.OnShot(float damage) => OnShot?.Invoke(damage);
    }
}