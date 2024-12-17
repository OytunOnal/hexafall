using System;
using System.Collections;
using UnityEngine;

namespace HexFall
{

    public class PlatformBehaviour : MonoBehaviour
    {
        private static readonly int SHADER_ALPHA_HASH = Shader.PropertyToID("_Alpha");
        private static readonly int SHADER_COLOR_HASH = Shader.PropertyToID("_Color");

        [SerializeField] Transform graphicsTransform;
        [SerializeField] Collider platformCollider;

        private MeshRenderer graphicsMeshRenderer;

        private Vector3 defaultScale;
        private Vector3 defaultPosition;

        private TweenCase scaleCase;
        private TweenCase moveCase;
        public TweenCase AlphaCase { get; private set; }

        private Coroutine changeTranspCor;

        private void Awake()
        {
            graphicsMeshRenderer = graphicsTransform.GetComponent<MeshRenderer>();

            defaultScale = graphicsTransform.localScale;
            defaultPosition = graphicsTransform.localPosition;
        }

        public void Reset()
        {
            graphicsTransform.localScale = defaultScale;
            graphicsTransform.localPosition = defaultPosition;

            //Debug.Log("<color=#7CD1B8>Called Reset Method</color>");
        }

        public void SetVisual(Color layerColor)
        {
            graphicsMeshRenderer.material.SetColor(SHADER_COLOR_HASH, layerColor);
        }

        public void SetActiveState(bool immediately = true) // Turn on collider // Fill grahics
        {
            platformCollider.enabled = true;

            if (immediately)
            {
                graphicsMeshRenderer.material.SetFloat(SHADER_ALPHA_HASH, 1f);
            }
            else
            {
                ChangeTransparency(1f);
            }
        }

        public void SetDisabledState(bool immediately = true) // Turn off collider // Graphics
        {
            platformCollider.enabled = false;

            if (immediately)
            {
                graphicsMeshRenderer.material.SetFloat(SHADER_ALPHA_HASH, 0f);
            }
            else
            {
                ChangeTransparency(0f);
            }
        }

        public void OnStep()
        {
            if (scaleCase != null && scaleCase.isActive)
                scaleCase.Kill();
            if (moveCase != null && moveCase.isActive)
                moveCase.Kill();

            scaleCase = graphicsTransform.DOScale(defaultScale * 0.95f, 0.3f);
            moveCase = graphicsTransform.DOLocalMoveY(defaultPosition.y - 0.15f, 0.3f).SetEasing(Ease.Type.QuartOut);
        }

        public void OnStepOff()
        {
            if (scaleCase != null && scaleCase.isActive)
                scaleCase.Kill();
            if (moveCase != null && moveCase.isActive)
                moveCase.Kill();

            scaleCase = graphicsTransform.DOLocalMoveY(defaultPosition.y, 0.2f).SetEasing(Ease.Type.ExpoOut);
            moveCase = graphicsTransform.DOScale(defaultScale, 0.2f);
        }

        public void ChangeTransparency(float alpha, bool immediately = false, Action onCompleted = null)
        {
            if (immediately)
            {
                graphicsMeshRenderer.material.SetFloat(SHADER_ALPHA_HASH, alpha);
                onCompleted?.Invoke();

                return;
            }

            if (changeTranspCor != null)
            {
                StopCoroutine(changeTranspCor);
                changeTranspCor = null;
            }

            changeTranspCor = StartCoroutine(ChangeTransparencyCoroutine(alpha, onCompleted));
        }

        private IEnumerator ChangeTransparencyCoroutine(float alpha, Action onCompleted = null)
        {
            float startedAlpha = graphicsMeshRenderer.material.GetFloat(SHADER_ALPHA_HASH);
            float currentAlpha = 0f;

            float time = 0;
            float animCoef = 1 / 0.2f;

            do
            {
                yield return null;

                time += Time.deltaTime * animCoef;

                currentAlpha = Mathf.Lerp(startedAlpha, alpha, time);

                graphicsMeshRenderer.material.SetFloat(SHADER_ALPHA_HASH, currentAlpha);

            }
            while (currentAlpha > 0 && currentAlpha < 1);

            onCompleted?.Invoke();
        }

        public void ResetPlatfrom()
        {
            if (scaleCase != null && scaleCase.isActive)
                scaleCase.Kill();
            if (moveCase != null && moveCase.isActive)
                moveCase.Kill();
            if (AlphaCase != null && AlphaCase.isActive)
                AlphaCase.Kill();

            graphicsTransform.gameObject.SetActive(true);
            platformCollider.enabled = true;

            graphicsTransform.localScale = defaultScale;
            graphicsTransform.localPosition = defaultPosition;
            graphicsMeshRenderer.material.SetFloat(SHADER_ALPHA_HASH, 1f);
        }

    }
}