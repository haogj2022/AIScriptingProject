using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelMenu : MonoBehaviour
{
    public GameObject[] levels;
    public static int level = 0;

    void Awake()
    {
        LoadLevel();
    }

    void LoadLevel()
    {
        foreach (GameObject level in levels)
        {
            level.SetActive(false);
        }

        switch (level)          
        {
            case 0:
                levels[0].SetActive(true);
                break;
            case 1:
                levels[1].SetActive(true);
                break;
            case 2:
                levels[2].SetActive(true);
                break;
            default:
                levels[0].SetActive(true);
                break;
        }
    }

    public void NextLevel()
    {
        level++;

        if (level > levels.Length)
        {
            level = 0;
        }
    }
}
