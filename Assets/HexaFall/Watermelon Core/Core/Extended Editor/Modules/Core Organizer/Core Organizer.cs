using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace HexFall
{
    [CreateAssetMenu(fileName = "Core Organizer", menuName = "Games/HexFall/Tools/Core Organizer")]
    public class CoreOrganizer : ScriptableObject
    {
        [SerializeField] CoreFolderData[] coreSettings;
    }


    [System.Serializable]
    public class CoreFolderData
    {
        [SerializeField] string name;
        [SerializeField] string directory;
        [SerializeField] string status;
        [SerializeField] bool include;
    }

}