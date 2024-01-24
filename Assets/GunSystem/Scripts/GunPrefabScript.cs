using UnityEngine;

namespace GunSystem
{
    public class GunPrefabScript : MonoBehaviour
    {
        public Transform BarrelTransform => _BarrelTransform;

        [SerializeField] Transform _BarrelTransform;
        [SerializeField] Animator[] _Animators;

        [SerializeField] GameObject _Shoot_SFX;
        [SerializeField] GameObject _Reload_SFX;

        const string HASH_SHOOT = "shoot";
        const string HASH_RELOAD = "reload";

        public void Shoot()
        {
            foreach(Animator a in _Animators)
            {
                a.Play(HASH_SHOOT, -1, 0);
            }

            if (_Shoot_SFX != null) Instantiate(_Shoot_SFX);
        }
        public void Reload()
        {
            foreach (Animator a in _Animators)
            {
                a.Play(HASH_RELOAD, -1, 0);
            }

            if (_Reload_SFX != null) Instantiate(_Reload_SFX);
        }
    }
}
