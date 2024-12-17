using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HexFall
{
    [System.Serializable]
    public struct AISettings
    {
        public int senceDepth;
        public float speed;
        public float thinkingDelay;
    }
}