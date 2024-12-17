using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommonUIManager : Singleton<CommonUIManager>
{
    [SerializeField]
    private List<CommonPanel> _panels = new List<CommonPanel>();

    private Hashtable _panelTable = new Hashtable();
    private Stack<CommonPanel> _panelStack = new Stack<CommonPanel>();

    private CommonPanelType _currentMenu;

    public int currentScore, highScore,starCount;
    public bool canPlay = true;

    public bool isNewHighScore = false;
    // property
    public CommonPanelType GetCurrentMenu => _currentMenu;
    [SerializeField] private CommonPanelType startPanelType = CommonPanelType.Game;

    public Action<string> OnDailyChallengePlay;

    private void Start()
    {
        RegisterAllPanels();

        OpenPanel(startPanelType);
        
    }

    #region Public Functions

    public void OnGamePause()
    {
        Time.timeScale = 0;
        Debug.Log("Game Paused");
    }

    public void OnGameResume()
    {
        Time.timeScale = 1;
        Debug.Log("Game Resumed");
    }

    public void SwitchPanel(CommonPanelType type)
    {
        ClosePanel();    // Disable the last menu
        OpenPanel(type); // Open desired menu
    }

    public void SetScore(int newScore, int newHighScore)
    {
        currentScore = newScore;
        if (newHighScore > highScore)
        {
            highScore = newHighScore;
            isNewHighScore = true;
        }
    }

    public void SetHighScore(int newHighScore)
    {
        highScore = newHighScore;
    }

    internal void SetStar(int stars)
    {
        starCount = stars;
    }
    public void OpenPanel(CommonPanelType type)
    {
        if (type == CommonPanelType.None) return;
        if (!PanelExist(type))
        {
            Debug.LogWarning($"You are trying to open a Panel {type} that has not been registered.");
            return;
        }

        CommonPanel menu = GetPanel(type);
        menu.SetEnable();
        _panelStack.Push(menu);

        _currentMenu = menu.Type;
    }

    public void ClosePanel()
    {
        if (_panelStack.Count <= 0)
        {
            Debug.LogWarning("MenuController CloseMenu ERROR: No menus in stack!");
            return;
        }
        CommonPanel lastMenuStack = _panelStack.Pop();

        // Disable GameObject
        lastMenuStack.SetDisable();

        if (_panelStack.Count > 0)
            _currentMenu = _panelStack.Peek().Type;
    }
    #endregion

    #region Private Functions
    private void RegisterAllPanels()
    {
        foreach (CommonPanel menu in _panels)
        {
            RegisterPanel(menu);

            // disable menu after register to hash table.
            menu.DisableCanvas();
        }
    }


    private void RegisterPanel(CommonPanel menu)
    {
        if (menu.Type == CommonPanelType.None)
        {
            Debug.LogWarning($"You are trying to register a {menu.Type} type menu that has not allowed.");
            return;
        }

        if (PanelExist(menu.Type))
        {
            Debug.LogWarning($"You are trying to register a Menu {menu.Type} that has already been registered.");
            return;
        }

        _panelTable.Add(menu.Type, menu);
    }

    private CommonPanel GetPanel(CommonPanelType type)
    {
        if (!PanelExist(type)) return null;

        return (CommonPanel)_panelTable[type];
    }

    private bool PanelExist(CommonPanelType type)
    {
        return _panelTable.ContainsKey(type);
    }
    #endregion
}