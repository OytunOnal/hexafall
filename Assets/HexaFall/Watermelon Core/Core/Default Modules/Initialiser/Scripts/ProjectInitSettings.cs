#pragma warning disable 0649

using UnityEngine;

namespace HexFall
{
    [CreateAssetMenu(fileName = "Project Init Settings", menuName = "Games/HexFall/Settings/Project Init Settings")]
    [HelpURL("https://docs.google.com/document/d/1ORNWkFMZ5_Cc-BUgu9Ds1DjMjR4ozMCyr6p_GGdyCZk")]
    public class ProjectInitSettings : ScriptableObject
    {
        [SerializeField] InitModule[] coreModules;
        public InitModule[] CoreModules => coreModules;

        [SerializeField] InitModule[] initModules;
        public InitModule[] InitModules => initModules;

        public void Initialise(Initialiser initialiser)
        {
            for (int i = 0; i < coreModules.Length; i++)
            {
                coreModules[i].CreateComponent(initialiser);
            }

            for (int i = 0; i < initModules.Length; i++)
            {
                initModules[i].CreateComponent(initialiser);
            }
        }

        public void StartInit(Initialiser initialiser)
        {
            for (int i = 0; i < coreModules.Length; i++)
            {
                coreModules[i].StartInit(initialiser);
            }

            for (int i = 0; i < initModules.Length; i++)
            {
                initModules[i].StartInit(initialiser);
            }
        }
    }
}

// -----------------
// Initialiser v 0.4.2
// -----------------