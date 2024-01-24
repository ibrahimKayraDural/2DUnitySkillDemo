using UnityEngine;

namespace GunSystem
{
    public enum FireType { Single, Burst, Auto }

    [CreateAssetMenu(menuName = "Data/GunData")]
    public class SO_GunData : ScriptableObject
    {
        public string ID => _ID;
        public GameObject VisualPrefab => _VisualPrefab;
        public FireType FireType => _FireType;
        public float Cooldown => _Cooldown;
        public float Damage => _Damage;
        public float ReloadDuration => _ReloadDuration;
        public int MagazineCount => _MagazineCount;
        public float BurstCooldown => _BurstCooldown;
        public int BurstAmmoCount => _BurstAmmoCount;

        [SerializeField] string _ID;
        [SerializeField] GameObject _VisualPrefab;
        [SerializeField] FireType _FireType;
        [SerializeField] float _Cooldown = .1f;
        [SerializeField] float _Damage = 1;
        [SerializeField] float _ReloadDuration = 1;
        [SerializeField] int _MagazineCount = 1;
        [SerializeField, HideInInspector] float _BurstCooldown = .1f;
        [SerializeField, HideInInspector] int _BurstAmmoCount = 1;
    }
}