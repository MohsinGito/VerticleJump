using System.Collections;
using UnityEngine;
using Utilities.Audio;
using Utilities.Data;
public class GameManager : MonoBehaviour
{

    #region Public Attributes

    public GameData gameData;

    [Header("Main Scripts")]
    public PlayerController playerController;
    public EnvironmentManager environmentManager;
    public UIManager gameplayUiManager;

    #endregion

    #region Main Methods

    private void Start()
    {
        // Setting Up Game Volume First
        if (!gameData.gameInitialized)
        {
            DataController.Instance.Sfx = 1;
            DataController.Instance.Music = 1;
        }

        gameData.sfxOn = DataController.Instance.Sfx == 1 ? true : false;
        gameData.musicOn = DataController.Instance.Music == 1 ? true : false;

        // Initializing Main Scripts
        gameplayUiManager.Init(gameData, this);
    }

    public void StartGame()
    {
        StartCoroutine("IncreaseScores");
        environmentManager.Init(gameData.selectedStage);
        playerController.Init(gameData.selectedCharacter, gameplayUiManager, environmentManager);
    } 

    public void EndGame()
    {
        StopCoroutine("IncreaseScores");
    }

    IEnumerator IncreaseScores()
    {
        gameData.sessionScores = 0;
        while (true)
        {
            yield return new WaitForSeconds(0.05f);
            gameData.sessionScores += 1;
        }
    }

    #endregion
}
