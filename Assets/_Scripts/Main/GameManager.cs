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
        gameData.CheckGameUnlockedElements();
        gameData.gameEarnedScores = DataController.Instance.Scores;

        // Setting Up Game Volume First
        if (!gameData.gameInitialized)
        {
            DataController.Instance.Sfx = 1;
            DataController.Instance.Music = 1;
        }

        //gameData.sfxOn = DataController.Instance.Sfx == 1 ? true : false;
        //gameData.musicOn = DataController.Instance.Music == 1 ? true : false;

        // Initializing Main Scripts
        gameplayUiManager.Init(gameData, this);
    }

    public void StartGame()
    {
        AudioController.Instance.PlayAudio(AudioName.BACKGROUND);
        environmentManager.Init(gameData.selectedStage, gameplayUiManager, gameData);
        playerController.Init(gameData.selectedCharacter, gameplayUiManager, environmentManager);
    } 

    public void EndGame()
    {
        gameData.gameEarnedScores += gameData.sessionScores;
        DataController.Instance.Scores = gameData.gameEarnedScores;
    }

    #endregion
}
