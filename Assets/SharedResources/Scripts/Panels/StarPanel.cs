using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarPanel : CommonPanel
{
    [SerializeField] private List<Animator> _starAnimators;
    [SerializeField] private GameObject _levelAvailable;
    [SerializeField] private GameObject _levelNotAvailable;
    private Coroutine _currentCoroutine;

    public override void SetEnable()
    {
        if (CommonDayManager.Instance.CanPlay())
        {
            Debug.Log("NextLevelAvailable");
            _levelAvailable.SetActive(true);
            _levelNotAvailable.SetActive(false);
        }
        else
        {
            Debug.Log("NextLevel noıt Available");
            _levelAvailable.SetActive(false);
            _levelNotAvailable.SetActive(true);
        }

        foreach (var animator in _starAnimators)
        {
            animator?.Play("idle");
        }
        StartStarAnim();

        base.SetEnable();
    }

    public override void SetDisable()
    {
        CancelStarAnim();

        base.SetDisable();
    }

    private void CancelStarAnim()
    {
        if (_currentCoroutine != null)
        {
            StopCoroutine(_currentCoroutine);
            _currentCoroutine = null;
        }
    }

    public void StartStarAnim()
    {
        CancelStarAnim(); // Ensure any existing animation is cancelled before starting a new one
        _currentCoroutine = StartCoroutine(StarAnim());
    }

    private IEnumerator StarAnim()
    {
        yield return new WaitForSeconds(0.5f);

        switch (CommonUIManager.Instance.starCount)
        {
            case 1:
                if (_starAnimators[0] != null)
                {
                    _starAnimators[0].Play("star anim");
                }
                break;
            case 2:
                if (_starAnimators[0] != null)
                {
                    _starAnimators[0].Play("star anim");
                }
                yield return new WaitForSeconds(0.5f);
                if (_starAnimators[2] != null)
                {
                    _starAnimators[2].Play("star anim");
                }
                break;
            case 3:
                if (_starAnimators[0] != null)
                {
                    _starAnimators[0].Play("star anim");
                }
                yield return new WaitForSeconds(0.5f);
                if (_starAnimators[2] != null)
                {
                    _starAnimators[2].Play("star anim");
                }
                yield return new WaitForSeconds(0.5f);
                if (_starAnimators[1] != null)
                {
                    _starAnimators[1].Play("star anim");
                }
                break;
        }
    }
}
