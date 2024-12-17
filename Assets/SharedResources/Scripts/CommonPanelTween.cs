using UnityEngine;
    public enum CommonPanelAnimationTypes
    {
        Fade,
        Move,
        Scale
    }

    public class CommonPanelTween : MonoBehaviour
    {
        [System.Serializable]
        private class CommonPanelTweenStat
        {
            public float _duration = .2f;
            public float _delay = 0;

            public Vector3 _from;
            public Vector3 _to;

            public CommonPanelAnimationTypes _animationType;
            public LeanTweenType _easeType;

            public LTDescr _tweenObj;
        }

        [SerializeField] bool _ignoreTimeScale = false;
        [SerializeField] CommonPanelTweenStat _onEnable;
        [SerializeField] CommonPanelTweenStat _onDisable;

        private CanvasGroup _cg;

        private RectTransform _rect;
        private Vector3 _defaultPos;

        private void Awake()
        {
            _rect = GetComponent<RectTransform>();
            _defaultPos = _rect.anchoredPosition;
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

        private void HandleTween(CommonPanelTweenStat stat)
        {
            LeanTween.cancel(this.gameObject);

            switch (stat._animationType)
            {
                case CommonPanelAnimationTypes.Fade:
                    Fade(stat);
                    break;
                case CommonPanelAnimationTypes.Move:
                    Move(stat);
                    break;
                case CommonPanelAnimationTypes.Scale:
                    Scale(stat);
                    break;
            }

            stat._tweenObj.setEase(stat._easeType).setDelay(stat._delay);

            if (_ignoreTimeScale)
            {
                stat._tweenObj.setIgnoreTimeScale(true);
            }
        }

        private void Move(CommonPanelTweenStat stat)
        {
            _rect.anchoredPosition = _defaultPos + stat._from;

            stat._tweenObj = LeanTween.move(_rect, _defaultPos + stat._to, stat._duration);
        }

        private void Scale(CommonPanelTweenStat stat)
        {
            _rect.localScale = stat._from;

            stat._tweenObj = LeanTween.scale(_rect, stat._to, stat._duration);
        }

        private void Fade(CommonPanelTweenStat stat)
        {
            _cg = _cg != null ? _cg : gameObject.AddComponent<CanvasGroup>();

            _cg.alpha = stat._from.x;

            stat._tweenObj = LeanTween.alphaCanvas(_cg, stat._to.x, stat._duration);
        }

        private void ResetTweenValue()
        {
            if (_cg) _cg.alpha = 1f;
            if (_rect) _rect.anchoredPosition = _defaultPos;
            if (_rect) _rect.localScale = Vector3.one;
        }
    }