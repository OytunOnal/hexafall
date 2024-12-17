using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Tutorial
{
    public class DragMovement : GameTutorialController
    {
        [SerializeField] private Transform objectToMove;
        [SerializeField] private Transform[] positions;
        [SerializeField] private float timeToMove;

        [SerializeField] private bool isFading = false;
        [SerializeField] private Image objectToFade;
        [SerializeField] private float fadeDuration;
        [SerializeField] private GameObject container;
        private void Start()
        {
            StartTutorial();
        }

        public override void StartTutorial()
        {
            container.SetActive(true);
            StartCoroutine(Tutorial());
        }

        private void OnDisable()
        {
            StopAllCoroutines();
            DOTween.KillAll();
        }
        private void Update()
        {
            if (Input.GetMouseButtonDown(0) && !IsClickingOnButton())
            {
                container.SetActive(false);
            }
        }

        bool IsClickingOnButton()
        {
            // Cast a ray from the mouse position
            PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
            eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            GraphicRaycaster uiRaycaster = GameObject.FindObjectOfType<GraphicRaycaster>();

            // Check for UI elements
            List<RaycastResult> results = new List<RaycastResult>();
            uiRaycaster.Raycast(eventDataCurrentPosition, results);

            foreach (RaycastResult result in results)
            {
                if (result.gameObject.GetComponent<Button>() != null)
                {
                    return true; // Clicking on a UI button
                }
            }
            return false; // Not clicking on a UI button
        }

        private IEnumerator Tutorial()
        {
            while (enabled)
            {
                objectToMove.position = positions[0].position;
                if(isFading)objectToFade.color = new Color(objectToFade.color.r, objectToFade.color.g, objectToFade.color.b, 1);
                
                
                for(int i = 1; i < positions.Length; i++)
                {
                    float duration = Vector3.Distance(objectToMove.position, positions[i].position) * timeToMove;
                    objectToMove.DOMove(positions[i].position,duration ).SetEase(Ease.Linear);
                    yield return new WaitForSeconds(duration);
                }

                if (isFading)
                {
                    objectToFade.DOFade(0, fadeDuration);
                    yield return new WaitForSeconds(fadeDuration);
                }
                


            }
        }
        
    }

}