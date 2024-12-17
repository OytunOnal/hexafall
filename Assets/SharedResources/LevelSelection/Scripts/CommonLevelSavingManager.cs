using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommonLevelSavingManager : MonoBehaviour
{
    public static CommonLevelSavingManager Instance;
    [SerializeField] private CommonDayManager commonDayManager;
    [SerializeField] private LevelSelectionManager levelSelectionManager;
    [SerializeField] private GameObject comeBackTomorrowWarning;
    [SerializeField] private GameObject noLevelsPlayedWarning;
    public string gameName;
    public List<int> savedLevelNos;
    public int maxLevelNo = 0;



    private void Awake()
    {
        Instance = this;
    }


    public void Initialize()
    {
        string savedData = PlayerPrefs.GetString(gameName, "");
        if (!string.IsNullOrEmpty(savedData))
        {
            LevelNoData data = JsonUtility.FromJson<LevelNoData>(savedData);
            savedLevelNos = data.saveData;
        }
        
        commonDayManager.Initialize(gameName);

        List<CompletedLevelDataHolder> completedLevels = new List<CompletedLevelDataHolder>();
        if (savedLevelNos.Count > 0)
        {
            noLevelsPlayedWarning.SetActive(false);
            foreach (int levelNo in savedLevelNos)
            {
                CompletedLevelData c = JsonUtility.FromJson<CompletedLevelData>(PlayerPrefs.GetString(gameName + levelNo));
                CompletedLevelDataHolder holder = ScriptableObject.CreateInstance<CompletedLevelDataHolder>();
                holder.Initialize(c);
                completedLevels.Add(holder);
            }
        }
        else
        {
            noLevelsPlayedWarning.SetActive(true);
        }
        
        levelSelectionManager.CreateCompletedLevels(completedLevels);

        int startingPoint = savedLevelNos.Count > 0 ? savedLevelNos[^1] + 1 : 0;

        if (startingPoint + 1 > maxLevelNo)
        {
            levelSelectionManager.DisableUpcomingLevels();
            return;
        }

        if (!commonDayManager.CanPlay())
        {
            comeBackTomorrowWarning.SetActive(true);
        }
        else
        {
            comeBackTomorrowWarning.SetActive(false);
            List<UpcomingLevelDataHolder> upcomingLevels = new List<UpcomingLevelDataHolder>();
            UpcomingLevelData upcomingLevelData = new UpcomingLevelData("Level: " + (startingPoint + 1), startingPoint, true);
            UpcomingLevelDataHolder upcomingLevelDataHolder = ScriptableObject.CreateInstance<UpcomingLevelDataHolder>();
            upcomingLevelDataHolder.Initialize(upcomingLevelData);
            upcomingLevels.Add(upcomingLevelDataHolder);
            for (int i = startingPoint + 1; i < startingPoint + commonDayManager.maxLevelCount - commonDayManager.playedLevelsCount; i++)
            {
                if (i > maxLevelNo)
                {
                    break;
                }
                UpcomingLevelData levelData = new UpcomingLevelData("Level: " + (i + 1), i, false);
                UpcomingLevelDataHolder levelDataHolder = ScriptableObject.CreateInstance<UpcomingLevelDataHolder>();
                levelDataHolder.Initialize(levelData);
                upcomingLevels.Add(levelDataHolder);
            }
            levelSelectionManager.CreateUpcomingLevels(upcomingLevels);
        }

    }


    public void LevelCompleted(CompletedLevelData levelId)
    {
        if (savedLevelNos.Contains(levelId.levelNo))
        {
            CompletedLevelData c = JsonUtility.FromJson<CompletedLevelData>(PlayerPrefs.GetString(gameName + levelId.levelNo));
            if(levelId.starAmount > c.starAmount)
                PlayerPrefs.SetString(gameName + levelId.levelNo, JsonUtility.ToJson(levelId));
        }
        else
        {
            CommonDayManager.Instance.GamePlayed();
            savedLevelNos.Add(levelId.levelNo);
            string saveData = JsonUtility.ToJson(new LevelNoData(savedLevelNos));
            PlayerPrefs.SetString(gameName, saveData);
            PlayerPrefs.SetString(gameName + levelId.levelNo, JsonUtility.ToJson(levelId));
        }
    }
    
    
    
    
}

public struct LevelNoData
{
    public List<int> saveData;

    public LevelNoData(List<int> savedLevelNos)
    {
        saveData = savedLevelNos;
    }
}
