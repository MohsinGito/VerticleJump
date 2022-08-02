using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class CharacterSelectionPopUp : GamePopUp
{
    #region Public Attributes

    [Header("Puase PopUp Elements")]
    public ZoomInOutPopUp popUpAnim;
    public Image displayImage;
    public TMP_Text displayName;
    public Button leftArrowButton;
    public Button rightArrowButton;
    public Button selectCharacterButton;

    #endregion

    #region Private Attributes

    private GameData gameData;
    private UnityAction callbackEvent;
    private int currentCharacterIndex;

    #endregion

    #region Public Methods

    public override void Init(GameData _gameData)
    {
        gameData = _gameData;
        currentCharacterIndex = 0;

        leftArrowButton.onClick.AddListener(MoveToLeft);
        rightArrowButton.onClick.AddListener(MoveToRight);
        selectCharacterButton.onClick.AddListener(SelectCharacter);

        SetUpCharacter();
    }

    public override void Display()
    {
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

    private void MoveToLeft()
    {
        if (currentCharacterIndex == 0)
            return;

        currentCharacterIndex -= 1;
        SetUpCharacter();
    }

    private void MoveToRight()
    {
        if (currentCharacterIndex == gameData.gameCharacters.Count - 1)
            return;

        currentCharacterIndex += 1;
        SetUpCharacter();
    }

    private void SetUpCharacter()
    {
        displayName.text = gameData.gameCharacters[currentCharacterIndex].characterName;
        displayImage.sprite = gameData.gameCharacters[currentCharacterIndex].idleSprite;
    }

    private void SelectCharacter()
    {
        gameData.selectedCharacter = gameData.gameCharacters[currentCharacterIndex];
        callbackEvent?.Invoke();
    }

    #endregion
}
