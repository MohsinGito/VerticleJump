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

    private float currentRepositionYPos;
    private GameStage currentStageInfo;
    private Vector3 nextPositionForRepositioning;
    private List<EnvironmentPatch> environmentPatches;

    #endregion

    #region Public Methods

    public void Init(GameStage _stageInfo)
    {
        currentStageInfo = _stageInfo;
        ground.sprite = _stageInfo.groundSprite;
        leftBg.color = _stageInfo.backgroundColor;
        rightBg.color = _stageInfo.backgroundColor;
        currentRepositionYPos = newEnvSpawnYPos;
        nextPositionForRepositioning = Vector3.zero;
        environmentPatches = new List<EnvironmentPatch>();

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
        environmentPatches[environmentPatches.Count - 1].Init(currentStageInfo, canSpawnEnemies);

        if (environmentPatches.Count > 1)
        {
            environmentPatches[environmentPatches.Count - 1].transform.position =
                environmentPatches[environmentPatches.Count - 2].nextPatchPos.position;
        }
    }

    public void RepositionEnvironment()
    {
        nextPositionForRepositioning += transform.position + new Vector3(0, respositionFactor);
        repositionEnv.DOMove(nextPositionForRepositioning, respositionSpeed);

        if(repositionEnv.position.y >= currentRepositionYPos)
        {
            environmentPatches[0].ResetPatch();
            environmentPatches[0].gameObject.SetActive(false);
           // PoolManager.Instance.ReturnToPool("Environment Patch", environmentPatches[0].gameObject);
            environmentPatches.RemoveAt(0);
            currentRepositionYPos += newEnvSpawnYPos;

            SpawnNewPatch();
        }
    }

    #endregion

}