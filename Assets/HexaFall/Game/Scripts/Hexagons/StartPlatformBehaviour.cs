using UnityEngine;

namespace HexFall
{

    public class StartPlatformBehaviour : MonoBehaviour
    {
        [SerializeField] Transform graphicsTransform;

        private MeshRenderer graphicsMeshRenderer;

        private Vector3 defaultScale;
        private Vector3 defaultPosition;
        private Color defaultColor;

        private MaterialPropertyBlock materialPropertyBlock;

        private static readonly int SHADER_TINT_HASH = Shader.PropertyToID("_Tint");

        public Vector3 Position => transform.position;

        private void Awake()
        {
            graphicsMeshRenderer = graphicsTransform.GetComponent<MeshRenderer>();

            defaultScale = graphicsTransform.localScale;
            defaultPosition = graphicsTransform.localPosition;

            graphicsTransform.gameObject.SetActive(true);

            materialPropertyBlock = new MaterialPropertyBlock();
        }

        public void Init()
        {
            graphicsTransform.localScale = defaultScale;
            graphicsMeshRenderer.material.SetColor(SHADER_TINT_HASH, defaultColor);
        }

        public void DisablePlatform()
        {
            graphicsTransform.DOScale(defaultScale * 0.95f, 0.3f).SetEasing(Ease.Type.Linear);
            graphicsTransform.DOLocalMoveY(defaultPosition.y - 0.15f, 0.3f).SetEasing(Ease.Type.QuartOut).OnComplete(delegate
            {
                graphicsTransform.DOLocalMoveY(defaultPosition.y, 0.2f).SetEasing(Ease.Type.ExpoOut);

                Tween.DoColor(graphicsMeshRenderer.material.GetColor(SHADER_TINT_HASH), Color.white, 0.4f, (color) =>
                {
                    graphicsMeshRenderer.GetPropertyBlock(materialPropertyBlock);
                    materialPropertyBlock.SetColor(SHADER_TINT_HASH, color);
                    graphicsMeshRenderer.SetPropertyBlock(materialPropertyBlock);
                }).OnComplete(delegate
                {
                    graphicsTransform.gameObject.SetActive(false);
                    transform.position = transform.position.SetY(99999);

                    Tween.NextFrame(() => gameObject.SetActive(false));
                });
            });
        }
    }
}