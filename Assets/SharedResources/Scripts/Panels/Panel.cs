using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public enum CommonPanelType
{
    None,
    Game,
    LevelCompleted,
    Star,
    Score,
    Win,
    Lose,
    GameOver,
    NotAvailable,
    Exit,
    LevelSelection,
    DailyChallenge
}

[RequireComponent(typeof(CommonPanelAnimation))]
public abstract class CommonPanel : MonoBehaviour
{
    [SerializeField] CommonPanelType _type;
    [SerializeField] bool _useAnimation = false;

    private CommonPanelAnimation _menuAnimation;
    private Canvas _canvas;

    public CommonPanelType Type => _type;

    protected virtual void Awake()
    {
        _canvas = GetComponent<Canvas>();
        _menuAnimation = GetComponent<CommonPanelAnimation>();
    }

    public virtual void SetEnable()
    {
        _canvas.enabled = true;
        if (!_useAnimation) return;

        _menuAnimation.PlayShowUI();
    }

    public virtual void SetDisable()
    {
        if (!_useAnimation)
        {
            DisableCanvas();
            return;
        }

        _menuAnimation.SetDisable(DisableCanvas);
    }

    public void DisableCanvas()
    {
        //if (_menuManager.GetCurrentMenu == _type) return;
        _canvas.enabled = false;
    }

    protected void OnButtonPressed(Button button, UnityAction buttonListener)
    {
        if (!button)
        {
            Debug.LogWarning($"There is a 'Button' that is not attached to the '{gameObject.name}' script,  but a script is trying to access it.");
            return;
        }

        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(buttonListener);
    }
}
