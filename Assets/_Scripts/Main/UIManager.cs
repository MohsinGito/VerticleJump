using DG.Tweening;
using UnityEngine;

public class UIManager : MonoBehaviour
{

    #region Public Attributes

    public GameObject background;

    [Header("UI Managing Scripts")]
    public GameplayUIScreen gameplayUiScreen;
    public GameplayPopupsManager popupsManager;
    public LaodingScreen laodingScreen;

    #endregion

    #region Private Attributes

    private GameData gameData;
    private GameManager gameManager;

    #endregion

    #region Public Methods

    public void Init(GameData _gameData, GameManager _gameManager)
    {
        gameData = _gameData;
        gameManager = _gameManager;

        if(gameData.restartGame)
        {
            laodingScreen.gameObject.SetActive(false);
            popupsManager.Init(gameData, this);
            gameplayUiScreen.Init(gameData, popupsManager);
            gameData.restartGame = false;
            StartGame();

            return;
        }

        if (gameData.gameInitialized)
        {
            laodingScreen.gameObject.SetActive(false);
            popupsManager.Init(gameData, this);
            gameplayUiScreen.Init(gameData, popupsManager);

            popupsManager.ShowStageSelectionScreen();
        }
        else
        {
            laodingScreen.Init(() =>
            {
                popupsManager.Init(gameData, this);
                gameplayUiScreen.Init(gameData, popupsManager);

                popupsManager.ShowStageSelectionScreen();

            }, gameData);
        }
    }

    public void AddRewardScores(bool _coinReward = false)
    {
        if (_coinReward)
            gameplayUiScreen.IncrementCoinScores();
        else
            gameplayUiScreen.IncrementBoostScores();
    }

    public void StartGame()
    {
        background.SetActive(false);
        gameManager.StartGame();
        gameplayUiScreen.Display();
    }

    public void GameEnd()
    {
        DOTween.KillAll();
        gameManager.EndGame();
        popupsManager.ShowGameEndingScreen();
    }

    #endregion

    #region Private Methods



    #endregion

}