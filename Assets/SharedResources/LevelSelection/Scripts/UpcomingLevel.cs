using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpcomingLevel : MonoBehaviour
{
    [SerializeField] private bool isPlayable;
    [SerializeField] private Button playButton;
    [SerializeField] private GameObject lockImage;
    [SerializeField] private TMP_Text levelNameText;

    public int levelNo;
    
    
    public void Initialize(UpcomingLevelDataHolder level, Action<UpcomingLevel> onPlayClick)
    {
        levelNameText.text = level.levelName;
        levelNo = level.levelNo;
        isPlayable = level.isPlayable;

        if (isPlayable)
        {
            lockImage.SetActive(false);
            playButton.gameObject.SetActive(true);
            playButton.onClick.AddListener(() => onPlayClick.Invoke(this));
        }
        else
        {
            lockImage.SetActive(true);
            playButton.gameObject.SetActive(false);
        }
    }
}
