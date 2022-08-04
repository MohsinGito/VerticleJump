using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentPatch : MonoBehaviour
{

    #region Public Attributes

    [Header("Movement Variables")]
    public float maxPlatforms;
    public float platformOffset;
    public float startSpawnY;
    public int spawnProbability;
    public Vector2 xMinMax;

    [Header("Display Sprites")]
    public SpriteRenderer layer1;
    public SpriteRenderer layer2;
    public SpriteRenderer layer3;

    [Header("Flying Obstacle Info")]
    public float flyingObstacleMoveSpeed;
    public List<FlyingObstacle> flyingObstacleInfo;
    public List<string> flyingObstacleNames;

    #endregion

    #region Private Attributes

    private GameData gameData;
    private bool canSpawnEnemies;
    private GameStage currentStageInfo;
    private List<Platform> platforms;
    private List<PoolObj> flyingObstacles;

    #endregion

    #region Public Methods

    public void Init(GameStage _stageInfo, GameData _gameData, bool _canSpawnEnemies = true)
    {
        ResetPatch();

        gameData = _gameData;   
        currentStageInfo = _stageInfo;
        layer1.sprite = _stageInfo.layerOne;
        layer2.sprite = _stageInfo.layerTwo;
        layer3.sprite = _stageInfo.layerThree;
        canSpawnEnemies = _canSpawnEnemies;

        SetUpPatch();
    }

    #endregion

    #region Private Methods


    public void ResetPatch()
    {
        if(platforms == null)
        {
            platforms = new List<Platform>();
            flyingObstacles = new List<PoolObj>();
            return;
        }

        StopAllCoroutines();

        foreach (Platform platform in platforms)
        {
            PoolManager.Instance.ReturnToPool("Platform", platform.gameObject);
        }
        platforms.Clear();

        foreach (PoolObj obj in flyingObstacles)
        {
            PoolManager.Instance.ReturnToPool(obj.Tag, obj.Prefab);
        }
        flyingObstacles.Clear();
    }

    private void SetUpPatch()
    {
        for (int i = 0; i < maxPlatforms; i++)
        {
            if (i == 0)
                SpawnNewPlatform(startSpawnY);
            else if (i == maxPlatforms / 2)
                SpawnNewPlatform(startSpawnY + ((maxPlatforms - 1) / 2 * platformOffset));
            else if (i == maxPlatforms - 1)
                SpawnNewPlatform(startSpawnY + ((maxPlatforms - 1) * platformOffset));
            else
            {
                if (Random.Range(0, spawnProbability) == Mathf.FloorToInt(spawnProbability / 2))
                    SpawnObstacle(Random.Range(0, 2) == 0, startSpawnY + ((maxPlatforms - i - 1) * platformOffset));
                else
                    SpawnNewPlatform(startSpawnY + ((maxPlatforms - i - 1) * platformOffset));
            }
        }

        SpawnPatchFlyingObstacle();
    }

    private void SpawnObstacle(bool _spawnGroundObstacle, float nextPatchYPos)
    {
        if (!canSpawnEnemies)
            return;

        SpawnNewPlatform(nextPatchYPos, false);
        platforms[platforms.Count - 1].Obstacle = Element.CONTAIN;
        platforms[platforms.Count - 1].Coins = Element.CONTAIN;
    }
    
    private void SpawnPatchFlyingObstacle()
    {
        foreach(FlyingObstacle flyingObstacle in flyingObstacleInfo)
        {
            if(gameData.sessionScores > flyingObstacle.criteriaScores)
            {
                SetNewFlyingObstacle(flyingObstacle.leftSide.position, flyingObstacle.rightSide.position);
            }
        }
    }

    private void SpawnNewPlatform(float _platformYPos, bool _canHaveCoins = true)
    {
        float _platformXPos = Random.Range(xMinMax.x, xMinMax.y);

        platforms.Add(PoolManager.Instance.GetFromPool("Platform").GetComponent<Platform>());
        platforms[platforms.Count - 1].ResetPlatForm();

        platforms[platforms.Count - 1].State = Random.Range(0, 2) == 1 ? PlatformType.SINGLE  : PlatformType.DOUBLE;
        platforms[platforms.Count - 1].Coins = Random.Range(0, 2) == 1 && _canHaveCoins ? Element.CONTAIN : Element.IDLE;
        platforms[platforms.Count - 1].transform.position = new Vector3(_platformXPos, _platformYPos, 0) + transform.position;
        platforms[platforms.Count - 1].DisplaySprite = currentStageInfo.platform;
        platforms[platforms.Count - 1].transform.parent = transform;
    }

    private void SetNewFlyingObstacle(Vector3 leftPos, Vector3 rightPos)
    {
        StartCoroutine(StartFlying());
        IEnumerator StartFlying()
        {
            string reandomObstacle = flyingObstacleNames[Random.Range(0, flyingObstacleNames.Count)];
            Transform obstacle = PoolManager.Instance.GetFromPool(reandomObstacle).transform;
            flyingObstacles.Add(new PoolObj(reandomObstacle, obstacle.gameObject));
            
            float rotation = -180f;
            Vector3 swapVar = Vector3.zero;

            // Randomly Selecting Target Positions
            if (Random.Range(0, 2) == 1)
            {
                swapVar = leftPos;
                leftPos = rightPos;
                rightPos = swapVar;

                rotation *= -1f;
                obstacle.transform.eulerAngles += new Vector3(0, rotation, 0);
            }

            while (true)
            {
                // Moving Towards Target Positions
                obstacle.transform.position = leftPos;
                yield return obstacle.DOMove(rightPos, flyingObstacleMoveSpeed).WaitForCompletion();

                // Changing Rotation Of Obstacle
                rotation *= -1f;
                obstacle.transform.eulerAngles += new Vector3(0, rotation, 0);

                // Sawping Target Positions
                swapVar = leftPos;
                leftPos = rightPos;
                rightPos = swapVar;
            }
        }
    }

    #endregion

}

[System.Serializable]
public struct FlyingObstacle
{
    public Transform leftSide;
    public Transform rightSide;
    public int criteriaScores;
}