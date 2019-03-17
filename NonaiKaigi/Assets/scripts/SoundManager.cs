using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{

    [SerializeField] private AudioClip[] _audioClips = null;
    private AudioSource _audioSource = null;

    void Start()
    {
        _audioSource = gameObject.AddComponent<AudioSource>();
        //ステージを呼び出す
        _audioSource.clip = _audioClips[0];
        _audioSource.volume = 0.6f;
        _audioSource.Play();
    }
    
}
