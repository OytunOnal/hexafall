using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HexFall
{
    public class MapLayer
    {
        public Dictionary<Vector2Int, Hex> Map { get; private set; }
        public float OffsetY { get; private set; }

        public MapLayer(Dictionary<Vector2Int, Hex> map, float offsetY)
        {
            if (Map == null) Map = new Dictionary<Vector2Int, Hex>();

            OffsetY = offsetY;

            foreach (var key in map.Keys)
            {
                Map.Add(key, map[key]);
                Map[key].SetMap(this);
            }          
        }

        public Hex GetHex(Vector2Int index)
        {
            if (!Map.ContainsKey(index))
            {
                //Debug.Log("There is no hex in the map");
                return null;
            }

            return Map[index];
        }

        public void Clear()
        {
            Map = new Dictionary<Vector2Int, Hex>();
        }

        public bool Equals(MapLayer otherMap)
        {
            //Check for null and compare run-time types.
            if ((otherMap == null) || !this.GetType().Equals(otherMap.GetType()))
            {
                return false;
            }
            else
            {
                return (otherMap.OffsetY == OffsetY);
            }
        }
    }
}
