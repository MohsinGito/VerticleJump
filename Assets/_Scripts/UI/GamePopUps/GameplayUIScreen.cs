using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameplayUIScreen : MonoBehaviour
{

    #region Main Attributes

    public ZoomInOutPopUp uiAnim;
    public TMP_Text scoresText;
    public TMP_Text livesText;
    public Button pauseButton;

    #endregion

    #region Private Attributes

    private int currentScores;
    private GameData gameData;
    private GameplayPopupsManager popupsManager;

    #endregion

    #region Public Methods

    public void Init(GameData _gameData, GameplayPopupsManager _popupsManager)
    {
        gameData = _gameData;
        popupsManager = _popupsManager;

        gameData.sessionScores = 0;
        gameData.gameEarnedScores = PlayerPrefs.GetInt("Scores");
        scoresText.text = gameData.sessionScores.ToString();
        pauseButton.onClick.AddListener(popupsManager.ShowPauseScreen);

        UpdateScoresUI();
    }

    public void Display()
    {
        uiAnim.Animate(true);
    }

    public void Hide()
    {
        uiAnim.Animate(false);
    }

    public void IncrementBoostScores()
    {
        gameData.sessionScores += gameData.scoresBoost;
    }

    public void IncrementCoinScores()
    {
        gameData.sessionScores += gameData.coinsScores;
    }

    public void UpdateLivesUI(int lives)
    {
        livesText.text = "Lives Left : " + lives;
    }

    private void UpdateScoresUI()
    {
        StartCoroutine(UpdateUI());
        IEnumerator UpdateUI()
        {
            while(true)
            {
                if(currentScores < gameData.sessionScores)
                {
                    currentScores += 1;
                    scoresText.text = currentScores.ToString();
                }
                yield return new WaitForSeconds(0.01f);  
            }
        }
    }

    #endregion

}