 using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScorePanel: CommonPanel
{
    [SerializeField] private GameObject _scorePanel;
    [SerializeField] private TMP_Text _scoreText;
    [SerializeField] private TMP_Text _highScoreTextForScore;

    [SerializeField] private GameObject _highScorePanel;
    [SerializeField] private TMP_Text _highScoreTextForHighScore;

    private void Start()
    {
    }

    public override void SetEnable()
    {
        if (!CommonUIManager.Instance.isNewHighScore || CommonUIManager.Instance.currentScore == 0)
        {
            _scoreText.text = CommonUIManager.Instance.currentScore.ToString();
            _highScoreTextForScore.text = "Highest Score: " + "<b><color=#000000>" + CommonUIManager.Instance.highScore.ToString() + "</color></b>";
            _scorePanel.SetActive(true);
        }
        else
        {
            _highScorePanel.SetActive(true);
            _highScoreTextForHighScore.text = CommonUIManager.Instance.highScore.ToString();
        }
        base.SetEnable();
    }
}