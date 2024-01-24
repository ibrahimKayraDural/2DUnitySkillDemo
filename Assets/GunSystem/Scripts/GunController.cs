using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace GunSystem
{
    public class GunController : MonobehaviourSingleton<GunController>
    {
        [Header("Reference")]
        [SerializeField] TextMeshProUGUI _AmmoTextMesh;
        [SerializeField] GameObject _Bullet;
        [SerializeField] SO_GunData[] _EquippedGuns;
        [SerializeField] float _ScrollWheelCooldown = .2f;

        SO_GunData _currentGun => _EquippedGuns[_gunIndex];
        GunPrefabScript _gunPrefabScript;
        int _gunIndex = 0;
        int[] _leftAmmos;
        bool _isReloading;
        bool isShootingBurst;
        bool hasShotOnceAfterCooldown;
        int burstAmmoLeft;
        float targetTime_Shoot = -1;
        float targetTime_ScrollWheel = -1;

        Transform _barrelTransform;
        float _distanceInY;

        void Start()
        {
            if (_EquippedGuns.Length <= 0) this.enabled = false;

            _leftAmmos = new int[_EquippedGuns.Length];
            for (int i = 0; i < _EquippedGuns.Length; i++) _leftAmmos[i] = _EquippedGuns[i].MagazineCount;

            RefreshGun();
        }
        void Update()
        {
            float scrollWheel = 0;
            if (targetTime_ScrollWheel <= Time.time)
            {
                scrollWheel = Input.GetAxisRaw("Mouse ScrollWheel");

                if (Mathf.Abs(scrollWheel) > 0)
                    targetTime_ScrollWheel = Time.time + _ScrollWheelCooldown;
            }
            

            if (Input.GetButtonDown("GunManager_Fire"))
            {
                SetBurstState(true);
                burstAmmoLeft = _currentGun.BurstAmmoCount;

                FireGun(true);
            }
            else if (Input.GetButton("GunManager_Fire") && _currentGun.FireType != FireType.Single)
            {
                if (_currentGun.FireType == FireType.Burst && burstAmmoLeft <= 0 && targetTime_Shoot <= Time.time && isShootingBurst)
                    burstAmmoLeft = _currentGun.BurstAmmoCount;

                FireGun();
            }
            else if (Input.GetButtonUp("GunManager_Fire"))
            {
                SetBurstState(false);

                if (_currentGun.FireType == FireType.Burst && (targetTime_Shoot < Time.time || hasShotOnceAfterCooldown))
                {
                    targetTime_Shoot = Time.time + _currentGun.Cooldown;
                    hasShotOnceAfterCooldown = false;
                }
            }

            if (Input.GetButtonDown("GunManager_Reload"))
            {
                ReloadGun();
                SetBurstState(false);
            }

            if (Input.GetButtonDown("GunManager_NextGun") || scrollWheel > 0)
            {
                SelectNextGun();
                SetBurstState(false);
            }
            else if (Input.GetButtonDown("GunManager_PreviousGun") || scrollWheel < 0)
            {
                SelectPreviousGun();
                SetBurstState(false);
            }

            LookAtMouse();
        }

        void SetBurstState(bool setToEnabled)
        {
            isShootingBurst = setToEnabled;
        }
        void LookAtMouse()
        {
            Vector2 lookVector = (Vector2)(transform.rotation * Vector3.down).normalized * _distanceInY + GameManager.Instance.MousePosition;
            lookVector = lookVector - (Vector2)transform.position;

            if (lookVector.magnitude <= _distanceInY + 0.01f) return;

            float angle = Vector2.SignedAngle(transform.right, lookVector);
            transform.Rotate(new Vector3(0, 0, angle));

        }
        void FireGun(bool pressedThisFrame = false)
        {
            if (_isReloading) return;

            if (_leftAmmos[_gunIndex] <= 0)
            {
                if (pressedThisFrame) ReloadGun();

                return;
            }

            if (_currentGun.FireType == FireType.Burst && isShootingBurst && burstAmmoLeft <= 0) return;
            if (_currentGun.FireType == FireType.Burst && isShootingBurst == false) return;
            if (targetTime_Shoot > Time.time) return;

            if (_Bullet.TryGetComponent(out Projectile_Base _))
            {
                Instantiate(_Bullet, _gunPrefabScript.BarrelTransform.position, transform.rotation)
                    .GetComponent<Projectile_Base>().Initialize(new string[] { "Bullet" }, _currentGun.Damage);

                _gunPrefabScript.Shoot();
                _leftAmmos[_gunIndex]--;
                RefreshAmmoTextMesh();

                targetTime_Shoot = isShootingBurst && _currentGun.FireType == FireType.Burst ?
                    Time.time + _currentGun.BurstCooldown : Time.time + _currentGun.Cooldown;

                if (_currentGun.FireType == FireType.Burst) hasShotOnceAfterCooldown = true;
                if (isShootingBurst && _currentGun.FireType == FireType.Burst) burstAmmoLeft--;
                if (burstAmmoLeft <= 0) targetTime_Shoot = Time.time + _currentGun.Cooldown;
            }
        }

        IEnumerator ReloadIEnumRef;
        void ReloadGun()
        {
            if (_isReloading) return;
            if (_currentGun.MagazineCount <= _leftAmmos[_gunIndex]) return;

            _isReloading = true;
            _gunPrefabScript.Reload();

            if (ReloadIEnumRef != null) StopCoroutine(ReloadIEnumRef);
            ReloadIEnumRef = ReloadIEnum(_gunIndex);
            StartCoroutine(ReloadIEnumRef);
        }
        IEnumerator ReloadIEnum(int reloadIndex)
        {
            yield return new WaitForSeconds(_EquippedGuns[reloadIndex].ReloadDuration);

            _isReloading = false;
            _leftAmmos[reloadIndex] = _EquippedGuns[reloadIndex].MagazineCount;
            RefreshAmmoTextMesh();
        }

        void SelectNextGun()
        {
            if (_isReloading) return;

            _gunIndex = (_gunIndex + 1) % _EquippedGuns.Length;
            RefreshGun();
        }
        void SelectPreviousGun()
        {
            if (_isReloading) return;

            if (_gunIndex == 0) _gunIndex = _EquippedGuns.Length - 1;
            else _gunIndex--;

            RefreshGun();
        }

        void RefreshAmmoTextMesh()
        {
            if (_AmmoTextMesh != null)
            {
                int leftAmmo = _leftAmmos[_gunIndex];
                _AmmoTextMesh.text = "Ammo Left: " + leftAmmo;
                _AmmoTextMesh.color = leftAmmo <= 0 ? Color.red : Color.white;
            }
        }
        void RefreshGun()
        {
            if (_gunPrefabScript != null) Destroy(_gunPrefabScript.gameObject);

            if (_currentGun.VisualPrefab.TryGetComponent(out GunPrefabScript _))
            {
                _gunPrefabScript = Instantiate(_currentGun.VisualPrefab,
                    transform.position, transform.rotation, transform).GetComponent<GunPrefabScript>();

                _barrelTransform = _gunPrefabScript.BarrelTransform;
                float beta = Vector2.Angle(-transform.right, transform.position - _barrelTransform.position);
                _distanceInY = Vector2.Distance(_barrelTransform.position, transform.position) * Mathf.Sin(beta * Mathf.Deg2Rad);
            }

            RefreshAmmoTextMesh();
        }

        //void OnDrawGizmos()
        //{
        //    if (Application.isEditor) return;

        //    Gizmos.DrawWireSphere(debug1, .03f);
        //    Gizmos.DrawWireSphere((Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition), _distanceInY);
        //    Gizmos.DrawWireSphere(transform.position, _distanceInY + .01f);

        //    Gizmos.DrawLine(transform.position, transform.position + transform.right * 100);
        //    Gizmos.DrawLine(_barrelTransform.position, _barrelTransform.position + transform.right * 100);
        //}
    }
}