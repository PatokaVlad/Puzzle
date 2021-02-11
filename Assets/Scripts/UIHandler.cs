using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIHandler : MonoBehaviour
{
    [SerializeField]
    private GameObject previousButton;
    [SerializeField]
    private GameObject nextButton;
    [SerializeField]
    private GameObject puzzleName;

    private PuzzleHandler _puzzleHandler;

    private void OnEnable()
    {
        _puzzleHandler.onPlayerWin += ActivateUI;
    }

    private void Awake()
    {
        _puzzleHandler = FindObjectOfType<PuzzleHandler>();
    }

    private void Start()
    {
        previousButton.SetActive(false);
        nextButton.SetActive(false);
        puzzleName.SetActive(false);
    }

    private void OnDisable()
    {
        _puzzleHandler.onPlayerWin -= ActivateUI;
    }

    private void ActivateUI()
    {
        previousButton.SetActive(SceneManager.GetActiveScene().buildIndex > 1);
        nextButton.SetActive(SceneManager.GetActiveScene().buildIndex < SceneManager.sceneCountInBuildSettings - 1);

        puzzleName.SetActive(true);
    }
}
