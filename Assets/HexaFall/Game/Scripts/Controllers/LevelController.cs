using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace HexFall
{

    public class LevelController : MonoBehaviour
    {
        public static LevelController instance;

        private static LevelSpawner levelSpawner = new LevelSpawner();

        [SerializeField] float chanceOfSingleBrickSpawn;
        [SerializeField] float chanceOfMultyBrickSpawn;
        [SerializeField] float chanceOfCoinSpawn;

        [Space(5f)]
        [SerializeField] Layer spawnLayer;
        [SerializeField] Transform waterTransform;
        [SerializeField] ColorsDatabase colorsDatabase;
        [SerializeField] AIController aiController;


        public static float DisableHeight { get; private set; }
        public static bool IsGameplayActive { get; private set; }
        public static int PlayersAmount { get; private set; }

        public static SimpleCallback OnPlayerAmountChangedEvent;

        public static Layer SpawnLayer => instance.spawnLayer;
        public static float ChanceOfSingleBrickSpawn => instance.chanceOfSingleBrickSpawn;
        public static float ChanceOfMultyBrickSpawn => instance.chanceOfMultyBrickSpawn;
        public static float ChanceOfCoinSpawn => instance.chanceOfCoinSpawn;

        public static float LevelReward => survivedSeconds * 1f;

        private static float survivedSeconds = 0;
        private static Level level;
        private static int initialPlayersAmount;
        private static float bottomFloorHeight;

        private void Awake()
        {
            if (instance != null)
            {
                Destroy(gameObject);
                return;
            }
            instance = this;

            PoolHandler.Init();
            colorsDatabase.Init();
        }

        private void OnDestroy()
        {
            instance = null;
        }
        private void Update()
        {
            if (!IsGameplayActive)
                return;

            survivedSeconds += Time.deltaTime;
        }

        public void LoadLevel(Level levelData)
        {
            level = levelData;

            DissposeLevel();

            levelSpawner.LoadLevel(levelData);

            PlayerBehaviour.Init(SpawnLayer.island.activeHexes[0].position);

            aiController.CreateEnemies(level);

            DisableHeight = 0;
            DisableHeight += level.GetLevelHeight() - 5f;

            instance.waterTransform.position = Vector3.zero.SetY(DisableHeight + 3f);

            initialPlayersAmount = 1 + AIController.AliveEnemiesAmount;
            PlayersAmount = initialPlayersAmount;
            OnPlayerAmountChangedEvent?.Invoke();

            bottomFloorHeight = levelData.layers[levelData.layers.Length - 1].offsetY;
        }

        public void OnGameplayStarted()
        {
            IsGameplayActive = true;
        }

        public void OnGameplayKill()
        {
            IsGameplayActive = false;
        }


        public static void OnAllEnemiesDied()
        {
            if (!IsGameplayActive)
                return;

            // if player also already falling into the water - don't count level as completed
            if (PlayerBehaviour.Position.y < (bottomFloorHeight - 1f))
                return;

            IsGameplayActive = false;

            PlayerBehaviour.PlayWinAnimation();

            GameController.instance.OnLevelCompleted();
        }

        public static void OnRevived()
        {
            if (level == null)
            {
                //Debug.LogWarning("Level wasn't inited");
                return;
            }

            Hex hex = HexMap.GetRandomActiveHex(HexMap.MapLayersList.Count - 1);

            if (hex == null)
            {
                //Debug.LogWarning("Error: There is on available active hax on the last island");
                return;
            }
            PlayerBehaviour.OnRevived(hex.GetWorldCoords());

            IsGameplayActive = true;
            UpdatePlayersAmount(1);
        }

        public static void OnPlayerDied()
        {
            if (!IsGameplayActive)
                return;

            IsGameplayActive = false;

            GameController.instance.OnLevelFailed();
        }

        public static void UpdatePlayersAmount(int delta)
        {
            PlayersAmount = Mathf.Clamp(PlayersAmount + delta, 0, initialPlayersAmount);
            OnPlayerAmountChangedEvent?.Invoke();

            if (PlayersAmount <= 1)
                OnAllEnemiesDied();
        }

        private void DissposeLevel()
        {
            survivedSeconds = 0f;

            PoolHandler.ReturnToPoolsEverything();
            HexMap.Clear();
            aiController.ReturnToPoolEnemies();
        }
    }
}