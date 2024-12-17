using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace Tutorial
{
    public class SwerveAnimation : MonoBehaviour
    {
        [SerializeField] private float time = 1;
        [SerializeField] private float endXPos;
        void Start()
        {
            transform.DOLocalMoveX(endXPos, time).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.Linear);
        }

    }
}
