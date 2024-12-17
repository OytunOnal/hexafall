
using System;
using System.Collections.Generic;
using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DailyChallengePanel : CommonPanel
{
    [SerializeField] private TMP_Text _weekDateText;
    [SerializeField] private Button _leftArrowButton, _rightArrowButton;
    [SerializeField] private GameObject _dailyChallengePlay, _dailyChallengeLock;
    [SerializeField] private List<TMP_Text> _datesText;
    [SerializeField] private List<Image> _datesImage;
    [SerializeField] private Sprite _dateBackground,_dateSelectedBackground;
    [SerializeField] private Color _textColor, _dateBackgroundColor, _dateSelectedBackgroundColor, _dateLockedColor;

    private int _selectedDayIndex = 6;
    private DateTime _startOfWeek;
    private DateTime _selectedDate;
    DateTime _today;
    protected override void Awake()
    {
        _leftArrowButton.onClick.AddListener(ShowPreviousWeek);
        _rightArrowButton.onClick.AddListener(ShowCurrentWeek);

        base.Awake();
    }

    public override void SetEnable()
    {
        ShowCurrentWeek(); 
        base.SetEnable();
    }

    public override void SetDisable()
    {

        base.SetDisable();
    }

    void UpdateWeekText()
    {
        // Calculate the end of the week (Sunday)
        DateTime endOfWeek = _startOfWeek.AddDays(6);

        // Format the dates
        string startOfWeekFormatted = _startOfWeek.ToString("dd MMMM", CultureInfo.InvariantCulture);
        string endOfWeekFormatted = endOfWeek.ToString("dd MMMM", CultureInfo.InvariantCulture);

        // Update the TMPro text
        _weekDateText.text = $"{startOfWeekFormatted} - {endOfWeekFormatted}";

        for (int i = 0; i < _datesText.Count; i++)
        {
            DateTime dayDate = _startOfWeek.AddDays(i);
            
            _datesText[i].text = dayDate.Day.ToString();

            if (dayDate.Day.Equals(_today.Day))
            {
                SelectDay(i);
            }
        }
    }

    public void SelectDay(int dayIndex)
    {
        _datesText[_selectedDayIndex].color = _textColor;
        _datesImage[_selectedDayIndex].color = _dateBackgroundColor;

        _datesText[dayIndex].color = Color.white;
        _datesImage[dayIndex].color = _dateSelectedBackgroundColor;

        // Determine the date of the selected day
        _selectedDate = _startOfWeek.AddDays(dayIndex);
        
        // Check if the selected day is today or before
        if (_selectedDate <= _today.AddDays(1))
        {
            _dailyChallengePlay.SetActive(true);
            _dailyChallengeLock.SetActive(false);
        }
        else
        {
            _dailyChallengePlay.SetActive(false);
            _dailyChallengeLock.SetActive(true);
        }

        _selectedDayIndex = dayIndex;
    }

    public void PlayDailyChallenge()
    {
        string key = _selectedDate.ToString("yyyyMMdd");

        CommonUIManager.Instance.OnDailyChallengePlay.Invoke(key);
        CommonUIManager.Instance.SwitchPanel(CommonPanelType.Game);
    }

    public void ShowPreviousWeek()
    {
        _rightArrowButton.gameObject.SetActive(true);
        _leftArrowButton.gameObject.SetActive(false);
        _startOfWeek = _startOfWeek.AddDays(-7);
        UpdateWeekText();
        SelectDay(0);
    }

    public void ShowCurrentWeek()
    {
        _rightArrowButton.gameObject.SetActive(false);
        _leftArrowButton.gameObject.SetActive(true);
        _startOfWeek = DateTime.Now.AddDays(-(int)DateTime.Now.DayOfWeek + (int)DayOfWeek.Monday);
        _today = DateTime.Now.Date;
        UpdateWeekText();
    }

}
