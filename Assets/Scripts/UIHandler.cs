using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIHandler : MonoBehaviour
{
    [SerializeField]
    private GameObject _previousButton;
    [SerializeField]
    private GameObject _nextButton;

    private PuzzleHandler _puzzleHandler;

    private void OnEnable()
    {
        _puzzleHandler.onPlayerWin += ActivateButtons;
    }

    private void Awake()
    {
        _puzzleHandler = FindObjectOfType<PuzzleHandler>();
    }

    private void OnDisable()
    {
        _puzzleHandler.onPlayerWin -= ActivateButtons;
    }

    private void ActivateButtons()
    {
        _previousButton.SetActive(SceneManager.GetActiveScene().buildIndex > 1);
        _nextButton.SetActive(SceneManager.GetActiveScene().buildIndex < SceneManager.sceneCountInBuildSettings - 1);
    }
}
