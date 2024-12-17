using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace HexFall
{
    public class UIMainMenu : UIPage
    {

        [Space]
        [SerializeField] RectTransform tapToPlayRect;
        [SerializeField] Button tapToPlayButton;


        private TweenCase tapToPlayPingPong;
        private TweenCase showHideStoreAdButtonDelayTweenCase;


        public override void Initialise()
        {
            tapToPlayButton.onClick.AddListener(TapToPlayButton);

            UpdateNicknameInput();
        }

        #region Show/Hide

        public override void PlayShowAnimation()
        {
            if (isPageDisplayed)
                return;

            showHideStoreAdButtonDelayTweenCase?.Kill();


            isPageDisplayed = true;

            ShowTapToPlay(false);


            showHideStoreAdButtonDelayTweenCase = Tween.DelayedCall(0.12f, delegate
            {
                UIController.OnPageOpened(this);
            });

        }

        public override void PlayHideAnimation()
        {
            if (!isPageDisplayed)
                return;

            showHideStoreAdButtonDelayTweenCase?.Kill();

            isPageDisplayed = false;

            HideTapToPlayText(false);


            Tween.DelayedCall(0.55f, delegate
            {
                UIController.OnPageClosed(this);
            });
        }

        #endregion

        #region Tap To Play Label

        public void ShowTapToPlay(bool immediately = true)
        {
            if (tapToPlayPingPong != null && tapToPlayPingPong.isActive)
                tapToPlayPingPong.Kill();

            if (immediately)
            {
                tapToPlayRect.localScale = Vector3.one;

                tapToPlayPingPong = tapToPlayRect.transform.DOPingPongScale(1.0f, 1.05f, 0.9f, Ease.Type.QuadIn, Ease.Type.QuadOut, unscaledTime: true);

                return;
            }

            // RESET
            tapToPlayRect.localScale = Vector3.zero;

            tapToPlayRect.DOPushScale(Vector3.one * 1.2f, Vector3.one, 0.35f, 0.2f, Ease.Type.CubicOut, Ease.Type.CubicIn).OnComplete(delegate
            {

                tapToPlayPingPong = tapToPlayRect.transform.DOPingPongScale(1.0f, 1.05f, 0.9f, Ease.Type.QuadIn, Ease.Type.QuadOut, unscaledTime: true);

            });

        }

        public void HideTapToPlayText(bool immediately = true)
        {
            if (tapToPlayPingPong != null && tapToPlayPingPong.isActive)
                tapToPlayPingPong.Kill();

            if (immediately)
            {
                tapToPlayRect.localScale = Vector3.zero;

                return;
            }

            tapToPlayRect.DOPushScale(Vector3.one * 1.2f, Vector3.zero, 0.2f, 0.35f, Ease.Type.CubicOut, Ease.Type.CubicIn);
        }

        #endregion

        #region Nickname Input

        private void UpdateNicknameInput()
        {

            PlayerBehaviour.InitNickname();
        }

        public void OnNicknameUpdated(string newName)
        {
            SaveController.GetSaveObject<SimpleStringSave>(PlayerBehaviour.NICK_SAVE_HASH).Value = newName;
            UpdateNicknameInput();
        }

        #endregion

        #region Buttons

        public void TapToPlayButton()
        {
            GameController.instance.OnGameStarted();
            AudioController.PlaySound(AudioController.Sounds.buttonSound);
        }

        #endregion
    }


}
