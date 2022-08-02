using UnityEngine;
using Utilities.Audio;
using Utilities.Data;

public class GameManager : MonoBehaviour
{

    #region Public Attributes

    public GameData gameData;
    public LaodingScreen laodingScreen;
    public GameplayUIManager gameplayUiManager;
    public GameplayPopupsManager gameplayPopupsManager;

    #endregion

    #region Main Methods

    private void Start()
    {
        if(gameData.gameInitialized)
        {
            laodingScreen.gameObject.SetActive(false);
            gameplayUiManager.Init(gameData, gameplayPopupsManager);
            gameplayPopupsManager.Init(gameData, gameplayUiManager);
        }
        else
        {
            DataController.Instance.Sfx = 1;
            DataController.Instance.Music = 1;

            laodingScreen.Init(() =>
            {
                gameplayUiManager.Init(gameData, gameplayPopupsManager);
                gameplayPopupsManager.Init(gameData, gameplayUiManager);

            }, gameData);
        }

        gameData.sfxOn = DataController.Instance.Sfx == 1 ? true : false;
        gameData.musicOn = DataController.Instance.Music == 1 ? true : false;
    }

    #endregion
}
