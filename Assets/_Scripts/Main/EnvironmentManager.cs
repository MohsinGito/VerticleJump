using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentManager : MonoBehaviour
{

    #region Public Attributes

    public int startingPatch;
    public float respositionSpeed;
    public float newEnvSpawnYPos;
    public Transform repositionEnv;
    public SpriteRenderer ground;
    public SpriteRenderer leftBg;
    public SpriteRenderer rightBg;

    #endregion

    #region Private Attributes

    private bool isFirstPatch;
    private float currentRepositionYPos;
    private Transform playerTransform;
    private GameStage currentStageInfo;
    private Vector3 nextPositionForRepositioning;

    private GameData gameData;
    private UIManager uiManager;
    private List<EnvironmentPatch> environmentPatches;

    #endregion

    #region Public Methods

    public void Init(GameStage _stageInfo, UIManager _uiManager, GameData _gameData)
    {
        gameData = _gameData;
        uiManager = _uiManager; 
        currentStageInfo = _stageInfo;
        ground.sprite = _stageInfo.groundSprite;
        leftBg.color = _stageInfo.backgroundColor;
        rightBg.color = _stageInfo.backgroundColor;
        currentRepositionYPos = newEnvSpawnYPos;
        nextPositionForRepositioning = Vector3.zero;
        environmentPatches = new List<EnvironmentPatch>();
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;

        isFirstPatch = true;
        leftBg.gameObject.SetActive(true);
        rightBg.gameObject.SetActive(true);

        for (int i = 0; i < startingPatch; i++)
        {
            if(i == 0)
                SpawnNewPatch(false);
            else
                SpawnNewPatch(true);
        }

        CheckForPlayerDistance();
    }

    public void SpawnNewPatch(bool canSpawnEnemies = true)
    {
        environmentPatches.Add(PoolManager.Instance.GetFromPool("Environment Patch").GetComponent<EnvironmentPatch>());

        if (environmentPatches.Count > 1)
        {
            environmentPatches[environmentPatches.Count - 1].transform.position =
                environmentPatches[environmentPatches.Count - 2].transform.position + new Vector3(0, newEnvSpawnYPos, 0);
        }

        environmentPatches[environmentPatches.Count - 1].Init(currentStageInfo, gameData, canSpawnEnemies);
    }

    public void RepositionEnvironment(float _repositionFactor)
    {
        uiManager.AddRewardScores();
        nextPositionForRepositioning += transform.position + new Vector3(0, _repositionFactor);
        repositionEnv.DOMove(nextPositionForRepositioning, respositionSpeed);
    }

    public void CheckForPlayerDistance()
    {
        StartCoroutine(StartChecking());
        IEnumerator StartChecking()
        {
            while(true)
            {
                if (playerTransform.position.y > currentRepositionYPos)
                {
                    RemoveAndAddNewPatch();
                    currentRepositionYPos = playerTransform.position.y + newEnvSpawnYPos;
                }
                yield return new WaitForFixedUpdate();
            }
        }
    }

    private void RemoveAndAddNewPatch()
    {
        if(isFirstPatch)
        {
            isFirstPatch = false;
            return;
        }

        PoolManager.Instance.ReturnToPool("Environment Patch", environmentPatches[0].gameObject);
        environmentPatches.RemoveAt(0);

        SpawnNewPatch();
    }

    #endregion

}