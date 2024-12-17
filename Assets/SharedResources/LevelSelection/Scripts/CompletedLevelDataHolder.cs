using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CompletedLevelDataHolder",menuName = "SharedResources/LevelSelection/CompletedLevelDataHolder")]
public class CompletedLevelDataHolder : ScriptableObject
{
    public bool hasStars;
    public int starAmount;
    public string levelName;
    public int levelNo;
    public string gameName;


    public void Initialize(CompletedLevelData data)
    {
        hasStars = data.hasStars;
        starAmount = data.starAmount;
        levelName = data.levelName;
        levelNo = data.levelNo;
        gameName = data.gameName;
    }
}

public struct CompletedLevelData
{
    public bool hasStars;
    public int starAmount;
    public string levelName;
    public int levelNo;
    public string gameName;


    public CompletedLevelData(bool stars, int starCount, string levelName, int levelNo, string gameName)
    {
        hasStars = stars;
        starAmount = starCount;
        this.levelName = levelName;
        this.levelNo = levelNo;
        this.gameName = gameName;
    }
    
}