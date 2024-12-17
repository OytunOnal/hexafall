using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HexFall
{
    public class ColorSkinBehavior : MonoBehaviour
    {
        [SerializeField] Material skinMaterial;
        public Material Material => skinMaterial;
    }
}