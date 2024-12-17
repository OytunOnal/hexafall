using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommonDayManager : MonoBehaviour
{
    public static CommonDayManager Instance;
    public string lastPlayDateHolder;
    public string lastPlayCountHolder;
    private DateTime _lastPlayDate;
    public int playedLevelsCount = 0;
    public int maxLevelCount = 1;

    private void Awake()
    {
        Instance = this;
    }


    public void Initialize(string gameName)
    {
        lastPlayDateHolder = gameName +  "LastPlayDate";
        lastPlayCountHolder = gameName + "LastPlayCount";
        
        playedLevelsCount = PlayerPrefs.GetInt(lastPlayCountHolder, 0);

        string s = PlayerPrefs.GetString(lastPlayDateHolder, DateTime.Today.AddDays(-1).ToString());
        _lastPlayDate = DateTime.Parse(s);            
    }

    public bool CanPlay()
    {
        if (_lastPlayDate == DateTime.Today)
        {
            if (playedLevelsCount >= maxLevelCount)
                return false;
            else return true;
        }
        else
        {
            playedLevelsCount = 0;
            _lastPlayDate = DateTime.Today;
            PlayerPrefs.SetString(lastPlayDateHolder, _lastPlayDate.ToString());
            PlayerPrefs.SetInt(lastPlayCountHolder, playedLevelsCount);
            return true;
        }
    }

    public void GamePlayed()
    {
        playedLevelsCount++;
        PlayerPrefs.SetInt(lastPlayCountHolder, playedLevelsCount);
    }
    
}
