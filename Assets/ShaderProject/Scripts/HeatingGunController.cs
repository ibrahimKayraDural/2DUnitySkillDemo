using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeatingGunController : MonoBehaviour
{
    [SerializeField] GameObject _OverheatMessage;
    [SerializeField] GameObject _ShootSfx;
    [SerializeField] SpriteRenderer _Sr;
    [SerializeField, Min(0)] float _ShootCooldown = .2f;
    [SerializeField, Min(0.001f)] float _HeatDisapearTime = 1f;
    [SerializeField, Min(1)] int _MaxShootAmountUntillHeat = 10;

    bool _canShoot => _targetTime_shoot <= Time.time && _overHeated == false;
    float _targetTime_shoot = -1;
    float _normalizedCurrentHeat = 0;
    bool _overHeated;

    void Update()
    {
        if (Input.GetMouseButton(0) && _canShoot)
        {
            Shoot();

            _targetTime_shoot = Time.time + _ShootCooldown;
        }

        SubtractFromCurrentHeat(Time.deltaTime / _HeatDisapearTime);
        LookAtMouse();
    }
    void LookAtMouse()
    {
        Vector2 lookVector = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;

        float angle = Vector2.SignedAngle(transform.right, lookVector);
        transform.Rotate(new Vector3(0, 0, angle));

    }
    void SetOverheat(bool setTo)
    {
        _overHeated = setTo;
        _OverheatMessage.SetActive(setTo);
    }

    void AddToCurrentHeat(float amount) => SetCurrentHeat(_normalizedCurrentHeat + amount);
    void SubtractFromCurrentHeat(float amount) => SetCurrentHeat(_normalizedCurrentHeat - amount);
    void SetCurrentHeat(float setTo)
    {
        _normalizedCurrentHeat = Mathf.Clamp(setTo, 0, 1);
        _Sr.material.SetFloat("_NormalizedHeat", _normalizedCurrentHeat);

        if (_normalizedCurrentHeat == 1) SetOverheat(true);
        if (_normalizedCurrentHeat == 0) SetOverheat(false);
    }

    void Shoot()
    {
        Instantiate(_ShootSfx);
        AddToCurrentHeat(1f / _MaxShootAmountUntillHeat);
    }
}