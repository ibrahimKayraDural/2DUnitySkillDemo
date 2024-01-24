using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GunSystem
{
    public class Target : MonoBehaviour
    {
        [SerializeField] Transform _CollisionParent;
        [SerializeField] SlidingBarScript _SliderScript;
        [SerializeField, Min(0.1f)] float _MaxHealth;

        Transform targetTransform;
        float health;
        float normalizedHealth => health / _MaxHealth;

        void Start()
        {
            if (GunController.Instance == null) Destroy(gameObject);

            targetTransform = GunController.Instance.transform;

            health = _MaxHealth;

            if(_SliderScript != null)
            {
                _SliderScript.SetSliderEnabled(false);
                _SliderScript.SetMaxValue(_MaxHealth);
                _SliderScript.SetMinValue(0);
                _SliderScript.SetValue(health);
            }
        }

        void Update()
        {
            float angle = Vector2.SignedAngle(_CollisionParent.right, targetTransform.position - _CollisionParent.position);
            _CollisionParent.Rotate(new Vector3(0, 0, angle));
        }

        public void OnShot(float damage)
        {
            health = Mathf.Max(0, health - damage);

            if (health <= 0) Die();

            if(_SliderScript != null)
            {
                _SliderScript.SetNormalizedValue(normalizedHealth);
                _SliderScript.SetSliderEnabled(true);
            }
        }

        void Die()
        {
            Destroy(gameObject);
        }
    }
}