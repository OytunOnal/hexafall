using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HexFall
{

    [CreateAssetMenu(fileName = "Levels Database", menuName = "Games/HexFall/Content/Levels Database")]
    public class LevelsDatabase : ScriptableObject
    {
        public Level[] levels;
    }
}