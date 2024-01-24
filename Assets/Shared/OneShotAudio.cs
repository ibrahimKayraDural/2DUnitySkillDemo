using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class OneShotAudio : MonoBehaviour
{
    [SerializeField] AudioClip[] _Clips;
    [SerializeField][Min(0)] float _PitchVariation = .1f;

    AudioClip currentClip;
    AudioSource source;

    void Start()
    {
        if (_Clips.Length <= 0) Destroy(gameObject); 
        else
        {
            currentClip = _Clips[Random.Range(0, _Clips.Length)];
            source = GetComponent<AudioSource>();

            gameObject.name = currentClip.name + "_OneShot";

            source.loop = false;
            source.clip = currentClip;
            source.pitch = Mathf.Clamp(Random.Range(source.pitch - _PitchVariation, source.pitch + _PitchVariation), -3, 3);
            source.Play();
        }
    }
    void Update()
    {
        if (source.isPlaying == false) Destroy(gameObject);
    }
}
