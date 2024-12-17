using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Tutorial
{
    public class FlappyBaseTutorial : GameTutorialController
    {
        [SerializeField] private GameObject container;
        public override void StartTutorial()
        {
            container.SetActive(true);    
        }
        
        private void Update()
        {
            if(EventSystem.current.IsPointerOverGameObject()) return;
            if (Input.GetMouseButtonDown(0))
            {
                container.SetActive(false);
            }
        }
    }

}