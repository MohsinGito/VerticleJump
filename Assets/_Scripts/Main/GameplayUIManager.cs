using UnityEngine;

public class GameplayUIManager : MonoBehaviour
{

    #region Public Attributes



    #endregion

    #region Private Attributes

    private GameData gameData;
    private GameplayPopupsManager popupsManager;

    #endregion

    #region Public Methods

    public void Init(GameData _gameData, GameplayPopupsManager _popupsManager)
    {
        gameData = _gameData;
        popupsManager = _popupsManager;
    }

    public void StartGame()
    {
        Debug.Log("start game");
    }

    #endregion

    #region Private Methods

    #endregion

}