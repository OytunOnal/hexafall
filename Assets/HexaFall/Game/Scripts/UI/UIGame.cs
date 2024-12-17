using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace HexFall
{
    public class UIGame : UIPage
    {
        [SerializeField] UIScalableObject playerAmountsLabel;
        [SerializeField] TMP_Text playerAmountsText;

        [SerializeField] Joystick joystick;

        private void OnEnable()
        {
            LevelController.OnPlayerAmountChangedEvent += UpdatePlayerAmountsText;
        }

        private void OnDisable()
        {
            LevelController.OnPlayerAmountChangedEvent -= UpdatePlayerAmountsText;
        }

        public override void Initialise()
        {
            joystick.Initialise(canvas);
        }

        #region Show/Hide

        public override void PlayHideAnimation()
        {
            if (!isPageDisplayed)
                return;

            playerAmountsLabel.Hide(scaleMultiplier: 1.05f);

            Tween.DelayedCall(0.55f, delegate
            {

                isPageDisplayed = false;
                UIController.OnPageClosed(this);
            });
        }

        public override void PlayShowAnimation()
        {
            if (isPageDisplayed)
                return;

            isPageDisplayed = true;
            UpdatePlayerAmountsText();

            playerAmountsLabel.Show(scaleMultiplier: 1.05f);

            UIController.OnPageOpened(this);
        }

        #endregion

        public void UpdatePlayerAmountsText()
        {
            playerAmountsText.text = LevelController.PlayersAmount.ToString();
        }
    }
}
