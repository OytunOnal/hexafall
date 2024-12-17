using UnityEngine;
using TMPro;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace HexFall
{
    public class PlayerBehaviour : BaseCharacterBehaviour
    {
        [SerializeField] Joystick joystick;
        [SerializeField] TMP_Text nicknameText;

        private static PlayerBehaviour instance;

        private Transform nicknameTransform;
        private Vector3 lastPosition;
        private float lastTimePositionChanged;

        private bool isInputActive;

        public static Vector3 Position => instance.transform.position;

        public static readonly string NICK_SAVE_HASH = "player_nick";
        private SimpleStringSave playerNickSave;

        protected override void Awake()
        {
            base.Awake();
            instance = this;
            nicknameTransform = nicknameText.gameObject.transform;
        }

        private void Start()
        {
            playerNickSave = SaveController.GetSaveObject<SimpleStringSave>(NICK_SAVE_HASH);
        }

        private void OnEnable()
        {
            SkinStoreController.OnProductSelected += OnCharacterSkinSelected;
        }

        private void OnDisable()
        {
            SkinStoreController.OnProductSelected -= OnCharacterSkinSelected;
        }

        public static void Init(Vector3 position)
        {
            instance.Init();

            instance.transform.position = position;
            instance.transform.eulerAngles = Vector3.zero.SetY(180f);

            instance.IsKinematic = false;
            instance.EnableInput();

            CameraBehavior.Target = instance.transform;

            instance.OnCharacterSkinSelected(SkinTab.Color, SkinStoreController.GetRandomSkin());
            instance.OnCharacterSkinSelected(SkinTab.Hat, SkinStoreController.GetRandomHat());

            if (GameController.UseNicknames)
            {
                instance.nicknameText.gameObject.SetActive(true);
                InitNickname();
            }
            else
            {
                instance.nicknameText.gameObject.SetActive(false);
            }
        }

        public static void InitNickname()
        {
            instance.nicknameText.text = "You";
            instance.nicknameTransform.eulerAngles = CameraBehavior.EulerRotation;
        }

        protected override void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Jump();
            }
        }

        protected override void FixedUpdate()
        {
            base.FixedUpdate();

            if (!isInputActive)
                return;

            Vector2 joystickPosition = joystick.Input;
            if (joystickPosition.magnitude > 0.2f)
            {
                playerAnimatorController.SetBool(ANIMATOR_PARAM_MOVEMENT_HASH, true);
                playerAnimatorController.SetFloat(ANIMATOR_PARAM_RUN_HASH, joystickPosition.magnitude);

                playerRigidbody.MovePosition(transform.position + new Vector3(joystickPosition.x, 0, joystickPosition.y) * movementSpeed * Time.fixedDeltaTime);

                playerGraphics.eulerAngles = new Vector3(0, Mathf.Atan2(joystickPosition.x, joystickPosition.y) * 180 / Mathf.PI, 0);
            }
            else
            {
                playerAnimatorController.SetBool(ANIMATOR_PARAM_MOVEMENT_HASH, false);
            }

            // do not allow to stay on the edge of hex
            if (!LevelController.IsGameplayActive)
                return;

            if (lastPosition != transform.position)
            {
                lastTimePositionChanged = Time.timeSinceLevelLoad;
            }

            lastPosition = transform.position;

            if (Time.timeSinceLevelLoad - lastTimePositionChanged > 2f)
            {
                Collider[] colls = Physics.OverlapSphere(transform.position, 0.5f);

                for (int i = 0; i < colls.Length; i++)
                {
                    if (colls[i].CompareTag("Platform"))
                    {
                        colls[i].GetComponent<PlatformBehaviour>().SetDisabledState(true);
                    }
                }

                lastTimePositionChanged = Time.timeSinceLevelLoad;
            }
        }

        public bool HasStayedLong()
        {
            if (Time.timeSinceLevelLoad - lastTimePositionChanged > 2f)
                return true;
            else return false;
        }

        protected override void OnCharacterDied()
        {
            base.OnCharacterDied();

            LevelController.OnPlayerDied();
            instance.DisableInput();
            playerAnimatorController.SetBool(ANIMATOR_PARAM_MOVEMENT_HASH, false);
        }

        public static void PlayWinAnimation()
        {
            instance.DisableInput();
            instance.IsKinematic = true;
            instance.transform.eulerAngles = new Vector3(0f, 180f, 0f);
            instance.playerAnimatorController.SetBool(instance.ANIMATOR_PARAM_MOVEMENT_HASH, false);
        }

        public static void DisableJoystick()
        {
            instance.DisableInput();
        }

        public static void OnRevived(Vector3 revivePosition)
        {
            instance.OnRevived();
            instance.EnableInput();

            instance.transform.position = revivePosition;

            if (instance.Backpack.GetBricksAmount() < instance.minBricksAmountAfterRevive)
            {
                instance.Backpack.SetBricks(instance.minBricksAmountAfterRevive - instance.Backpack.GetBricksAmount());
            }
        }

        public override void AddBrick(BrickBehavior brick)
        {
            base.AddBrick(brick);

            AudioController.PlaySound(AudioController.Sounds.brickSound, 0.5f);
        }

        private void EnableInput()
        {
            isInputActive = true;
        }

        private void DisableInput()
        {
            isInputActive = false;
        }

#if UNITY_EDITOR

        [Button("Find Skin Holder")]
        private void FindSkinsHolder()
        {
            Selection.activeGameObject = GameObject.Find("mixamorig:HeadTop_End");
        }
#endif
    }
}