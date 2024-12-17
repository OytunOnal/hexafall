using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ColorsPreset
{
    [SerializeField] List<LayerColors> layerColors = new List<LayerColors>();
    public List<LayerColors> LayerColors => LayerColors;

    public LayerColors GetColorsForLayer(int layerIndex)
    {
        return layerColors[layerIndex % layerColors.Count];
    }
}