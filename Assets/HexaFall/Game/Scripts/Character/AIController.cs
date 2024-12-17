using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HexFall
{

    public class AIController : MonoBehaviour
    {
        [SerializeField] int botsAmount;
        private static int BotsAmount => instance.botsAmount;

        [SerializeField] NicknamesDatabase nicknamesDatabase;

        private static AIController instance;

        private static Pool enemiesPool;
        private static List<EnemyBehaviour> enemiesList = new List<EnemyBehaviour>();

        public static SimpleCallback OnOponentsAmountChanged;

        public static int AliveEnemiesAmount { get; private set; }

        private void Awake()
        {
            if (instance != null)
            {
                Destroy(this);
                return;
            }

            instance = this;
            enemiesPool = PoolManager.GetPoolByName("Enemy");

        }

        private void OnDestroy()
        {
            instance = null;
        }

        public void CreateEnemies(Level level)
        {
            AliveEnemiesAmount = Mathf.Clamp(BotsAmount, 1, LevelController.SpawnLayer.island.activeHexes.Length - 1);
            OnOponentsAmountChanged?.Invoke();

            List<string> botsNicknames = new List<string>();
            botsNicknames = instance.nicknamesDatabase.GetRandomUniqueNicks(AliveEnemiesAmount);

            for (int i = 0; i < AliveEnemiesAmount; i++)
            {
                var position = LevelController.SpawnLayer.island.activeHexes[i + 1].position;

                var enemy = enemiesPool.GetPooledObject(position).GetComponent<EnemyBehaviour>();

                // Reset
                Transform enemyTransform = enemy.transform;
                enemyTransform.localScale = Vector3.one;
                enemyTransform.eulerAngles = Vector3.zero;

                enemy.Init(botsNicknames[i]);

                enemiesList.Add(enemy);
            }
        }

        public static void OnEnemyDied()
        {
            AliveEnemiesAmount--;
            OnOponentsAmountChanged?.Invoke();

            if (AliveEnemiesAmount <= 0)
            {
                LevelController.OnAllEnemiesDied();
            }
        }

        public void ReturnToPoolEnemies()
        {
            for (int i = 0; i < enemiesList.Count; i++)
            {
                if (enemiesList[i]!=null)
                enemiesList[i].gameObject.SetActive(false);
            }

            enemiesList.Clear();
        }


        //____________________________HEX____________________________

        public static List<Hex> FindRouteToHex(Hex start, Hex goal, int bricksAvailable)
        {
            var frontier = new Queue<HexRouteData>();
            frontier.Enqueue(new HexRouteData() { hex = start, bricksAvailable = bricksAvailable });

            var cameFrom = new Dictionary<Hex, Hex>();
            cameFrom.Add(start, null);

            while (frontier.Count != 0)
            {
                var current = frontier.Dequeue();

                if (current.hex == goal)
                    break;

                current.hex.ForEachNeighbour((neighbour) =>
                {
                    if (!cameFrom.ContainsKey(neighbour))
                    {
                        if (neighbour.State == HexState.Disabled)
                        {
                            if (current.bricksAvailable > 0)
                            {
                                frontier.Enqueue(new HexRouteData() { hex = neighbour, bricksAvailable = current.bricksAvailable - 1 });
                                cameFrom.Add(neighbour, current.hex);
                            }
                        }
                        else
                        {
                            frontier.Enqueue(new HexRouteData() { hex = neighbour, bricksAvailable = current.bricksAvailable });
                            cameFrom.Add(neighbour, current.hex);
                        }
                    }
                }, true);
            }

            if (frontier.Count == 0)
            {
                return null;
            }

            var route = new List<Hex>();
            route.Add(goal);

            var currentHex = goal;
            while (currentHex != start)
            {
                var prevHex = cameFrom[currentHex];

                route.Add(prevHex);

                currentHex = prevHex;
            }

            route.Reverse();

            route.RemoveAt(0);

            return route;
        }


        public struct HexRouteData
        {
            public Hex hex;

            public int bricksAvailable;
        }
    }

}