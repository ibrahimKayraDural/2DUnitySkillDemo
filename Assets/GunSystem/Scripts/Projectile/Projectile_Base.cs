using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GunSystem
{
    public interface IShootable
    {
        public void OnShot(float damage);
    }

    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(Collider2D))]
    public abstract class Projectile_Base : MonoBehaviour
    {
        [SerializeField] float _Speed = 1;
        [SerializeField] float _Lifetime = 5;

        Rigidbody2D _rb;
        string[] _tagsToIgnore;
        float _damage;

        public virtual void Initialize(string[] tagsToIgnore, float damage)
        {
            _tagsToIgnore = tagsToIgnore;
            _damage = damage;

            _rb = GetComponent<Rigidbody2D>();
            _rb.gravityScale = 0;
            _rb.velocity = transform.right * _Speed;

            Collider2D col = GetComponent<Collider2D>();
            col.isTrigger = true;

            Invoke(nameof(DestroyProjectile), _Lifetime);
        }

        internal virtual void OnHit(Collider2D collision)
        {
            if (collision.gameObject.TryGetComponent(out IShootable shootable)) shootable.OnShot(_damage);
        }
        internal virtual void DestroyProjectile()
        {
            Destroy(gameObject);
        }

        void OnTriggerEnter2D(Collider2D collision)
        {
            foreach(string t in _tagsToIgnore)
            {
                if (collision.tag == t) return;
            }

            OnHit(collision);
            DestroyProjectile();
        }
    }
}