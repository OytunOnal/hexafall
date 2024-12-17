using System;
using System.Collections;
using UnityEngine;

    public class CommonPanelAnimation : MonoBehaviour
    {
        [SerializeField] protected CommonPanelTween[] _objectToAnimate;

        Coroutine disableRoutine;

        public void PlayShowUI()
        {
            if (disableRoutine != null)
            {
                StopCoroutine(disableRoutine);
                disableRoutine = null;
            }
                

            foreach (var obj in _objectToAnimate)
            {
                if (obj.isActiveAndEnabled) obj.HandleOnEnable();
            }
        }

        public void SetDisable(Action onComplete)
        {
            float tweenDuration = 0f;

            foreach (var obj in _objectToAnimate)
            {
                obj.HandleOnDisable();

                // get the longest onDisable tween duration
                float duration = obj.GetOnDisableDuration();
                if (tweenDuration < duration)
                    tweenDuration = duration;
            }

            disableRoutine = StartCoroutine(DisableGameobjectRoutine(onComplete, tweenDuration));
        }

        protected IEnumerator DisableGameobjectRoutine(Action onComplete, float duration)
        {
            yield return new WaitForSeconds(duration);
            onComplete?.Invoke();
        }
    }