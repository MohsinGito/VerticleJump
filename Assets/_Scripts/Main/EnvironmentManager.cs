using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentManager : MonoBehaviour
{

    #region Public Attributes

    public int startingPatch;
    public float newEnvSpawnYPos;
    public float repositionSpeed;
    public float evnRepositionStartY;
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

    private GameData gameData;
    private UIManager uiManager;
    private PlayerController playerController;
    private List<EnvironmentPatch> environmentPatches;

    #endregion

    #region Public Methods

    public void Init(GameStage _stageInfo, UIManager _uiManager, PlayerController _playerController, GameData _gameData)
    {
        gameData = _gameData;
        uiManager = _uiManager;
        currentStageInfo = _stageInfo;
        playerController = _playerController;
        ground.sprite = _stageInfo.groundSprite;
        leftBg.color = _stageInfo.backgroundColor;
        rightBg.color = _stageInfo.backgroundColor;
        playerTransform = _playerController.transform;
        currentRepositionYPos = newEnvSpawnYPos;
        environmentPatches = new List<EnvironmentPatch>();

        isFirstPatch = true;
        leftBg.gameObject.SetActive(true);
        rightBg.gameObject.SetActive(true);

        for (int i = 0; i < startingPatch; i++)
        {
            if(i < 2)
                SpawnNewPatch(false);
            else
                SpawnNewPatch(true);
        }

        CheckForPlayerDistance();
    }

    public void SpawnNewPatch(bool _notInInitialStages = true)
    {
        environmentPatches.Add(PoolManager.Instance.GetFromPool("Environment Patch").GetComponent<EnvironmentPatch>());

        if (environmentPatches.Count > 1)
        {
            environmentPatches[environmentPatches.Count - 1].transform.position =
                environmentPatches[environmentPatches.Count - 2].transform.position + new Vector3(0, newEnvSpawnYPos, 0);
        }

        environmentPatches[environmentPatches.Count - 1].Init(currentStageInfo, gameData, _notInInitialStages);

        if(_notInInitialStages)
        {
            if(Random.Range(0, 2) == 1)
                environmentPatches[environmentPatches.Count - 1].SetPowerUpOnPatch(PowerUp.EXTRA_LIFE, 1);

            if (Random.Range(0, 2) == 1)
                environmentPatches[environmentPatches.Count - 1].SetPowerUpOnPatch(PowerUp.JUMP_BOOST, 1);
        }
    }

    public void CheckForPlayerDistance()
    {
        StartCoroutine(StartChecking());
        IEnumerator StartChecking()
        {
            while (true)
            {
                if (playerTransform.position.y > currentRepositionYPos)
                {
                    RemoveAndAddNewPatch();
                    currentRepositionYPos = playerTransform.position.y + newEnvSpawnYPos;
                }

                RepositionEnvironment();
                yield return new WaitForFixedUpdate();
            }
        }
    }

    private void RepositionEnvironment()
    {
        if(playerTransform.position.y > evnRepositionStartY)
        {
            if (!playerController.isInJumpBoost)
                uiManager.AddRewardScores();
            evnRepositionStartY = playerTransform.position.y;
        }

        if(repositionEnv.position.y != evnRepositionStartY)
            repositionEnv.DOMove(new Vector3(0, evnRepositionStartY, 0), repositionSpeed / 100);
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