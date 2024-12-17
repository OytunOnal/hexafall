using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HexFall
{
    public class GameController : MonoBehaviour
    {
        public static GameController instance;
        public static readonly string LEVEL_INDEX_HASH = "level_index";

        [Header("Settings")]
        [SerializeField] bool useNicknames;

        [Header("References")]
        [SerializeField] LevelsDatabase levelsDatabase;
        [SerializeField] UIController uiController;
        [SerializeField] LevelController levelController;
        [SerializeField] CurrenciesController currenciesController;
        private static int _levelIndex;

        public static int LevelIndex
        {
            get => _levelIndex;
            set
            {
                _levelIndex = value;
            }
        }

        public static int Coins
        {
            get => CurrenciesController.Get(CurrencyType.Coin);
            set
            {
                CurrenciesController.Set(CurrencyType.Coin, value);
            }
        }

        public static bool UseNicknames => instance.useNicknames;

        public static SimpleCallback OnLevelChangedEvent;
        private static LevelsDatabase LevelDatabase => instance.levelsDatabase;
        private static SimpleIntSave levelIndexSave;

        private void Awake()
        {
            if (instance != null)
            {
                Destroy(gameObject);
                return;
            }
            instance = this;

            SaveController.Initialise(false);
            levelIndexSave = SaveController.GetSaveObject<SimpleIntSave>(LEVEL_INDEX_HASH);

            if (SaveController.GetSaveObject<SimpleStringSave>("player_nick").Value == null || SaveController.GetSaveObject<SimpleStringSave>("player_nick").Value.Equals(""))
            {
                SaveController.GetSaveObject<SimpleStringSave>("player_nick").Value = "Guest" + UnityEngine.Random.Range(1001, 9999);
            }
        }

        private void OnDisable()
        {
            instance = null;
        }

        private void Start()
        {
            currenciesController.Initialise();

            uiController.Initialise();
            uiController.InitialisePages();

            SkinStoreController.Init();

            LoadGame();
            GameLoading.MarkAsReadyToHide();
        }

        private static Level GetLevel()
        {
            LevelIndex = UnityEngine.Random.Range(0, LevelDatabase.levels.Length);

            return LevelDatabase.levels[LevelIndex];
        }

        private void LoadGame()
        {
            levelController.LoadLevel(GetLevel());

            uiController.ShowPage<UIMainMenu>();
        }

        public void OnGameStarted()
        {
            levelController.OnGameplayStarted();

            uiController.HidePage<UIMainMenu>(() =>
            {
                uiController.ShowPage<UIGame>();
            });
        }

        public void OnLevelFailed()
        {
            levelController.OnGameplayKill();

            uiController.HidePage<UIGame>(() =>
           {
               AudioController.PlaySound(AudioController.Sounds.loseSound);
               CommonUIManager.Instance.SwitchPanel(CommonPanelType.Lose);
           });
        }

        public void OnLevelCompleted()
        {
            levelController.OnGameplayKill();

            uiController.HidePage<UIGame>(() =>
            {
                AudioController.PlaySound(AudioController.Sounds.winSound);
                CommonUIManager.Instance.SwitchPanel(CommonPanelType.Win);
                PlayerBehaviour.DisableJoystick();
            });
        }

        public void NextLevel()
        {
            levelController.LoadLevel(GetLevel());

            uiController.HidePage<UIGame>();
            uiController.HidePage<UIComplete>(() =>
             {
                 uiController.ShowPage<UIMainMenu>();
             });
        }

        public void OnHomeButtonPressed()
        {
            uiController.HidePage<UIComplete>(() =>
            {
                levelController.LoadLevel(GetLevel());

                UIController.instance.ShowPage<UIMainMenu>();
            });
        }


        public void OnNoThanksButtonPressed()
        {
            uiController.HidePage<UIGameOver>(() =>
            {
                levelController.LoadLevel(GetLevel());

                uiController.ShowPage<UIMainMenu>();
            });
        }
    }
}