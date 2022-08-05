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

    private int livesLeft;
    private GameData gameData;
    private GameManager gameManager;
    private PlayerController playerController;

    #endregion

    #region Public Methods

    public void Init(PlayerController _playerController, GameData _gameData, GameManager _gameManager)
    {
        gameData = _gameData;
        gameManager = _gameManager;
        playerController = _playerController;

        if (gameData.restartGame)
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

        livesLeft = 1;
        gameplayUiScreen.UpdateLivesUI(livesLeft);
    }

    public void AddRewardScores(bool _coinReward = false)
    {
        if (playerController.isInJumpBoost)
            return;

        if (_coinReward)
            gameplayUiScreen.IncrementCoinScores();
        else
            gameplayUiScreen.IncrementBoostScores();
    }

    public void AddLife()
    {
        livesLeft += 1;
        gameplayUiScreen.UpdateLivesUI(livesLeft);
    }

    public bool AllLivesEnded(bool _collidedWithDeadEnd)
    {
        if (_collidedWithDeadEnd)
            livesLeft = 0;
        else
            livesLeft -= 1;

        if(livesLeft <= 0)
        {
            gameplayUiScreen.UpdateLivesUI(0);
            return true;
        }
        gameplayUiScreen.UpdateLivesUI(livesLeft);
        return false;
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