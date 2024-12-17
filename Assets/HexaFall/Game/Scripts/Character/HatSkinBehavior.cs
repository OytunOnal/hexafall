using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HexFall
{
    public class HatSkinBehavior : MonoBehaviour
    {
        [SerializeField] Vector3 modelPosition;
        public Vector3 ModelPosition => modelPosition;
    }
}