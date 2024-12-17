using UnityEngine;

namespace HexFall
{
    [System.Serializable]
    public class ActiveHex
    {
        public Vector3 position;
        public Vector2Int gridPosition;
        public HexType type;

        public ActiveHex()
        {
        }

        public ActiveHex(Vector2Int gridPosition, HexType type)
        {
            this.gridPosition = gridPosition;
            this.type = type;
        }

        public ActiveHex(Vector3 position, Vector2Int gridPosition, HexType type)
        {
            this.position = position;
            this.gridPosition = gridPosition;
            this.type = type;
        }
    }    
}