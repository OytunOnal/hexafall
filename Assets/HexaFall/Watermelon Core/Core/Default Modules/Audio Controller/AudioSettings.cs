using UnityEngine;

namespace HexFall
{
    [CreateAssetMenu(fileName = "Audio Settings", menuName = "Games/HexFall/Settings/Audio Settings")]
    public class AudioSettings : ScriptableObject
    {
        [SerializeField] Music music;
        public Music Music => music;

        [SerializeField] Sounds sounds;
        public Sounds Sounds => sounds;
    }
}

// -----------------
// Audio Controller v 0.4
// -----------------