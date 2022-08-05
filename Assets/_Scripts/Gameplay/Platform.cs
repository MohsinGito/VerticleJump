using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{

    #region Public Attributes

    [Header("Left Side Platform Info")]
    public SpriteRenderer leftPlatform;
    public Transform leftCoinPos;
    public Transform leftObstaclePos;

    [Header("Right Side Platform Info")]
    public SpriteRenderer rightPlatform;
    public Transform rightCoinPos;
    public Transform rightObstaclePos;

    [Header("Other Info")]
    public List<string> groundEnemies;
    public Vector2 coinsLimit;

    #endregion

    #region Private Attributes

    private bool leftSideOccupied;
    private bool rightSideOccupied;
    private PlatformType currentType;
    private List<PoolObj> enemySpawned = new List<PoolObj>();
    private List<PoolObj> coinsSpawned = new List<PoolObj>();

    #endregion

    #region Public Properties

    public Sprite DisplaySprite
    {
        set
        {
            leftPlatform.sprite = value;
            rightPlatform.sprite = value;
        }
    }

    public PlatformType State
    {
        set
        {
            currentType = value;
            switch (currentType)
            {
                case PlatformType.SINGLE:
                    if(Random.Range(0, 2) == 1)
                    {
                        leftSideOccupied = false;
                        rightSideOccupied = true;
                        leftPlatform.gameObject.SetActive(true);
                    }
                    else
                    {
                        leftSideOccupied = true;
                        rightSideOccupied = false;
                        rightPlatform.gameObject.SetActive(true);
                    }
                    break;
                case PlatformType.DOUBLE:
                    leftSideOccupied = false;
                    rightSideOccupied = false;
                    leftPlatform.gameObject.SetActive(true);
                    rightPlatform.gameObject.SetActive(true);
                    break;
            }
        }
        get
        {
            return currentType;
        }
    }

    public Element Coins
    {
        set
        {
            if (value == Element.CONTAIN)
            {
                if (!leftSideOccupied)
                {
                    leftSideOccupied = true;
                    SpawnCoin(leftCoinPos);
                }
                else if (!rightSideOccupied)
                {
                    rightSideOccupied = true;
                    SpawnCoin(rightCoinPos);
                }
            }
        }
    }

    public Element Obstacle
    {
        set
        {
            if(value == Element.CONTAIN)
            {
                if (!leftSideOccupied)
                {
                    leftSideOccupied = true;
                    SpawnObstacle(leftObstaclePos);
                }
                else if (!rightSideOccupied)
                {
                    rightSideOccupied = true;
                    SpawnObstacle(rightObstaclePos);
                }
            }
        }
    }

    #endregion

    #region Main Methods

    public void ResetPlatForm()
    {
        foreach (PoolObj obj in enemySpawned)
        {
            obj.Prefab.transform.position = Vector3.zero;
            PoolManager.Instance.ReturnToPool(obj.Tag, obj.Prefab);
        }

        foreach (PoolObj obj in coinsSpawned)
        {
            obj.Prefab.transform.position = Vector3.zero;
            PoolManager.Instance.ReturnToPool(obj.Tag, obj.Prefab);
        }

        enemySpawned.Clear();
        coinsSpawned.Clear();
        leftSideOccupied = true;
        rightSideOccupied = true;
        leftPlatform.gameObject.SetActive(false);
        rightPlatform.gameObject.SetActive(false);
    }

    private void SpawnObstacle(Transform _spawnPos)
    {
        string randomEnemy = groundEnemies[Random.Range(0, groundEnemies.Count)];
        GameObject newEnemy = PoolManager.Instance.GetFromPool(randomEnemy);
        enemySpawned.Add(new PoolObj(randomEnemy, newEnemy));
        newEnemy.transform.parent = _spawnPos;
        newEnemy.transform.position = _spawnPos.position;
        newEnemy.transform.localScale = Vector2.one;
    }

    private void SpawnCoin(Transform _spawnPos)
    {
        GameObject coin = PoolManager.Instance.GetFromPool("Coin");
        coinsSpawned.Add(new PoolObj("Coin", coin));
        coin.transform.parent = _spawnPos;
        coin.transform.position = _spawnPos.position;
    }

    #endregion

}

public enum PlatformType
{
    SINGLE,
    DOUBLE
}

public enum Element
{
    IDLE,
    CONTAIN
}