using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HexFall
{

    [CreateAssetMenu(fileName = "Colors Database", menuName = "Games/HexFall/Content/Colors Database")]
    public class ColorsDatabase : ScriptableObject
    {
        [SerializeField] List<ColorsPreset> colorPresets = new List<ColorsPreset>();

        private static ColorsDatabase instance;

        public void Init()
        {
            instance = this;
        }

        public static ColorsPreset GetPresetForLevel(int levelNumber)
        {
            return instance.colorPresets[levelNumber % instance.colorPresets.Count];
        }
    }
}