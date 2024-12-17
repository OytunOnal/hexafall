using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CommonGameOverUI : MonoBehaviour {

    public static CommonGameOverUI Instance { get; private set; }

    [SerializeField] private Button _nextButton;
    [SerializeField] private Button _restartButton;
    [SerializeField] private Image _backgroundImage;
    [SerializeField] private GameObject _gameOverPanel;

    [SerializeField] private GameObject _gameOver;
    [SerializeField] private GameObject _victory;

    private float _backgroundAlpha;


    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        _backgroundAlpha = _backgroundImage.color.a;
    }


    public void OnGameOvered()
    {
        _gameOver.SetActive(true);
        _victory.SetActive(false);
        Show();
    }

    public void OnVictory()
    {
        _victory.SetActive(true);
        _gameOver.SetActive(false);
        Show();
    }


    public bool HasGameOvered()
    {
        if (_gameOverPanel.activeSelf) return true;
        else return false;
    }

    private void Show()
    {
        _gameOverPanel.SetActive(true);
        ShowEffect();
    }

    private void ShowEffect()
    {
        RectTransform rectTransform = GetComponent<RectTransform>();
        Sequence sq = DOTween.Sequence();
        sq.SetLink(gameObject);
        sq.SetUpdate(true);
        sq.Append(rectTransform.DOLocalMoveY(100, 1f).From().SetEase(Ease.OutBounce));
        sq.Join(_backgroundImage.DOFade(_backgroundAlpha, 1f).SetEase(Ease.OutExpo));
        sq.OnComplete(() =>
        {
            ButtonEffect(_nextButton);
            ButtonEffect(_restartButton);
        });
    }

    private void ButtonEffect(Button button)
    {
        button.GetComponent<RectTransform>().DOScale(2.75f, .5f)
            .SetUpdate(true)
            .SetLoops(-1, LoopType.Yoyo)
            .SetLink(gameObject);
    }

}

