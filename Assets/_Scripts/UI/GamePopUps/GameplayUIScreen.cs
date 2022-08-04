using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameplayUIScreen : MonoBehaviour
{

    #region Main Attributes

    public ZoomInOutPopUp uiAnim;
    public TMP_Text scoresText;
    public Button pauseButton;
    
    private GameData gameData;
    private GameplayPopupsManager popupsManager;

    #endregion

    #region Public Methods

    public void Init(GameData _gameData, GameplayPopupsManager _popupsManager)
    {
        gameData = _gameData;
        popupsManager = _popupsManager;

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

    private void UpdateScoresUI()
    {
        StartCoroutine(UpdateUI());
        IEnumerator UpdateUI()
        {
            while(true)
            {
                scoresText.text = gameData.sessionScores.ToString();
                yield return null;  
            }
        }
    }

    #endregion

}