using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayManager : MonoBehaviour
{

    #region Private Attribtues

    private GameData gameData;

    #endregion

    #region Public Methods

    public void Init(GameData _gameData)
    {
        gameData = _gameData;
    }

    #endregion

    #region Game Controlling

    public void StartGame()
    {

    }

    public void EndGame()
    {

    }

    public void RestartGame()
    {

    }

    #endregion

}
