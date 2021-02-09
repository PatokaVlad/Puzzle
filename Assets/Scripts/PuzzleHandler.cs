using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleHandler : MonoBehaviour
{
    [SerializeField]
    private BalloonsHandler _balloonsHandler;

    private int piecesCount = 0;

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
        }
    }
}
