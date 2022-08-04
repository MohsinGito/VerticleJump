using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentManager : MonoBehaviour
{

    #region Public Attributes

    public int startingPatch;
    public float respositionFactor;
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
    private GameStage currentStageInfo;
    private Vector3 nextPositionForRepositioning;

    private UIManager uiManager;
    [SerializeField] private List<EnvironmentPatch> environmentPatches;

    #endregion

    #region Public Methods

    public void Init(GameStage _stageInfo, UIManager _uiManager)
    {
        uiManager = _uiManager; 
        currentStageInfo = _stageInfo;
        ground.sprite = _stageInfo.groundSprite;
        leftBg.color = _stageInfo.backgroundColor;
        rightBg.color = _stageInfo.backgroundColor;
        currentRepositionYPos = newEnvSpawnYPos;
        nextPositionForRepositioning = Vector3.zero;
        environmentPatches = new List<EnvironmentPatch>();

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
    }

    public void SpawnNewPatch(bool canSpawnEnemies = true)
    {
        environmentPatches.Add(PoolManager.Instance.GetFromPool("Environment Patch").GetComponent<EnvironmentPatch>());

        if (environmentPatches.Count > 1)
        {
            environmentPatches[environmentPatches.Count - 1].transform.position =
                environmentPatches[environmentPatches.Count - 2].nextPatchPos.position;
        }

        environmentPatches[environmentPatches.Count - 1].Init(currentStageInfo, canSpawnEnemies);
    }

    public void RepositionEnvironment()
    {
        uiManager.AddRewardScores();
        nextPositionForRepositioning += transform.position + new Vector3(0, respositionFactor);
        repositionEnv.DOMove(nextPositionForRepositioning, respositionSpeed);
    }

    public void RemoveAndSpawnNewPatch()
    {
        if(isFirstPatch)
        {
            isFirstPatch = false;
            return;
        }
        environmentPatches[0].gameObject.SetActive(false);
        PoolManager.Instance.ReturnToPool("Environment Patch", environmentPatches[0].gameObject);
        environmentPatches.RemoveAt(0);
        currentRepositionYPos += newEnvSpawnYPos;

        SpawnNewPatch();
    }

    #endregion

}