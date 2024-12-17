using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "UpcomingLevelDataHolder",menuName = "SharedResources/LevelSelection/UpcomingLevelDataHolder")]
public class UpcomingLevelDataHolder : ScriptableObject
{
    public string levelName;
    public int levelNo;
    public bool isPlayable;

    public void Initialize(UpcomingLevelData data)
    {
        levelName = data.levelName;
        levelNo = data.levelNo;
        isPlayable = data.isPlayable;
    }
}

public struct UpcomingLevelData
{
    public string levelName;
    public int levelNo;
    public bool isPlayable;

    public UpcomingLevelData(string levelName,int levelNo,bool isPlayable)
    {
        this.levelName = levelName;
        this.levelNo = levelNo;
        this.isPlayable = isPlayable;
    }
}
