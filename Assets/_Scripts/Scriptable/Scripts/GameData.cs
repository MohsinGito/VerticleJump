using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameData", menuName = "SO/GameData")]
public class GameData : ScriptableObject
{

    #region Main Attributes

    public bool testBuild;

    [Header("-- In Game Data --")]
    public int sessionScores;
    public int gameHighScores;
    public List<GameStage> gameStages;
    public List<GameCharacter> gameCharacters;
    public List<GameEnemy> gameFlyingEnemies;
    public List<GameEnemy> gameCrawlingEnemies;

    [Header("-- Player Selection --")]
    [HideInInspector] public GameStage selectedStage;
    [HideInInspector] public GameCharacter selectedCharacter;

    [Header("-- Game Settings --")]
    public bool sfxOn;
    public bool musicOn;
    public bool gameInitialized;

    #endregion

}

[System.Serializable]
public struct GameStage
{
    public string stageName;
    public Sprite layerOne;
    public Sprite layerTwo;
    public Sprite layerThree;
    public Sprite groundSprite;
    public Sprite stageUISprite;
}

[System.Serializable]
public struct GameCharacter
{
    public string characterName;
    public Sprite idleSprite;
    public Sprite jumpSprite;
    public Sprite dieSprite;
}

[System.Serializable]
public struct GameEnemy
{
    public string enemyName;
    public List<Sprite> animationSprites;
}