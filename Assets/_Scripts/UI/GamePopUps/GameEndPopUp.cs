using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameEndPopUp : GamePopUp
{
    #region Public Attributes

    [Header("Puase PopUp Elements")]
    public ZoomInOutPopUp popUpAnim;
    public TMP_Text scoresText;
    public Button restartButton;
    public Button menuButton;

    #endregion

    #region Private Attributes

    private GameData gameData;
    private UnityAction callbackEvent;

    #endregion

    #region Public Methods

    public override void Init(GameData _gameData)
    {
        gameData = _gameData;

        menuButton.onClick.AddListener(MoveToMainMenu);
        restartButton.onClick.AddListener(RestartGame);
    }

    public override void Display()
    {
        scoresText.text = gameData.sessionScores + "";
        popUpAnim.Animate(true);
    }

    public override void Hide()
    {
        popUpAnim.Animate(false);
    }

    public override void SetAction(UnityAction _callback)
    {
        callbackEvent = _callback;
    }

    #endregion

    #region Private Methods

    private void MoveToMainMenu()
    {
        SceneManager.LoadScene("Gameplay");
    }

    private void RestartGame()
    {

    }

    #endregion
}
