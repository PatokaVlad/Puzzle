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

    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    public void PlayWinClip() => _audioSource.PlayOneShot(winClip);

    public void PlayFailClip() => _audioSource.PlayOneShot(failClip);

    public void PlayPuzzleClip(bool isWin)
    {
        if (isWin) 
            _audioSource.PlayOneShot(winClip);
        else
            _audioSource.PlayOneShot(failClip);
    }
}
