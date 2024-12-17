using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HexFall
{
    public class CameraBehavior : MonoBehaviour
    {
        [SerializeField] Vector3 positionOffset;
        [SerializeField] Vector3 rotationOffset;

        public static Transform Target { get; set; }
        public static Vector3 EulerRotation => instance != null ? instance.transform.eulerAngles : Vector3.zero;

        private static CameraBehavior instance;

        private void Awake()
        {
            instance = this;
        }

        private void LateUpdate()
        {
            if (Target != null)
            {
                var newPos = Target.position + positionOffset;

                transform.position = newPos;
            }
        }
    }
}