using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace HexFall
{
    public class HexMap : MonoBehaviour
    {
        private static HexMap instance;

        public const float HEX_WIDTH = 1.02f;
        public const float HEX_WIDTH_HALF = 0.51f;
        public const float HEX_HEIGHT = 0.88f;
        public const float HEX_HEIGHT_HALF = 0.44f;

        public static Vector2Int[] NeighbourCoordsEven = new Vector2Int[]
        {
            new Vector2Int(1, 0), new Vector2Int(0, -1), new Vector2Int(-1, -1),
            new Vector2Int(-1, 0), new Vector2Int(-1, 1), new Vector2Int(0, 1),
        };
        public static Vector2Int[] NeighbourCoordsOdd = new Vector2Int[]
        {
            new Vector2Int(1, 0), new Vector2Int(+1, -1), new Vector2Int(0, -1),
            new Vector2Int(-1, 0), new Vector2Int(0, 1), new Vector2Int(1, 1),
        };

        public static List<MapLayer> MapLayersList { get; private set; }


        private void Awake()
        {
            if (instance != null)
            {
                Destroy(this);
                return;
            }

            instance = this;
            MapLayersList = new List<MapLayer>();
        }

        private void OnDestroy()
        {
            instance = null;
        }
        public static void InitLayers(MapLayer mapLayer)
        {
            if (!MapLayersList.Contains(mapLayer))
            {
                MapLayersList.Add(mapLayer);
            }
        }

        public static MapLayer GetLayer(Vector3 objectPosition)
        {
            for (int i = 0; i < MapLayersList.Count; i++)
            {
                if (objectPosition.y >= (MapLayersList[i].OffsetY - 0.2f))
                {
                    //Debug.Log("<color><b>Layer was found</b></color> = " + mapLayersList[i].OffsetY);
                    return MapLayersList[i];
                }
            }

            var lastLayer = MapLayersList[MapLayersList.Count - 1];

            if (objectPosition.y < MapLayersList[MapLayersList.Count - 1].OffsetY)
            {
                //Debug.Log("Entity was died and returns ladt layer");
                return lastLayer;
            }

            return lastLayer; // By default           
        }

        public static Hex GetRandomActiveHex(int layerIndex)
        {
            if (MapLayersList == null || MapLayersList.Count == 0)
            {
                //Debug.LogWarning("Layers weren't inited");
                return null;
            }

            if (layerIndex < MapLayersList.Count)
            {
                MapLayer mapLayer = MapLayersList[layerIndex];

                Hex hex = GetRandomActiveHexOnIsland(mapLayer);
                
                if (hex != null)
                {
                    return hex;
                }

                List<Hex> hiddenHexes = new List<Hex>();

                foreach (Vector2Int key in mapLayer.Map.Keys)
                {
                    hex = mapLayer.Map[key];
                    
                    if (hex.HexType == HexType.Hidden)
                    {
                        hiddenHexes.Add(hex);                       
                    }
                }

                if (hiddenHexes.Count > 0)
                {
                    hex = hiddenHexes.GetRandomItem();
                    hex.ChangeHexState(HexState.Active, true);
                }
            }
            else
            {
               // Debug.LogWarning("There is no layer with current index");
                return null;
            }


            return null;
        }

        public static Hex WorldPosToHexStatic(MapLayer mapLayer, Vector3 worldPos)
        {
            return WorldPosToHex(mapLayer, worldPos);
        }

        public static Hex WorldPosToHex(MapLayer mapLayer, Vector3 worldPos)
        {
            var centerHex = GetHex(mapLayer, GetApproximateHexCoords(worldPos));

            if (centerHex == null)
            {
                return null;
            }

            var hexPos = centerHex.GetWorldCoords();

            var closestHex = centerHex;
            var closestDistance = (hexPos - worldPos).sqrMagnitude;

            closestHex.ForEachNeighbour((neighbour) =>
            {
                var distance = (neighbour.GetWorldCoords() - worldPos).sqrMagnitude;

                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestHex = neighbour;
                }
            });

            return closestHex;
        }

        private static Hex GetHex(MapLayer mapLayer, Vector2Int index)
        {
            int listIndex = MapLayersList.IndexOf(mapLayer);
            if (listIndex == -1)
            {
                //Debug.Log("There is no hex in the map");
                return null;
            }

            return MapLayersList[listIndex].GetHex(index);// Map[index];
        }

        private static Vector2Int GetApproximateHexCoords(Vector3 worldPos)
        {
            var z = worldPos.z / HEX_HEIGHT;
            float offset = 0;

            if (Mathf.RoundToInt(z) % 2 != 0)
            {
                offset = HEX_WIDTH_HALF;
            }

            var x = (worldPos.x + offset) / HEX_WIDTH;
            return new Vector2Int(Mathf.RoundToInt(x), Mathf.RoundToInt(z));
        }

        public static Hex GetHexStatic(MapLayer mapLayer, Vector2Int index)
        {
            return GetHex(mapLayer, index);
        }

        public static Hex GetExistingHex(MapLayer mapLayer, Vector2Int index)
        {
            int listIndex = MapLayersList.IndexOf(mapLayer);

            if (listIndex == -1)
            {
                //Debug.Log("There is no hex in the map");
                return null;
            }

            return MapLayersList[listIndex].GetHex(index);// Map[index];
        }

        public static Hex GetRandomActiveHexOnIsland(MapLayer mapLayer)
        {
            List<Hex> result = new List<Hex>();
            List<Hex> activeHexes = new List<Hex>();

            int layerIndex = MapLayersList.IndexOf(mapLayer);

            if (layerIndex == -1)
                return null;

            var layer = MapLayersList[layerIndex];

            foreach (var gridindex in layer.Map.Keys)
            {
                var hex = GetExistingHex(layer, gridindex);

                if (hex.State == HexState.Active)
                {
                    activeHexes.Add(hex);

                    if (hex.HasBrick())
                        result.Add(hex);
                }
            }

            if (result.IsNullOrEmpty())
            {
                if(activeHexes.IsNullOrEmpty())
                {
                    return null;
                }
                else
                {
                    return activeHexes.GetRandomItem();
                }
            }
            else
            {
                return result.GetRandomItem();
            }

        }

        public static List<Hex> GetHexesWithBricks(MapLayer mapLayer)
        {
            List<Hex> result = new List<Hex>();

            int layerIndex = MapLayersList.IndexOf(mapLayer);

            if (layerIndex == -1)
                return result;

            var layer = MapLayersList[layerIndex];

            foreach (var gridindex in layer.Map.Keys)
            {
                var hex = GetExistingHex(layer, gridindex);

                if (hex.State == HexState.Active && hex.HasBrick())
                {
                    result.Add(hex);
                }
            }

            return result;
        }    

        public static void Clear()
        {
            for (int i = 0; i < MapLayersList.Count; i++)
            {
                MapLayersList[i].Clear();
            }

            MapLayersList = new List<MapLayer>();
        }

        public delegate void HexDelegate(Hex hex);
    }
}