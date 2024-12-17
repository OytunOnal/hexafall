using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HexFall
{

    public class EditorPlatformBehaviour : MonoBehaviour
    {
        [SerializeField] MeshRenderer meshRenderer;
#if UNITY_EDITOR
        private Vector2Int gridPosition;
        public HexType editorHexType;

        public Vector2Int GridPosition { get => gridPosition; set => gridPosition = value; }
        public HexType EditorHexType
        {
            get => editorHexType;
            set
            {
                editorHexType = value;
                meshRenderer.sharedMaterial = EditorSceneController.Instance.GetMaterial(gridPosition, editorHexType);
            }
        }
#endif
    }
}