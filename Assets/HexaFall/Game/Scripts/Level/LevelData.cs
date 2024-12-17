using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LevelData
{
    [SerializeField] int mapSizeX;
    [SerializeField] int mapSizeZ;

    [SerializeField] int layersAmount;
    [SerializeField] int layersHeightOffset;

    [SerializeField] float hexWidth;
    [SerializeField] float hexLength;
    [SerializeField] float hexOffset;

    public int MapSizeX => mapSizeX;
    public int MapSizeZ => mapSizeZ;

    public int LayersAmount => layersAmount;
    public int LayersHeightOffset => layersHeightOffset;

    public float HexWidth => hexWidth;
    public float HexLength => hexLength;
    public float HexOffset => hexOffset;
}
