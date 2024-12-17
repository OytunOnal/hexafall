using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HexFall
{
    public static class PoolHandler
    {
        private const string PLATFORM_POOL_NAME = "Platform";
        private const string BRICK_POOL_NAME = "Brick";      
        private const string COINS_POOL_NAME = "Coin";

        private static Pool platformsPool;
        private static Pool bricksPool;
        private static Pool bordersPool;
        private static Pool coinsPool;

        public static void Init()
        {
            platformsPool = PoolManager.GetPoolByName(PLATFORM_POOL_NAME);
            bricksPool = PoolManager.GetPoolByName(BRICK_POOL_NAME);           
            coinsPool = PoolManager.GetPoolByName(COINS_POOL_NAME);
        }

        public static PlatformBehaviour GetPlatform()
        {
            return platformsPool.GetPooledObject().GetComponent<PlatformBehaviour>();
        }

        public static BrickBehavior GetBrick()
        {
            return bricksPool.GetPooledObject().GetComponent<BrickBehavior>();
        }       


        public static void ReturnToPoolsEverything()
        {
            bricksPool?.ReturnToPoolEverything(true);
            bordersPool?.ReturnToPoolEverything();
            platformsPool?.ReturnToPoolEverything();
        }
    }
}
