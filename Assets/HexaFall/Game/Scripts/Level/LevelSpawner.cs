using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace HexFall
{
    public class LevelSpawner
    {
        public void LoadLevel(Level level)
        {
            List<Layer> layers = new List<Layer>();
            layers.Add(LevelController.SpawnLayer);
            layers.AddRange(level.layers);
            ColorsPreset colorPreset = ColorsDatabase.GetPresetForLevel(GameController.LevelIndex);

            for (int i = 0; i < layers.Count; i++)
            {
                Layer layer = layers[i];
                Island island = layer.island;
                LayerColors layerColors = colorPreset.GetColorsForLayer(i == 0 ? (layers.Count - 1) : i);

                Dictionary<Vector2Int, Hex> levelSpawnerData = new Dictionary<Vector2Int, Hex>();

                for (int hexSettingsIndex = 0; hexSettingsIndex < island.activeHexes.Length; hexSettingsIndex++)
                {
                    var hexLevelData = island.activeHexes[hexSettingsIndex];

                    ActiveHex newActiveHex = new ActiveHex(hexLevelData.position, hexLevelData.gridPosition, hexLevelData.type);

                    PlatformBehaviour platform = PoolHandler.GetPlatform();

                    // RESET
                    Transform platformTransform = platform.transform;
                    platformTransform.localScale = Vector3.one;
                    platformTransform.eulerAngles = Vector3.zero;
                    platformTransform.position = Vector3.zero;

                    platform.SetVisual(newActiveHex.type.Equals(HexType.Border) ? layerColors.BorderColor : layerColors.LayerColor);

                    Hex hex = new Hex(newActiveHex, platform);

                    float random = Random.Range(0f, 1f);

                    if (newActiveHex.type.Equals(HexType.Ground) && layer.offsetY != 7) // && layer.offsetY != 7 - FIRST SINGLE HEXES
                    {
                        if (random < (LevelController.ChanceOfSingleBrickSpawn + LevelController.ChanceOfMultyBrickSpawn))
                        {
                            BrickBehavior brick = PoolHandler.GetBrick();

                            // RESET
                            Transform brickTransform = brick.transform;

                            brickTransform.localScale = Vector3.one;
                            brickTransform.eulerAngles = Vector3.zero;
                            brickTransform.position = Vector3.zero;

                            hex.SetBrick(brick);
                            brick.Init(random < LevelController.ChanceOfMultyBrickSpawn);
                        }

                    }

                    levelSpawnerData.Add(newActiveHex.gridPosition, hex);

                }

                MapLayer mapLayer = new MapLayer(levelSpawnerData, layer.offsetY);

                HexMap.InitLayers(mapLayer);
            }
        }
    }
}
