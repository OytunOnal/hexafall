using System;
using UnityEngine;

namespace HexFall
{

    public abstract class BaseCharacterBehaviour : MonoBehaviour
    {
        public readonly float CHARACTER_HEIGHT = 2.4f;

        [SerializeField]
        protected CharacterGroundCheck characterGroundCheck;
        [SerializeField]
        protected CharacterAutoJump characterAutoJump;

        [SerializeField]
        protected Transform playerGraphics;

        [SerializeField]
        protected ProductSetuper productSetuper;

        [Space]
        [SerializeField]
        protected float movementSpeed = 5;
        [SerializeField]
        protected float rotateSpeed = 5;
        [SerializeField]
        protected Vector3 jumpForce;

        [Space]
        [SerializeField] protected BackpackBehavior backpack;
        [SerializeField] protected int minBricksAmountAfterRevive;

        protected Rigidbody playerRigidbody;
        protected Animator playerAnimatorController;

        protected readonly int ANIMATOR_PARAM_RUN_HASH = Animator.StringToHash("Run");
        protected readonly int ANIMATOR_PARAM_MOVEMENT_HASH = Animator.StringToHash("Movement");
        protected readonly int ANIMATOR_PARAM_IS_GROUNDED_HASH = Animator.StringToHash("isGrounded");
        protected readonly int ANIMATOR_PARAM_JUMP_HASH = Animator.StringToHash("Jump");
        protected readonly int ANIMATOR_PARAM_FALL_HASH = Animator.StringToHash("Fall");

        public Hex ActiveHex { get; private set; }

        public MapLayer CurrentLayer => HexMap.GetLayer(characterGroundCheck.transform.position);
        public BackpackBehavior Backpack => backpack;
        public bool IsGrounded
        {
            get
            {
                return (playerRigidbody.linearVelocity.y < 1f && playerRigidbody.linearVelocity.y > -1f);
            }
        }

        public bool IsDied { get; protected set; }

        protected bool IsKinematic
        {
            get => playerRigidbody.isKinematic;

            set
            {
                playerRigidbody.collisionDetectionMode = CollisionDetectionMode.ContinuousSpeculative;
                playerRigidbody.isKinematic = value;
            }
        }

        protected virtual void Awake()
        {
            playerRigidbody = GetComponent<Rigidbody>();

            playerAnimatorController = playerGraphics.GetComponent<Animator>();

            characterGroundCheck.Init(this);
            characterAutoJump.Init(this);
        }

        public virtual void SteppedOn(Hex hex)
        {
            ActiveHex = hex;
        }

        protected virtual void Init()
        {
            backpack.ClearBackpack();
            characterGroundCheck.Clear();
            OnCharacterGrounded();

            playerGraphics.localRotation = Quaternion.Euler(0f, 0f, 0f); // Reset graphic rotation

            IsDied = false;
        }

        protected virtual void Update()
        {

        }

        protected virtual void FixedUpdate()
        {
            if (IsDied)
                return;

            if (transform.position.y < LevelController.DisableHeight)
            {
                if (!IsDied)
                    OnCharacterDied();
            }

        }

        protected virtual void OnTriggerEnter(Collider collider)
        {

        }

        protected virtual void OnTriggerExit(Collider collider)
        {

        }

        protected void Jump()
        {
            if (!characterGroundCheck.IsCharacterGrounded)
                return;

            playerRigidbody.linearVelocity = Vector3.zero;
            playerRigidbody.AddForce(jumpForce, ForceMode.Force);
            playerAnimatorController.SetTrigger(ANIMATOR_PARAM_JUMP_HASH);
        }

        public virtual void OnCharacterSkinSelected(SkinTab tab, SkinData product)
        {
            productSetuper.ApplyProduct(tab, product.Prefab);
        }

        public virtual void OnCharacterGrounded()
        {
            playerAnimatorController.SetBool(ANIMATOR_PARAM_IS_GROUNDED_HASH, true);
        }

        public virtual void OnCharacterFall()
        {
            playerAnimatorController.SetBool(ANIMATOR_PARAM_FALL_HASH, false);
        }

        public virtual void OnAutoJump()
        {
            Jump();
        }

        protected virtual void OnCharacterDied()
        {
            if (IsDied)
            {
                return;
            }

            IsDied = true;
            IsKinematic = true;

            LevelController.UpdatePlayersAmount(-1);
        }

        protected virtual void OnRevived()
        {
            IsKinematic = false;

            IsDied = false;
        }

        public virtual void AddBrick(BrickBehavior brick)
        {
            backpack.PickUpBrick(brick);
        }

        public bool HasBrick()
        {
            return backpack.HasBricks();
        }

        public void RemoveBrick(Vector3 targetPlatformPosition, Quaternion rotation, bool immediately = false, Action onCompleted = null)
        {
            backpack.RemoveBrick(targetPlatformPosition, rotation, immediately, onCompleted);
        }
    }
}