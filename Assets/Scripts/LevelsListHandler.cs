using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelsListHandler : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> levelsGroup = new List<GameObject>();

    private void Start()
    {
        CreateGroups();
    }

    private void CreateGroups()
    {
        for(int i = 0; i < levelsGroup.Count; i++)
        {
            GameObject levelGroupToSpawn = levelsGroup[i];
            Instantiate(levelGroupToSpawn, transform);
        } 
    }
}
