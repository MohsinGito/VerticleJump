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
    private bool notInInitialStages;
    private GameStage currentStageInfo;
    private List<PoolObj> flyingObstacles;

    #endregion

    #region Public Methods

    public void Init(GameStage _stageInfo, GameData _gameData, bool _notInInitialStages = true)
    {
        ResetPatch();

        gameData = _gameData;
        currentStageInfo = _stageInfo;
        layer1.sprite = _stageInfo.layerOne;
        layer2.sprite = _stageInfo.layerTwo;
        layer3.sprite = _stageInfo.layerThree;
        notInInitialStages = _notInInitialStages;
        flyingObstacleNames = Generics<string>.Randomize(flyingObstacleNames);

        SetUpPatch();
    }

    public void SetPowerUpOnPatch(PowerUp powerUp, int max)
    {
        List<Platform> randomPlatforms = Generics<Platform>.Randomize(mustPlatforms);
        for (int i = 0; i < max; i++)
        {
            if (max > randomPlatforms.Count)
                break;

            if(!randomPlatforms[i].PowerUpAssigned)
            {
                randomPlatforms[i].ResetPlatForm();
                randomPlatforms[i].State = GetRandomPlatformType();
                SetPowerUp(randomPlatforms[i]);
            }
            else
            {
                max += 1;
            }
        }

        void SetPowerUp(Platform platform)
        {
            switch (powerUp)
            {
                case PowerUp.EXTRA_LIFE:
                    platform.ExtraLife = Element.CONTAIN;
                    break;
                case PowerUp.JUMP_BOOST:
                    platform.JumpBoost = Element.CONTAIN;
                    break;
            }
            SetCoinOrObstacle(platform);
        }
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
            obj.Prefab.transform.DOKill();
            PoolManager.Instance.ReturnToPool(obj.Tag, obj.Prefab);
        }

        flyingObstacles.Clear();
    }

    private void SetUpPatch()
    {
        foreach (Platform platform in mustPlatforms)
        {
            SetUpPlarform(platform);
        }

        if (notInInitialStages)
        {
            for (int i = 0; i < platforms.Count; i++)
            {
                if (Random.Range(0, spawnProbability) == Mathf.FloorToInt(spawnProbability / 2))
                    SetUpObstacle(platforms[i], Random.Range(0, 2) == 0);
                else
                    SetUpPlarform(platforms[i]);
            }

            SpawnPatchFlyingObstacle();
        }
    }

    private void SetUpObstacle(Platform platform, bool _spawnGroundObstacle)
    {
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
            if (Random.Range(0, 2) == 0)
            {
                platform.Coins = Element.CONTAIN;
                if (notInInitialStages && Random.Range(0, 2) == 0) { platform.Obstacle = Element.CONTAIN; }
            }
            else
            {
                if (notInInitialStages && Random.Range(0, 2) == 0) { platform.Obstacle = Element.CONTAIN; }
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
        float speed = Random.Range(flyingObstacleMoveSpeed / 2, flyingObstacleMoveSpeed);
        string reandomObstacle = flyingObstacleNames[Random.Range(0, flyingObstacleNames.Count)];
        Transform obstacle = PoolManager.Instance.GetFromPool(reandomObstacle).transform;
        flyingObstacles.Add(new PoolObj(reandomObstacle, obstacle.gameObject));

        // Randomly Selecting Target Positions
        if (Random.Range(0, 2) == 1)
            FlyToLeft();
        else
            FlyToRight();

        void FlyToLeft()
        {
            obstacle.transform.position = rightPos;
            obstacle.transform.eulerAngles = new Vector3(0, 180f, 0);
            obstacle.DOMove(leftPos, speed).OnComplete(FlyToRight);
        }

        void FlyToRight()
        {
            obstacle.transform.position = leftPos;
            obstacle.transform.eulerAngles = new Vector3(0, 0, 0);
            obstacle.DOMove(rightPos, speed).OnComplete(FlyToLeft);
        }
    }

    #endregion

    #region Utilities

    private void SetCoinOrObstacle(Platform platform)
    {
        if (Random.Range(0, 2) == 0)
            platform.Obstacle = Element.CONTAIN;
        else
            platform.Coins = Element.CONTAIN;
    }

    private PlatformType GetRandomPlatformType()
    {
        return Random.Range(0, 2) == 1 ? PlatformType.SINGLE : PlatformType.DOUBLE;
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

public enum PowerUp
{
    EXTRA_LIFE,
    JUMP_BOOST
}