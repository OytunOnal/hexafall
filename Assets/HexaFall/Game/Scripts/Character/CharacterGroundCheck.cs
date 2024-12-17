using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace HexFall
{
    using static PlatformBehaviour;

    [RequireComponent(typeof(Collider))]
    [RequireComponent(typeof(Rigidbody))]
    public class CharacterGroundCheck : MonoBehaviour
    {
        private const string PLATFORM_TAG = "Platform";

        private const float AUTO_KILL_PLATFORM_DELAY = 1.5F;
        private const float PERMISSION_TO_STAY_ON_PLATFORM_TIME = 3f;

        private bool isCharacterGrounded;
        public bool IsCharacterGrounded
        {
            get { return isCharacterGrounded; }
        }

        public Hex CurrentHex { get; private set; }
        public Dictionary<Hex, float> steppedPlatformsDictionary = new Dictionary<Hex, float>();

        private BaseCharacterBehaviour characterController;

        private void Start()
        {

        }

        public void Init(BaseCharacterBehaviour characterController)
        {
            this.characterController = characterController;
        }

        public void Clear()
        {
            steppedPlatformsDictionary = new Dictionary<Hex, float>();

            CurrentHex = null;
        }

        private void Update()
        {
            if (!LevelController.IsGameplayActive)
                return;

            if (characterController.IsDied)
            {
                return;
            }

            if (!characterController.IsGrounded && !characterController.HasBrick())
            {
                if (characterController.GetComponent<PlayerBehaviour>())
                return;
            }

            var hex = HexMap.WorldPosToHexStatic(characterController.CurrentLayer, transform.position);

            if (hex == null)
            {
                return;
            }

            bool changedHex = CurrentHex != hex;

            if (changedHex)
            {
                if (CurrentHex != null)
                {
                    CurrentHex.SteppedOff(characterController, isGraphicStepOff: true);
                }

                CurrentHex = hex;

                CurrentHex.SteppedOn(characterController);
            }

            isCharacterGrounded = CurrentHex.State != HexState.Disabled;

            if (changedHex)
            {
                if (isCharacterGrounded)
                {
                    characterController.OnCharacterGrounded();
                }
                else
                {
                    characterController.OnCharacterFall();
                }
            }

            // Update stepped dictionary platform
            float timer = Time.unscaledTime + AUTO_KILL_PLATFORM_DELAY;

            if (!steppedPlatformsDictionary.ContainsKey(CurrentHex))
            {
                steppedPlatformsDictionary.Add(CurrentHex, timer);
            }

            if (steppedPlatformsDictionary != null && steppedPlatformsDictionary.Count > 0)
            {
                foreach (Hex hexKey in steppedPlatformsDictionary.Keys)
                {
                    if (steppedPlatformsDictionary[hexKey] <= Time.unscaledTime)
                    {
                        //if (Time.time -hexKey.lastTimeStepped > 1.5f )
                        {
                            hexKey.SteppedOff(characterController);
                            steppedPlatformsDictionary.Remove(hexKey);
                        }
                        break;
                    }
                }
            }
        }
    }
}