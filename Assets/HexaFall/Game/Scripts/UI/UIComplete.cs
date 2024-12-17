using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;
using System.Collections.Generic;
using TMPro;

namespace HexFall
{
    public class UIComplete : UIPage
    {
        [SerializeField] UIFade backgroundFade;

        [Space]
        [SerializeField] UIScalableObject levelCompleteLabel;

        [Space]
        [SerializeField] UIScalableObject rewardLabel;
        [SerializeField] TMP_Text rewardAmountText;
        [SerializeField] Image rewardIconImage;

        [Space]
        [SerializeField] UIFade homeLabel;
        [SerializeField] Button homeButton;

        [Space]
        [SerializeField] UIFade nextButtonFade;
        [SerializeField] Button nextButton;

        private int currentReward;

        public override void Initialise()
        {
            homeButton.onClick.AddListener(HomeButton);
            nextButton.onClick.AddListener(NextButton);

            rewardIconImage.sprite = CurrenciesController.GetCurrency(CurrencyType.Coin).Icon;
        }

        #region Show/Hide

        public override void PlayShowAnimation()
        {
            if (isPageDisplayed)
                return;

            isPageDisplayed = true;

            rewardLabel.Hide(immediately: true);
            HideHomeButton(immediately: true);
            nextButtonFade.Hide(immediately: true);

            backgroundFade.Show(duration: 0.3f, false);

            currentReward = (int)LevelController.LevelReward;

            levelCompleteLabel.Show(immediately: false, duration: 0.3f, onCompleted: () =>
            {
                ShowRewardLabel(currentReward, false, 0.3f, delegate
                {
                    rewardLabel.RectTransform.DOPushScale(Vector3.one * 1.1f, Vector3.one, 0.2f, 0.2f).OnComplete(delegate
                    {
                        Tween.DelayedCall(0.2f, delegate
                        {
                            nextButtonFade.Show(immediately: false);
                            ShowHomeButton(false);

                            UIController.OnPageOpened(this);
                        });
                    });
                });
            });
        }

        public override void PlayHideAnimation()
        {
            if (!isPageDisplayed)
                return;

            isPageDisplayed = false;

            backgroundFade.Hide(0.25f, false, () =>
            {
                UIController.OnPageClosed(this);
            });
        }

        #endregion

        #region RewardLabel

        public void ShowRewardLabel(float rewardAmount, bool immediately = false, float duration = 0.3f, Action onComplted = null)
        {
            rewardLabel.Show(immediately: immediately);

            if (immediately)
            {
                rewardAmountText.text = "+" + rewardAmount;
                onComplted?.Invoke();

                return;
            }

            rewardAmountText.text = "+" + 0;

            Tween.DoFloat(0, rewardAmount, duration, (float value) =>
            {

                rewardAmountText.text = "+" + (int)value;
            }).OnComplete(delegate
            {

                onComplted?.Invoke();
            });
        }

        #endregion

        private void ShowHomeButton(bool immediately)
        {
            homeButton.interactable = true;
            homeLabel.Show(immediately: immediately);
        }

        private void HideHomeButton(bool immediately)
        {
            homeButton.interactable = false;
            homeLabel.Hide(immediately: immediately);
        }

        #region Buttons

        public void NextButton()
        {
            GameController.instance.NextLevel();
            AudioController.PlaySound(AudioController.Sounds.buttonSound);
        }

        public void HomeButton()
        {
            GameController.instance.OnHomeButtonPressed();
            AudioController.PlaySound(AudioController.Sounds.buttonSound);
        }

        #endregion
    }
}
