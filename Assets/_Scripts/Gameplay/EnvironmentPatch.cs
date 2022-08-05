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
    public List<Platform> platforms;
    public List<Platform> mustPlatforms;

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
        foreach (Platform platform in platforms)
        {
            platform.gameObject.SetActive(false);
        }

        if (flyingObstacles == null)
        {
            flyingObstacles = new List<PoolObj>();
            return;
        }

        foreach (PoolObj obj in flyingObstacles)
        {
            PoolManager.Instance.ReturnToPool(obj.Tag, obj.Prefab);
        }

        flyingObstacles.Clear();
        StopAllCoroutines();
    }

    private void SetUpPatch()
    {
        foreach(Platform platform in mustPlatforms)
        {
            SetUpPlarform(platform);
        }

        for (int i = 0; i < platforms.Count; i++)
        {
            if (Random.Range(0, spawnProbability) == Mathf.FloorToInt(spawnProbability / 2))
                SetUpObstacle(platforms[i], Random.Range(0, 2) == 0);
            else
                SetUpPlarform(platforms[i]);
        }

        SpawnPatchFlyingObstacle();
    }

    private void SetUpObstacle(Platform platform, bool _spawnGroundObstacle)
    {
        if (!canSpawnEnemies)
            return;

        SetUpPlarform(platform, false);
        platforms[platforms.Count - 1].Obstacle = Element.CONTAIN;
        platforms[platforms.Count - 1].Coins = Element.CONTAIN;
    }

    private void SetUpPlarform(Platform platform, bool _canHaveCoins = true)
    {
        platform.gameObject.SetActive(true);
        float _platformXPos = Random.Range(xMinMax.x, xMinMax.y);

        platform.ResetPlatForm();
        platform.transform.position = new Vector3(_platformXPos, platform.transform.position.y, 0);
        platform.DisplaySprite = currentStageInfo.platform;

        if (Random.Range(0, 2) == 1)
        {
            platform.State = PlatformType.SINGLE;
            platform.Coins = Random.Range(0, 2) == 0 && _canHaveCoins ? Element.CONTAIN : Element.IDLE;
        }
        else
        {
            platform.State = PlatformType.DOUBLE;
            if(Random.Range(0, 2) == 0)
            {
                platform.Coins = Element.CONTAIN;
                if (canSpawnEnemies && Random.Range(0, 2) == 0) { platform.Obstacle = Element.CONTAIN; }
            }
            else
            {
                if (canSpawnEnemies && Random.Range(0, 2) == 0) { platform.Obstacle = Element.CONTAIN; }
                platform.Coins = Element.CONTAIN;
            }
        }
    }

    private void SpawnPatchFlyingObstacle()
    {
        foreach (FlyingObstacle flyingObstacle in flyingObstacleInfo)
        {
            if (gameData.sessionScores > flyingObstacle.criteriaScores)
            {
                SetNewFlyingObstacle(flyingObstacle.leftSide.position, flyingObstacle.rightSide.position);
            }
        }
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