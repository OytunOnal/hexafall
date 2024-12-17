using System;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
    public enum AnimationTypes
    {
        Fade,
        Move,
        Scale
    }

    public class CommonTweenUI : MonoBehaviour
    {
        [System.Serializable]
        private class CommonTweenStat
        {
            public float _duration = .2f;
            public float _delay = 0;

            public Vector3 _from;
            public Vector3 _to;

            public AnimationTypes _animationType;
            public Ease _easeType; // Use DoTween's Ease enum

            public Tweener _tweenObj; // Use DoTween's Tweener type
        }

        [SerializeField] bool _ignoreTimeScale = false;
        [SerializeField] CommonTweenStat _onEnable;
        [SerializeField] CommonTweenStat _onDisable;

        private CanvasGroup _cg;

        private RectTransform _rect;
        private Vector3 _defaultPos;

        private void Awake()
        {
            _rect = GetComponent<RectTransform>();
            _defaultPos = _rect.anchoredPosition;
        }

        private void OnDisable()
        {
            
            DOTween.Kill(this.gameObject);
        }

        public void HandleOnEnable()
        {
            ResetTweenValue();

            HandleTween(_onEnable);
        }

        public void HandleOnDisable()
        {
            HandleTween(_onDisable);
        }

        public float GetOnDisableDuration()
        {
            return _onDisable._delay + _onDisable._duration;
        }

        private void HandleTween(CommonTweenStat stat)
        {
            // Kill any existing tweens on this GameObject
            DOTween.Kill(this.gameObject);

            switch (stat._animationType)
            {
                case AnimationTypes.Fade:
                    Fade(stat);
                    break;
                case AnimationTypes.Move:
                    Move(stat);
                    break;
                case AnimationTypes.Scale:
                    Scale(stat);
                    break;
            }

            stat._tweenObj.SetEase(stat._easeType).SetDelay(stat._delay);

            if (_ignoreTimeScale)
            {
                stat._tweenObj.SetUpdate(true); // Use SetUpdate to control time scale dependency
            }
        }

        private void Move(CommonTweenStat stat)
        {
            _rect.anchoredPosition = _defaultPos + stat._from;

            stat._tweenObj = _rect.DOAnchorPos(_defaultPos + stat._to, stat._duration);
        }

        private void Scale(CommonTweenStat stat)
        {
            _rect.localScale = stat._from;

            stat._tweenObj = _rect.DOScale(stat._to, stat._duration);
        }

        private void Fade(CommonTweenStat stat)
        {
            _cg = gameObject.GetComponent<CanvasGroup>();
            if (!_cg) _cg = gameObject.AddComponent<CanvasGroup>();

            _cg.alpha = stat._from.x;

            stat._tweenObj = _cg.DOFade(stat._to.x, stat._duration);
        }

        private void ResetTweenValue()
        {
            if (_cg) _cg.alpha = 1f;
            if (_rect) _rect.anchoredPosition = _defaultPos;
            if (_rect) _rect.localScale = Vector3.one;
        }
    }
