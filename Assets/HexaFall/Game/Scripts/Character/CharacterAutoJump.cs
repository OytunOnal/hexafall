using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HexFall
{
    [RequireComponent(typeof(Collider))]
    [RequireComponent(typeof(Rigidbody))]
    public class CharacterAutoJump : MonoBehaviour
    {
        private const string GROUND_TAG = "Ground";
        private const string PLATFORM_TAG = "Platform";

        private bool isGroundInFront;
        public bool IsGroundInFront
        {
            get { return isGroundInFront; }
        }

        private List<Collider> groundColliders = new List<Collider>();

        private BaseCharacterBehaviour characterController;

        public void Init(BaseCharacterBehaviour characterController)
        {
            this.characterController = characterController;
        }

        private void OnTriggerEnter(Collider collision)
        {
            if (collision.CompareTag(GROUND_TAG) || collision.CompareTag(PLATFORM_TAG))
            {
                if (groundColliders.FindIndex(x => x == collision) == -1)
                {
                    groundColliders.Add(collision);

                    isGroundInFront = true;
                }
            }
        }
    }
}