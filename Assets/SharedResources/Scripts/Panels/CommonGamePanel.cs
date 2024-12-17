using Cysharp.Threading.Tasks;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CommonGamePanel: CommonPanel
{

    [SerializeField] private TMP_Text _scoreText;
    [SerializeField] private TMP_Text _highScoreText;

    private void Start()
    {
    }

    public override void SetEnable()
    {
        base.SetEnable();
        SetHighScore(); 
    }

    private async void SetHighScore()
    {
        await UniTask.Delay(500);
        CommonUIManager.Instance.SetHighScore(int.Parse(_highScoreText.text.ToUpper().Replace("BEST: ", "")));
    }

    public override void SetDisable()
    {
        if (_scoreText.IsActive())
            CommonUIManager.Instance.SetScore(int.Parse(_scoreText.text), int.Parse(_highScoreText.text.ToUpper().Replace("BEST: ","")));
        base.SetDisable();
    }
}