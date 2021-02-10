using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BalloonsHandler : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> balloons = new List<GameObject>();

    [SerializeField]
    private Camera _camera;
    [SerializeField]
    private ParticleHandler _particleHandler;
    private SoundHandler _soundHandler;

    [SerializeField]
    private float balloonsCount = 10;
    private float edgeX,
        edgeY;

    [SerializeField]
    private bool useMouse;

    public bool UseMouse { get => useMouse; }

    private void Start()
    {
        _soundHandler = FindObjectOfType<SoundHandler>();

        edgeX = _camera.orthographicSize * _camera.aspect;
        edgeY = _camera.orthographicSize;
    }

    public void SpawnBalloons()
    {
        for (int i = 0; i < balloonsCount; i++)
        {
            GameObject balloon = balloons[Random.Range(0, balloons.Count)];
            Vector2 spawnPosition = new Vector2(Random.Range(-edgeX, edgeX), -edgeY - 2);
            Instantiate(balloon, spawnPosition, Quaternion.identity);
        }
    }

    public void PlayObjectParticle(Vector2 position)
    {
        _particleHandler.PlayParticle(position);
    }

    public void DecreaseBalloonsCount()
    {
        if (balloonsCount != 0)
        {
            balloonsCount--;
            if(_soundHandler != null)
                _soundHandler.PlayBalloonClip();
        }

    }
}
