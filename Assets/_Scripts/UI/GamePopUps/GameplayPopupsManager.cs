using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class GameplayPopupsManager : MonoBehaviour
{

    #region Public Attributes

    public GameObject BG;
    public TMP_Text gameScoresText;

    [Header("Game PopUps")]
    public GamePopUp stageSelectionPopUp;
    public GamePopUp caharacterSelectionPopUp;
    public GamePopUp gameEndPopUp;
    public GamePopUp settingsPopUp;
    public GamePopUp gamePausePopUp;

    #endregion

    #region Private Attributes

    private GameData gameData;
    private UIManager gameplayUiManager;
    private List<GamePopUp> gamePopUpList;

    #endregion

    #region Initializing Methods

    public void Init(GameData _gameData, UIManager _uiManager)
    {
        gameData = _gameData;
        gameplayUiManager = _uiManager;

        stageSelectionPopUp.Init(gameData);
        caharacterSelectionPopUp.Init(gameData);
        gameEndPopUp.Init(gameData);
        settingsPopUp.Init(gameData);
        gamePausePopUp.Init(gameData);

        gamePopUpList = new List<GamePopUp>();
        gamePopUpList.Add(stageSelectionPopUp);
        gamePopUpList.Add(caharacterSelectionPopUp);
        gamePopUpList.Add(gameEndPopUp);
        gamePopUpList.Add(settingsPopUp);
        gamePopUpList.Add(gamePausePopUp);

        gameScoresText.text = _gameData.gameEarnedScores.ToString();
    }

    #endregion

    #region UI PopUps Displaying

    public void ShowStageSelectionScreen()
    {
        EnablePopUp(stageSelectionPopUp);
        stageSelectionPopUp.SetAction(ShowCharacterSelectionScreen);
    }

    public void ShowCharacterSelectionScreen()
    {
        EnablePopUp(caharacterSelectionPopUp);
        caharacterSelectionPopUp.SetAction( () =>
        {
            gameScoresText.transform.parent.gameObject.SetActive(false);
            caharacterSelectionPopUp.Hide(); gameplayUiManager.StartGame();
            DOVirtual.DelayedCall(0.25f, delegate { EnablePopUp(null); });
        });
    }

    public void ShowSettingsScreen()
    {
        EnablePopUp(settingsPopUp);
        settingsPopUp.SetAction(() =>
        {
            settingsPopUp.Hide();
            DOVirtual.DelayedCall(0.25f, delegate { EnablePopUp(null); });
        });
    }

    public void ShowPauseScreen()
    {
        EnablePopUp(gamePausePopUp);
        gamePausePopUp.SetAction(() =>
        {
            gamePausePopUp.Hide();
            DOVirtual.DelayedCall(0.25f, delegate { EnablePopUp(null); });
        });
    }

    public void ShowGameEndingScreen()
    {
        EnablePopUp(gameEndPopUp);
        gameEndPopUp.SetAction(() =>
        {
            gameEndPopUp.Hide();
            DOVirtual.DelayedCall(0.25f, delegate { EnablePopUp(null); });
        });
    }

    #endregion

    #region Private Methods

    private void EnablePopUp(GamePopUp _popUpToEnable)
    {
        if(_popUpToEnable == null)
        {
            foreach (GamePopUp popUp in gamePopUpList)
                popUp.gameObject.SetActive(false);

            return;
        }

        foreach(GamePopUp popUp in gamePopUpList)
        {
            if (popUp.name == _popUpToEnable.name)
            {
                popUp.gameObject.SetActive(true);
                popUp.Display();
            }
            else
            {
                popUp.gameObject.SetActive(false);
            }
        }
    }

    #endregion

}