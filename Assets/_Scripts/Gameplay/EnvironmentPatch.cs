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
    public Transform nextPatchPos;
    public Vector2 xMinMax;

    [Header("Display Sprites")]
    public SpriteRenderer layer1;
    public SpriteRenderer layer2;
    public SpriteRenderer layer3;

    #endregion

    #region Private Attributes

    private bool canSpawnEnemies;
    private bool canSpawnFlyingEnemies;
    private GameStage currentStageInfo;
    private List<Platform> platforms;

    #endregion

    #region Public Methods

    public void Init(GameStage _stageInfo, bool _canSpawnEnemies = true)
    {
        ResetPatch();

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
            return;
        }

        foreach (Platform platform in platforms)
        {
            PoolManager.Instance.ReturnToPool("Platform", platform.gameObject);
        }
        platforms.Clear();
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
                    SpawnObstacle(Random.Range(0, 2) == 0, i);
                else
                    SpawnNewPlatform(startSpawnY + ((maxPlatforms - i - 1) * platformOffset));
            }
        }
    }

    private void SpawnObstacle(bool _spawnGroundObstacle, int _index)
    {
        if (!canSpawnEnemies)
            return;

        if (_spawnGroundObstacle || !canSpawnFlyingEnemies)
        {
            SpawnNewPlatform(startSpawnY + ((maxPlatforms - _index - 1) * platformOffset), false);
            platforms[platforms.Count - 1].Obstacle = Element.CONTAIN;
            platforms[platforms.Count - 1].Coins = Element.CONTAIN;
        }
        else
        {

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

    #endregion

}