using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LayerColors
{
    [SerializeField] Color layerColor = Color.white;
    [SerializeField] Color borderColor = new Color(0.62f, 0.62f, 0.62f, 1f);

    public Color LayerColor => layerColor;
    public Color BorderColor => borderColor;
}