#pragma warning disable 649

using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace HexFall
{
    [ExecuteInEditMode]
    public class EditorSceneController : MonoBehaviour
    {

#if UNITY_EDITOR
        private static EditorSceneController instance;
        public static EditorSceneController Instance { get => instance; }

        public const float HEX_WIDTH = 1.02f;
        public const float HEX_WIDTH_HALF = 0.51f;
        public const float HEX_HEIGHT = 0.88f;
        public const float HEX_HEIGHT_HALF = 0.44f;

        [Header("Tints")]
        [SerializeField] private Material materialPrefab;
        [SerializeField] private Color groundColor;
        [SerializeField] private Color borderColor;
        [SerializeField] private Color hiddenColor;
        [SerializeField] private Color centralColor;


        [Header("Other stuff")]
        [SerializeField] private GameObject container;
        [SerializeField] private GameObject hexPlatformPrefab;
        [SerializeField] private Camera sceneCamera;
        [SerializeField] private Vector2Int fieldSize;
        

        //Materials
        private Material groundHexMaterial;
        private Material borderHexMaterial;
        private Material hiddenHexMaterial;
        private Material centralMaterial;

        Vector3 startPos;
        private float offset;
        private float xCoordinate;
        private float zCoordinate;
        private float yCoordinate;


        //handle mouse
        private Event currentEvent;
        private Vector2 fixedMousePosition;
        private Ray ray;
        private RaycastHit hitInfo;
        private GameObject lastSelectedGameObject;
        private bool isMaterialsInitialized;
        private HexType selectedHexType;
        private EditorPlatformBehaviour editorPlatformBehaviour;

        public float CameraY
        {
            get { return sceneCamera.transform.position.y; }
            set { sceneCamera.transform.position = sceneCamera.transform.position.SetY(value); }
        }

        public HexType SelectedHexType { get => selectedHexType; set => selectedHexType = value; }
        public Color GroundColor => groundColor;
        public Color BorderColor => borderColor;
        public Color HiddenColor => hiddenColor;

        public EditorSceneController()
        {
            instance = this;
        }

        public void InitMaterials()
        {
            groundHexMaterial = new Material(materialPrefab);
            borderHexMaterial = new Material(materialPrefab);
            hiddenHexMaterial = new Material(materialPrefab);
            centralMaterial = new Material(materialPrefab);
            groundHexMaterial.color = groundColor;
            borderHexMaterial.color = borderColor;
            hiddenHexMaterial.color = hiddenColor;
            centralMaterial.color = centralColor;
        }

        public Material GetMaterial(Vector2Int gridPosition, HexType editorHexType)
        {
            if (!isMaterialsInitialized)
            {
                InitMaterials();
                isMaterialsInitialized = true;
            }

            if (IsCentralHex(gridPosition))
            {
                return centralMaterial;
            }
            else
            {
                switch (editorHexType)
                {
                    case HexType.Ground:
                        return groundHexMaterial;
                    case HexType.Border:
                        return borderHexMaterial;
                    case HexType.Hidden:
                    default:
                        return hiddenHexMaterial;
                }
            }
        }

        public bool IsCentralHex(Vector2Int gridPosition)
        {
            return (gridPosition == new Vector2Int(0, -2) || gridPosition == new Vector2Int(-1, -1) || gridPosition == new Vector2Int(2, -1) || gridPosition == new Vector2Int(-1, 1) || gridPosition == new Vector2Int(2, 1) || gridPosition == new Vector2Int(0, 2));
        }

        public void UpdateSelectedHexType(int value)
        {
            switch (value)
            {
                case 1:
                    selectedHexType = HexType.Border;
                    break;
                case 2:
                    selectedHexType = HexType.Hidden;
                    break;
                case 0: 
                default:
                    selectedHexType = HexType.Ground;
                    break;
            }
        }

        public void SelectGameObject(GameObject selectedGameObject)
        {
            Selection.activeGameObject = selectedGameObject;
        }


        public void CreateGrid()
        {
            GameObject tempHex;
            Vector2Int gridPosition;
            int halfSizeY = fieldSize.y / 2;
            int halfSizeX = fieldSize.x / 2;
            // + 1 senter hex

            for (int y = -halfSizeY; y <= halfSizeY; y++)
            {
                for (int x = -halfSizeX; x <= halfSizeX; x++)
                {
                    tempHex = PrefabUtility.InstantiatePrefab(hexPlatformPrefab) as GameObject;
                    tempHex.transform.SetParent(container.transform);

                    gridPosition = new Vector2Int(x, y);
                    
                    tempHex.name = $"Hex platform ({x},{y})";
                    tempHex.transform.position = CalculateWorldPosition(gridPosition);
                    tempHex.GetComponent<EditorPlatformBehaviour>().GridPosition = gridPosition;
                }
            }
        }

        public Vector3 CalculateWorldPosition(Vector2Int gridPosition)
        {
            if(gridPosition == Vector2Int.zero)
            {
                return Vector3.zero.SetY(yCoordinate); 
            }

            offset = 0;

            if (gridPosition.y % 2 != 0)
            {
                offset = HEX_WIDTH_HALF;
            }

            xCoordinate = gridPosition.x * HEX_WIDTH - offset;
            zCoordinate = gridPosition.y * HEX_HEIGHT;
            return new Vector3(xCoordinate, yCoordinate, zCoordinate);
        }

        public Vector2Int CalculateGridPosition(Vector3 worldPosition)
        {
            zCoordinate = worldPosition.z / HEX_HEIGHT;
            offset = 0;

            if(Mathf.RoundToInt(zCoordinate) % 2 != 0)
            {
                offset = HEX_WIDTH_HALF;
            }

            xCoordinate = (worldPosition.x + offset) / HEX_WIDTH;
            return new Vector2Int(Mathf.RoundToInt(xCoordinate), Mathf.RoundToInt(zCoordinate));
        }

        public void Clear()
        {
            for (int i = container.transform.childCount - 1; i >= 0; i--)
            {
                DestroyImmediate(container.transform.GetChild(i).gameObject);
            }
        }

        public void HandleMouseEvents()
        {
            currentEvent = Event.current;

            if (currentEvent != null)
            {

                if (Event.current.type == EventType.Layout || Event.current.type == EventType.Repaint)
                {
                    EditorUtility.SetDirty(this); // make sure to receive all mouse events
                    return;
                }


                if ((currentEvent.isMouse) && (currentEvent.button == 0))
                {
                    if (currentEvent.type == EventType.MouseDown)
                    {

                        fixedMousePosition = new Vector2(currentEvent.mousePosition.x, Screen.height - currentEvent.mousePosition.y);
                        ray = sceneCamera.ScreenPointToRay(fixedMousePosition);

                        if (Physics.Raycast(ray, out hitInfo))
                        {
                            editorPlatformBehaviour = hitInfo.collider.gameObject.GetComponent<EditorPlatformBehaviour>();

                            if ((editorPlatformBehaviour != null) && (!IsCentralHex(editorPlatformBehaviour.GridPosition)))
                            {
                                editorPlatformBehaviour.EditorHexType = selectedHexType;
                            }
                        }
                    }
                    else if (currentEvent.type == EventType.MouseDrag)
                    {
                        fixedMousePosition = new Vector2(currentEvent.mousePosition.x, Screen.height - currentEvent.mousePosition.y);
                        ray = sceneCamera.ScreenPointToRay(fixedMousePosition);

                        if (Physics.Raycast(ray, out hitInfo))
                        {
                            if (hitInfo.collider.gameObject != lastSelectedGameObject)
                            {
                                lastSelectedGameObject = hitInfo.collider.gameObject;

                                editorPlatformBehaviour = hitInfo.collider.gameObject.GetComponent<EditorPlatformBehaviour>();

                                if ((editorPlatformBehaviour != null) && (!IsCentralHex(editorPlatformBehaviour.GridPosition)))
                                {
                                    editorPlatformBehaviour.EditorHexType = selectedHexType;
                                }
                            }
                        }
                    }
                }
            }
        }

        private void OnGUI()
        {
            HandleMouseEvents();
        }

        public void LoadItems(ActiveHex[] activeHexes, Vector2Int newFieldSize, float newY)
        {
            fieldSize = newFieldSize;
            yCoordinate = newY;
           
            //mainLayerMaterial = material != null ? material : materialPrefab;

            Clear();
            CreateGrid();

            EditorPlatformBehaviour[] platforms = container.GetComponentsInChildren<EditorPlatformBehaviour>();

            for (int platformIndex = 0; platformIndex < platforms.Length; platformIndex++)
            {
                platforms[platformIndex].EditorHexType = HexType.Ground;

                for (int activeHexIndex = 0; activeHexIndex < activeHexes.Length; activeHexIndex++)
                {
                    if (IsCentralHex(platforms[platformIndex].GridPosition))
                    {
                        break;
                    }

                    if(platforms[platformIndex].GridPosition == activeHexes[activeHexIndex].gridPosition)
                    {
                        platforms[platformIndex].EditorHexType = activeHexes[activeHexIndex].type;
                        break;
                    }
                }
            }
        }

        public ActiveHex[] SaveItems()
        {
            EditorPlatformBehaviour[] platforms = container.GetComponentsInChildren<EditorPlatformBehaviour>();
            List<ActiveHex> activeHexes = new List<ActiveHex>();

            for (int i = 0; i < platforms.Length; i++)
            {
                activeHexes.Add(new ActiveHex(platforms[i].transform.position, platforms[i].GridPosition, platforms[i].EditorHexType));
            }

            return activeHexes.ToArray();
        }

        [Button]
        public void SpawnField()
        {
            Clear();
            CreateGrid();
        }
#endif
    }
}
