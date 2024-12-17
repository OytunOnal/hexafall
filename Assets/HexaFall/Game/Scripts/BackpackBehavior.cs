using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HexFall
{
    public class BackpackBehavior : MonoBehaviour
    {
        private List<BrickBehavior> bricks;

        [SerializeField] AnimationCurve brickMovesOutCurve;
        [SerializeField] AnimationCurve brickMovesInCurve;

        private void Awake()
        {
            bricks = new List<BrickBehavior>();
        }

        public void PickUpBrick(BrickBehavior brick)
        {
            var brickLocalPos = Vector3.up * bricks.Count * 0.55f;

            brick.GetPickedUp();
            StartCoroutine(BrickmovementCoroutine(brick, brickLocalPos));
        }

        private IEnumerator BrickmovementCoroutine(BrickBehavior brick, Vector3 localPos)
        {
            Vector3 worldPos;
            float time = 0;

            do
            {
                yield return null;

                time += Time.deltaTime;

                worldPos = transform.position + transform.rotation * localPos * 0.2f;

                brick.transform.position = Vector3.Lerp(brick.transform.position, worldPos, time);
                brick.transform.rotation = Quaternion.Lerp(brick.transform.rotation, transform.rotation, time);
            }
            while (Vector3.Distance(brick.transform.position, worldPos) > 0.1f);

            localPos = Vector3.up * bricks.Count * 0.55f;

            brick.transform.SetParent(transform);
            brick.transform.localPosition = localPos;
            brick.transform.rotation = transform.rotation;

            bricks.Add(brick);

            if (brick.IsMulty)
            {
                for (int i = 0; i < 4; i++)
                {
                    brick = PoolHandler.GetBrick();

                    localPos = Vector3.up * bricks.Count * 0.55f;

                    brick.transform.SetParent(transform);
                    brick.transform.localPosition = localPos;
                    brick.transform.rotation = transform.rotation;
                    
                    bricks.Add(brick);
                }
            }
        }

        public void SetBricks(int amounts)
        {
            BrickBehavior brick;

            for (int i = 0; i < amounts; i++)
            {
                brick = PoolHandler.GetBrick();

                Vector3 localPos = Vector3.up * bricks.Count * 0.55f;

                brick.transform.SetParent(transform);
                brick.transform.localPosition = localPos;
                brick.transform.rotation = transform.rotation;

                bricks.Add(brick);
            }
            
        }

        public bool HasBricks() => !bricks.IsNullOrEmpty();

        public int GetBricksAmount() => bricks.Count;

        public void RemoveBrick(Vector3 targetWorldPosition, Quaternion targetRotation, bool immediately = true, Action onCompleted = null)
        {
            if (!HasBricks())
                return;

            var lastIndex = bricks.Count - 1;
            var brick = bricks[lastIndex];

            bricks.RemoveAt(lastIndex);

            if (immediately)
            {

                brick.transform.position = targetWorldPosition;
                brick.transform.rotation = targetRotation;

                brick.transform.SetParent(null);
                brick.gameObject.SetActive(false);

                onCompleted?.Invoke();

                return;
            }
        }
       
        public void ClearBackpack()
        {
            for (int i = 0; i < bricks.Count; i++)
            {
                bricks[i].transform.SetParent(null);
            }

            bricks = new List<BrickBehavior>();
        }
    }
}