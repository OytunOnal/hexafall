using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HexFall
{
    [RequireComponent(typeof(BoxCollider))]
    public class CoinBehaviour : MonoBehaviour
    {
        [SerializeField] Transform graphicHolder;
        [SerializeField] ParticleSystem pickableParticle;

        private BoxCollider boxCollider;
        private Coroutine rotationCoroutine;

        private Vector3 startedScale;

        private int playerLayer;
        private int enemyLayer;

        private void Awake()
        {
            boxCollider = GetComponent<BoxCollider>();

            startedScale = graphicHolder.localScale;

            playerLayer = LayerMask.NameToLayer("Player");
            enemyLayer = LayerMask.NameToLayer("Enemy");
        }

        private void OnEnable() // Called when object spawned
        {
            graphicHolder.localScale = startedScale; // temp 

            boxCollider.isTrigger = true;
            boxCollider.enabled = true;

            if (pickableParticle != null)
                pickableParticle.gameObject.SetActive(false);

            rotationCoroutine = StartCoroutine(RotationCoroutine());
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.layer == enemyLayer)
            {
                OnCoinPicked();
            }

            if (other.gameObject.layer == playerLayer)
            {
                GameController.Coins++;
                OnCoinPicked();
                AudioController.PlaySound(AudioController.Sounds.coinSound);
            }
        }

        private void OnCoinPicked()
        {
            boxCollider.isTrigger = false;
            boxCollider.enabled = false;

            if (pickableParticle != null)
            {
                // Play particle
                pickableParticle.gameObject.SetActive(true);
                pickableParticle.Play();
            }

            graphicHolder.DOScale(Vector3.zero, 0.25f).SetEasing(Ease.Type.BackIn).OnComplete(delegate
            {
                if (rotationCoroutine != null)
                {
                    StopCoroutine(rotationCoroutine);
                    rotationCoroutine = null;
                }

                gameObject.SetActive(false);
            });
        }

        private IEnumerator RotationCoroutine()
        {
            float axisY = graphicHolder.localRotation.y;

            while (true)
            {
                if (axisY >= 360f)
                    axisY = 0;

                axisY++;

                transform.localRotation = Quaternion.Euler(0f, axisY, 0f);

                yield return Time.deltaTime;
            }
        }
    }
}