using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Tutorial
{
    public class TutorialRepeater : MonoBehaviour
    {
        [SerializeField] private Button repeatButton;
        [SerializeField] private GameTutorialController tutorialController;
        private void OnEnable()
        {
            repeatButton.onClick.AddListener(RepeatTutorial);
        }
        private void OnDisable()
        {
            repeatButton.onClick.RemoveListener(RepeatTutorial);
        }

        private void RepeatTutorial()
        {
            tutorialController.StartTutorial();
            gameObject.SetActive(false);
        }
    }

}