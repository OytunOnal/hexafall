using UnityEngine;

namespace HexFall
{
    public abstract class FloatingTextBaseBehaviour : MonoBehaviour
    {
        public abstract void Activate(string text, float scale = 1.0f);
    }
}