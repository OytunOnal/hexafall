using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HexFall
{
    [System.Serializable]
    public class Level : ScriptableObject
    {
        public Layer[] layers;
        public Vector2Int fieldSize;
        public Vector2Int[] enemyPositions;

        public float GetLevelHeight()
        {
            float height = 0;
            for (int layerIndex = 0; layerIndex < layers.Length; layerIndex++)
            {
                height += layers[layerIndex].offsetY;
            }
            return height;
        }
    }

    [System.Serializable]
    public enum HexType
    {
        Ground = 0,
        Border = 1,
        Hidden = 2,
    }

    
}