using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundHandler : MonoBehaviour
{
    private AudioSource _audioSource;

    [SerializeField]
    private AudioClip failClip;
    [SerializeField]
    private AudioClip winClip;
    [SerializeField]
    private AudioClip balloonClip;

    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    public void PlayWinClip() => _audioSource.PlayOneShot(winClip);

    public void PlayFailClip() => _audioSource.PlayOneShot(failClip);

    public void PlayBalloonClip() => _audioSource.PlayOneShot(balloonClip);

}
