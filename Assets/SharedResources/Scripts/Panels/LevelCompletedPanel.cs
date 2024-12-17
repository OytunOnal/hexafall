 using System;
using UnityEngine;
using UnityEngine.UI;

public class LevelCompletedPanel: CommonPanel
{
    [SerializeField] private GameObject _nextButton, _backToFeedButton, noLevelText; 
    private void Start()
    {
    }

    public override void SetEnable()
    {
        if (!CommonUIManager.Instance.canPlay)
        {
            _nextButton.SetActive(false);
            noLevelText.SetActive(true);
            _backToFeedButton.GetComponent<RectTransform>().anchoredPosition =
                new Vector2(_backToFeedButton.GetComponent<RectTransform>().anchoredPosition.x,
                700);
        }
        base.SetEnable();
    }
}