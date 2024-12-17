 using System;
using UnityEngine;
using UnityEngine.UI;
using Cysharp.Threading.Tasks;

public class ExitPanel: CommonPanel
{
    [SerializeField] private Button _cancelButton;

    private float _volume;

    private void Start()
    {
    }

    public override void SetEnable()
    {
        base.SetEnable();
        StopGame();
    }

    private async void StopGame()
    {
        await UniTask.Delay(500);
        _volume = AudioListener.volume;
        AudioListener.volume = 0;
        Time.timeScale = 0;
        _cancelButton.onClick.AddListener(CancelClick);
    }

    private void CancelClick()
    {
        AudioListener.volume = _volume;
        Time.timeScale = 1;
        CommonUIManager.Instance.SwitchPanel(CommonPanelType.Game);
    }

}