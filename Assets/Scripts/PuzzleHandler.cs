using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleHandler : MonoBehaviour
{
    [SerializeField]
    private BalloonsHandler _balloonsHandler;
    [SerializeField]
    private ParticleHandler _particleHandler;
    [SerializeField]
    private GameObject completePuzzle;
    [SerializeField]
    private GameObject puzzleBackground;

    private int piecesCount = 0;

    [SerializeField]
    private float correctPositionAccuracy = 0.5f;
    [SerializeField]
    private float animationAccuracy = 0.01f;
    [SerializeField]
    private float smoothDragMultiplier = 10f;
    [SerializeField]
    private float minSmoothAnimationMultiplier = 2.5f;
    [SerializeField]
    private float maxSmoothAnimationMultiplier = 4f;
    [SerializeField]
    private float shadowAnimationDuration = 0.5f;

    public delegate void OnPlayerWin();
    public event OnPlayerWin onPlayerWin;

    [SerializeField]
    private bool useMouse;

    public bool UseMouse { get => useMouse; }
    public float CorrectPositionAccuracy { get => correctPositionAccuracy; }
    public float AnimationAccuracy { get => animationAccuracy; }
    public float SmoothDragMultiplier { get => smoothDragMultiplier; }
    public float MinSmoothAnimationMultiplier { get => minSmoothAnimationMultiplier; }
    public float MaxSmoothAnimationMultiplier { get => maxSmoothAnimationMultiplier; }
    public float ShadowAnimationDuration { get => shadowAnimationDuration; }

    private void Start()
    {
        completePuzzle.SetActive(false);
    }

    public void IncreasePiecesCount()
    {
        piecesCount++;
    }

    public void DecreasePiecesCount()
    {
        piecesCount--;

        if(piecesCount == 0)
        {
            completePuzzle.SetActive(true);
            puzzleBackground.SetActive(false);

            _balloonsHandler.SpawnBalloons();
            onPlayerWin();
        }
    }

    public void PlayObjectParticle(Vector2 position)
    {
        _particleHandler.PlayParticle(position);
    }
}
