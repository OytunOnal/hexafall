using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace HexFall
{

        public class EnemyBehaviour : BaseCharacterBehaviour
        {
            [Header("Enemy")]
            [SerializeField] AISettings aiSettings;
            [SerializeField] TMP_Text nicknameText;

            [Space]
            [SerializeField] float rotationSpeed = 500;

            private List<Hex> path;
            private int weight;
            private int currentIndex = -1;

            private bool isWaiting = true;

            public SimpleCallback OnDied;
            private MapLayer lastLayer = null;
            private Transform nicknameTransform;


            public int NextIslandGoalId { get; set; }

            protected override void Awake()
            {
                base.Awake();
                nicknameTransform = nicknameText.gameObject.transform;
            }

            public void Init(string nickname)
            {
                base.Init();

                transform.eulerAngles = Vector3.zero.SetY(180f);
                IsKinematic = false;

                if (GameController.UseNicknames)
                {
                    nicknameText.gameObject.SetActive(true);
                    nicknameText.text = nickname;
                    nicknameTransform.eulerAngles = CameraBehavior.EulerRotation;
                }
                else
                {
                    nicknameText.gameObject.SetActive(false);
                }

                OnCharacterSkinSelected(SkinTab.Color, SkinStoreController.GetRandomProduct(SkinTab.Color));
                OnCharacterSkinSelected(SkinTab.Hat, SkinStoreController.GetRandomProduct(SkinTab.Hat));
            }

            protected override void FixedUpdate()
            {
                base.FixedUpdate();

                if (!LevelController.IsGameplayActive || IsDied)
                    return;

                if (currentIndex == -1 || path.IsNullOrEmpty() || (lastLayer != null && !lastLayer.Equals(CurrentLayer)))
                {
                    RecalculatePath();
                }

                if (isWaiting)
                    return;

                Vector3 targetPosition = path[currentIndex].GetWorldCoords(); //devTarget.transform.position;

                if (Vector3.Distance(targetPosition, transform.position) > 0.05f)
                {
                    playerAnimatorController.SetBool(ANIMATOR_PARAM_MOVEMENT_HASH, true);
                    playerAnimatorController.SetFloat(ANIMATOR_PARAM_RUN_HASH, 1.0f);

                    Vector3 position = Vector3.MoveTowards(transform.position, targetPosition, aiSettings.speed * Time.fixedDeltaTime);
                    playerRigidbody.MovePosition(position);

                    var rotation = Quaternion.LookRotation(targetPosition - playerGraphics.position);
                    var resultRotation = Quaternion.Slerp(playerGraphics.rotation, rotation, Time.fixedDeltaTime * rotationSpeed);

                    playerGraphics.rotation = Quaternion.Euler(0f, resultRotation.eulerAngles.y, 0f);
                }
                else
                {
                    playerAnimatorController.SetBool(ANIMATOR_PARAM_MOVEMENT_HASH, false);

                    currentIndex++;

                    if (currentIndex >= path.Count)
                    {
                        currentIndex = -1;
                        RecalculatePath();
                    }
                }

                if (GameController.UseNicknames)
                {
                    nicknameTransform.eulerAngles = CameraBehavior.EulerRotation;
                }
            }

            public override void OnCharacterGrounded()
            {
                base.OnCharacterGrounded();
            }

            public void RecalculatePath()
            {
                lastLayer = CurrentLayer;
                var bricksAvailable = backpack.GetBricksAmount();

                isWaiting = true;
                //Debug.Log($"<color=#99D1CC><b>[EnemyBehaviour] => RecalculatePath</b></color>");

                if (bricksAvailable > 5)
                {
                    // Todo Try getting to another island

                    var hex = HexMap.GetRandomActiveHexOnIsland(CurrentLayer);
                    //Debug.Log("next hex: " + hex.GridHexCoords);

                    if (hex == null)
                    {
                        CalculatePathForTheBrick(bricksAvailable);
                    }
                    else
                    {
                        path = AIController.FindRouteToHex(HexMap.WorldPosToHexStatic(CurrentLayer, transform.position), hex, bricksAvailable);

                        if (path == null)
                        {
                            CalculatePathForTheBrick(bricksAvailable);
                        }
                        else
                        {
                            Tween.DelayedCall(aiSettings.thinkingDelay, () => isWaiting = false);

                            currentIndex = 0;
                        }
                    }
                }
                else
                {
                    CalculatePathForTheBrick(bricksAvailable);
                }
            }

            public override void SteppedOn(Hex hex)
            {
                base.SteppedOn(hex);
            }

            private void CalculatePathForTheBrick(int bricksAvailable)
            {
                var hexesWithBricks = HexMap.GetHexesWithBricks(CurrentLayer);

                if (!hexesWithBricks.IsNullOrEmpty())
                {
                    var target = hexesWithBricks.GetRandomItem();

                    path = AIController.FindRouteToHex(HexMap.WorldPosToHexStatic(CurrentLayer, transform.position), target, bricksAvailable);

                    Tween.DelayedCall(aiSettings.thinkingDelay, () => isWaiting = false);

                    currentIndex = 0;
                }
            }

            protected override void OnCharacterDied()
            {
                base.OnCharacterDied();


                AIController.OnEnemyDied();

                ReturnToPool();
            }

            private void ReturnToPool()
            {
                if (!playerRigidbody.isKinematic)
                    playerRigidbody.linearVelocity = Vector3.zero;

                gameObject.SetActive(false);
            }
        }
    
}