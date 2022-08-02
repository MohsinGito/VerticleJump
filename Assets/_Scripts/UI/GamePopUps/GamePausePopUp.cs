using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class GamePausePopUp : GamePopUp
{

    #region Public Attributes

    [Header("Puase PopUp Elements")]
    public ZoomInOutPopUp popUpAnim;
    public TMP_Text pauseText;
    public Button cancelButton;
    public Button restartButton;
    public Button menuButton;
    public Button yesButton;
    public Button noButton;

    #endregion

    #region Private Attributes

    private GameData gameData;
    private UnityAction callbackEvent;

    #endregion

    #region Public Methods

    public override void Init(GameData _gameData)
    {
        gameData = _gameData;

        noButton.onClick.AddListener(Cancel);
        yesButton.onClick.AddListener(MoveToMainMenu);
        cancelButton.onClick.AddListener(Cancel);
        menuButton.onClick.AddListener(SetupConfirmationPopUp);
        restartButton.onClick.AddListener(RestartGame);
    }

    public override void Display()
    {
        SetUpPausePopUp();
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

    private void SetUpPausePopUp()
    {
        pauseText.text = "Pause";
        cancelButton.gameObject.SetActive(true);
        restartButton.gameObject.SetActive(true);
        menuButton.gameObject.SetActive(true);
        yesButton.gameObject.SetActive(false);
        noButton.gameObject.SetActive(false);
    }

    private void SetupConfirmationPopUp()
    {
        pauseText.text = "Exit To Menu?";
        cancelButton.gameObject.SetActive(false);
        restartButton.gameObject.SetActive(false);
        menuButton.gameObject.SetActive(false);
        yesButton.gameObject.SetActive(true);
        noButton.gameObject.SetActive(true);
    }

    private void Cancel()
    {
        callbackEvent?.Invoke();
    }

    private void MoveToMainMenu()
    {

    }

    private void RestartGame()
    {

    }

    #endregion

}
