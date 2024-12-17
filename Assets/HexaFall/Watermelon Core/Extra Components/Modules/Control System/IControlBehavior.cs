using UnityEngine;

namespace HexFall
{
    public interface IControlBehavior
    {
        public Vector3 FormatedInput { get; }
        public bool IsInputActive { get; }

        public void EnableControl();
        public void DisableControl();
        public void ResetControl();

        public event SimpleCallback OnInputActivated;
    }
}