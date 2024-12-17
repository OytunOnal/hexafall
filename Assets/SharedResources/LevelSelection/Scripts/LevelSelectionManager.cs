using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSelectionManager : CommonPanel
{
    public static Action<int> OnPlayClicked;

    [SerializeField] private Canvas canvas;
    [SerializeField] private Transform completedLevelsHolder;
    [SerializeField] private Transform upcomingLevelsHolder;
    [SerializeField] private GameObject upcomingLevelsPanel;
    [SerializeField] private RectTransform completedLevelsPanel;
    [SerializeField] private UpcomingLevel upcomingLevelPrefab;
    [SerializeField] private CompletedLevel completedLevelPrefab;
    [SerializeField] private CommonLevelSavingManager levelSavingManager;

    public override void SetEnable()
    {
        upcomingLevelsHolder.GetComponent<RectTransform>().anchoredPosition = Vector3.zero;
        levelSavingManager.Initialize();
        base.SetEnable();
    }


    public void CreateCompletedLevels(List<CompletedLevelDataHolder> completedLevels)
    {
        foreach (CompletedLevelDataHolder level in completedLevels)
        {
            CompletedLevel c = Instantiate(completedLevelPrefab, completedLevelsHolder);
            c.Initialize(level,OnLevelReplayClick);
        }
    }

    public void CreateUpcomingLevels(List<UpcomingLevelDataHolder> upcomingLevels)
    {
        foreach (UpcomingLevelDataHolder level in upcomingLevels)
        {
            UpcomingLevel l = Instantiate(upcomingLevelPrefab, upcomingLevelsHolder);
            l.Initialize(level,OnPlayClick);
        }
    }

    public void DisableUpcomingLevels()
    {
        completedLevelsPanel.offsetMin = new Vector2(completedLevelsPanel.offsetMin.x, -700);
        upcomingLevelsPanel.SetActive(false);
    }

    private void OnLevelReplayClick(CompletedLevel level)
    {
        OnPlayClicked?.Invoke(level.levelNo);
        CommonUIManager.Instance.SwitchPanel(CommonPanelType.Game);
    }

    private void OnPlayClick(UpcomingLevel level)
    {
        OnPlayClicked?.Invoke(level.levelNo);
        CommonUIManager.Instance.SwitchPanel(CommonPanelType.Game);
    }
}
