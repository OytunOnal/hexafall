using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace HexFall
{
    public class UIGameOver : UIPage
    {
        [SerializeField] float noThanksDelay;
        
        [Space]
        [SerializeField] UIScalableObject levelFailed;
        [SerializeField] UIFade backgroundFade;
        [SerializeField] UIScalableObject continueButtonScalable;

        [Space]
        [SerializeField] Button noThanksButton;
        [SerializeField] TMP_Text noThanksText;

        private TweenCase continuePingPongCase;

        public override void Initialise()
        {
            noThanksButton.onClick.AddListener(NoThanksButton);
        }

        #region Show/Hide

        public override void PlayShowAnimation()
        {
            if (isPageDisplayed)
                return;

            isPageDisplayed = true;

            levelFailed.Hide(immediately: true);
            continueButtonScalable.Hide(immediately: true);
            HideNoThanksButton();

            backgroundFade.Show(0.3f, false, () =>
            {
                levelFailed.Show( scaleMultiplier: 1.1f);

                ShowNoThanksButton(noThanksDelay, immediately: false);

                continueButtonScalable.Show(scaleMultiplier: 1.05f, onCompleted: () =>
                {
                    continuePingPongCase = continueButtonScalable.RectTransform.DOPingPongScale(1.0f, 1.05f, 0.9f, Ease.Type.QuadIn, Ease.Type.QuadOut, unscaledTime: true);

                    UIController.OnPageOpened(this);
                });
            });
        }

        public override void PlayHideAnimation()
        {
            if (!isPageDisplayed)
                return;

            backgroundFade.Hide(0.3f, false, () =>
            {
                isPageDisplayed = false;
                UIController.OnPageClosed(this);

                if (continuePingPongCase != null && continuePingPongCase.isActive)
                    continuePingPongCase.Kill();
            });
        }

        #endregion

        #region NoThanks Block

        public void ShowNoThanksButton(float delayToShow = 0.3f, bool immediately = true)
        {
                noThanksButton.gameObject.SetActive(true);
                noThanksText.gameObject.SetActive(true);

        }

        public void HideNoThanksButton()
        {
            noThanksButton.gameObject.SetActive(false);
            noThanksText.gameObject.SetActive(false);
        }

        #endregion

        #region Buttons 

        public void NoThanksButton()
        {
            GameController.instance.OnNoThanksButtonPressed();
            AudioController.PlaySound(AudioController.Sounds.buttonSound);
        }

        #endregion
    }
}