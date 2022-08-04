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

        if (gameData.gameInitialized)
        {
            laodingScreen.gameObject.SetActive(false);
            popupsManager.Init(gameData, this);
            gameplayUiScreen.Init(gameData, popupsManager);
        }
        else
        {
            laodingScreen.Init(() =>
            {
                popupsManager.Init(gameData, this);
                gameplayUiScreen.Init(gameData, popupsManager);

            }, gameData);
        }
    }

    public void StartGame()
    {
        background.SetActive(false);
        gameManager.StartGame();
        gameplayUiScreen.Display();
    }

    public void GameEnd()
    {
        gameManager.EndGame();
        popupsManager.ShowGameEndingScreen();
    }

    #endregion

    #region Private Methods



    #endregion

}