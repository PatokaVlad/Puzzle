using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class SceneLoader : MonoBehaviour
{
    public void LoadLevelScene(int index)
    {
        if (index >= 0 && index <= SceneManager.sceneCount) 
            SceneManager.LoadScene(index);
    }

    public void LoadNextLevel()
    {
        int sceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
        if (SceneManager.sceneCount >= sceneIndex)
            SceneManager.LoadScene(sceneIndex);
    }

    public void LoadPreviousLevel()
    {
        int sceneIndex = SceneManager.GetActiveScene().buildIndex - 1;
        if (sceneIndex >= 1)
            SceneManager.LoadScene(sceneIndex, LoadSceneMode.Additive);
    }
}
