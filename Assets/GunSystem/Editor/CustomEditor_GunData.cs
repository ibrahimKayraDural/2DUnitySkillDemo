using UnityEngine;
using UnityEditor;

namespace GunSystem
{
    [CustomEditor(typeof(SO_GunData))]
    public class CustomEditor_GunData : Editor
    {
        SerializedProperty _type;
        SerializedProperty _burstCooldown;
        SerializedProperty _burstAmmoCount;

        void OnEnable()
        {
            _type = serializedObject.FindProperty("_FireType");
            _burstCooldown = serializedObject.FindProperty("_BurstCooldown");
            _burstAmmoCount = serializedObject.FindProperty("_BurstAmmoCount");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            base.OnInspectorGUI();

            SO_GunData data = (SO_GunData)target;
            float dps = data.MagazineCount * data.Damage;

            FireType type = (FireType)_type.enumValueIndex;
            if (type == FireType.Burst)
            {
                EditorGUILayout.PropertyField(_burstCooldown);
                EditorGUILayout.PropertyField(_burstAmmoCount);

                int session = Mathf.CeilToInt(data.MagazineCount / data.BurstAmmoCount);
                dps /= (session - 1) * data.Cooldown + (data.MagazineCount - 1 - (session - 1)) * data.BurstCooldown + data.ReloadDuration;
            }
            else
            {
                dps /= Mathf.Max(data.Cooldown * (data.MagazineCount - 1) + data.ReloadDuration, 0.001f);
            }

            EditorGUILayout.LabelField("CURRENT DPS (reload included): " + dps);

            serializedObject.ApplyModifiedProperties();
        }
    } 
}