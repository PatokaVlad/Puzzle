using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BalloonsHandler : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> balloons = new List<GameObject>();

    [SerializeField]
    private ParticleHandler _particleHandler;
    private SoundHandler _soundHandler;
    private PuzzleHandler _puzzleHandler;

    [SerializeField]
    private int balloonsCount = 20;
    [SerializeField]
    private int oneTimeSpawnCount = 5;
    [SerializeField]
    private float spawnDeltaTime = 0.1f;
    [SerializeField]
    private float minSpeed = 4;
    [SerializeField]
    private float maxSpeed = 7;
    private int countOnScene;
    private float edgeX,
        edgeY;

    [SerializeField]
    private bool useMouse;

    public bool UseMouse { get => useMouse; }
    public float MinSpeed { get => minSpeed; }
    public float MaxSpeed { get => maxSpeed; }

    private void Awake()
    {
        _puzzleHandler = FindObjectOfType<PuzzleHandler>();   
    }

    private void OnEnable()
    {
        _puzzleHandler.onPlayerWin += StartSpawn;
    }

    private void Start()
    {
        _soundHandler = FindObjectOfType<SoundHandler>();

        countOnScene = balloonsCount;
        edgeX = Camera.main.orthographicSize * Camera.main.aspect;
        edgeY = Camera.main.orthographicSize;
    }

    private void OnDisable()
    {
        _puzzleHandler.onPlayerWin -= StartSpawn;
    }

    public void SpawnBalloons(int count)
    {
        for (int i = 0; i < count; i++)
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
        if (countOnScene != 0)
        {
            countOnScene--;
            if(_soundHandler != null)
            {
                _soundHandler.PlayBalloonClip();
            }
        }
    }

    private IEnumerator SpawnObjects()
    {
        int count = 0;
        while(count < balloonsCount)
        {
            SpawnBalloons(oneTimeSpawnCount);
            count += oneTimeSpawnCount;
            yield return new WaitForSeconds(spawnDeltaTime);
        }

        if (count - balloonsCount < 0)
            SpawnBalloons(balloonsCount - count);
    }

    private void StartSpawn()
    {
        StartCoroutine(SpawnObjects());
    }
}
