using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleHandler : MonoBehaviour
{
    [SerializeField]
    private BalloonsHandler _balloonsHandler;
    [SerializeField]
    private ParticleHandler _particleHandler;

    private int piecesCount = 0;

    public delegate void OnPlayerWin();
    public event OnPlayerWin onPlayerWin;

    [SerializeField]
    private bool useMouse;

    public bool UseMouse { get => useMouse; }

    public void IncreasePiecesCount()
    {
        piecesCount++;
    }

    public void DecreasePiecesCount()
    {
        piecesCount--;

        if(piecesCount == 0)
        {
            _balloonsHandler.SpawnBalloons();
            onPlayerWin();
        }
    }

    public void PlayObjectParticle(Vector2 position)
    {
        _particleHandler.PlayParticle(position);
    }
}
