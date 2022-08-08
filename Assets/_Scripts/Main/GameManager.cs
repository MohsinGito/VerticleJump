using System.Collections;
using RestManager;
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
        ResetGame();

        // Setting Up Game Volume First
        if (!gameData.gameInitialized)
        {
            DataController.Instance.Sfx = 1;
            DataController.Instance.Music = 1;
        }

        // Initializing Main Scripts
        gameplayUiManager.Init(gameData, this);
    }

    private void ResetGame()
    {
        if(gameData.resetGame)
        {
            gameData.gameEarnedScores = 0;
            DataController.Instance.Scores = 0;
            gameData.gameInitialized = false;
        }
        else
        {
            gameData.gameEarnedScores = DataController.Instance.Scores;
        }
        gameData.CheckGameUnlockedElements();
    }

    public void StartGame()
    {
        AudioController.Instance.PlayAudio(AudioName.BACKGROUND);
        environmentManager.Init(gameData.selectedStage, gameplayUiManager, playerController, gameData);
        playerController.Init(gameData.selectedCharacter, gameplayUiManager, environmentManager);
    } 

    public void EndGame()
    {
        GameServer.Instance.InitializeAdd(1);
        GameServer.Instance.SendScoresInfo(gameData.sessionScores);

        gameData.gameEarnedScores += gameData.sessionScores;
        DataController.Instance.Scores = gameData.gameEarnedScores;
    }

    #endregion
}
