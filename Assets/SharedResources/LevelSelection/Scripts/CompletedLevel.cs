using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class CompletedLevel : MonoBehaviour
{
    [SerializeField] private GameObject starsHolder;
    [SerializeField] private GameObject[] stars;
    [SerializeField] private GameObject[] unStars;
    [SerializeField] private TMP_Text levelName;

    public int levelNo;
    public Button replayButton;
    
    
    public void Initialize(CompletedLevelDataHolder level,Action<CompletedLevel> onReplayButtonClick)
    {
        if (level.hasStars)
        {
            starsHolder.SetActive(true);
            for (int i = 0; i < 3; i++)
            {
                if (i < level.starAmount)
                {
                    stars[i].SetActive(true);
                    unStars[i].SetActive(false);
                }
                else
                {
                    stars[i].SetActive(false);
                    unStars[i].SetActive(true);
                }
                
            }
        }
        else
        {
            starsHolder.SetActive(false);
        }

        levelName.text = level.levelName;
        levelNo = level.levelNo;
        replayButton.onClick.AddListener(() =>onReplayButtonClick.Invoke(this));
    }
}
