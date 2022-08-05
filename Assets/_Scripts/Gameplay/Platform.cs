using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{

    #region Public Attributes

    [Header("Left Side Platform Info")]
    public SpriteRenderer leftPlatform;
    public Transform leftSpawnPos;

    [Header("Right Side Platform Info")]
    public SpriteRenderer rightPlatform;
    public Transform rightSpawnPos;

    [Header("Other Info")]
    public List<string> groundEnemies;
    public Vector2 coinsLimit;

    #endregion

    #region Private Attributes

    private bool powerUpAssigned;
    private bool leftSideOccupied;
    private bool rightSideOccupied;
    private PlatformType currentType;
    private List<PoolObj> spawnedElements = new List<PoolObj>();

    #endregion

    #region Public Properties

    public Sprite DisplaySprite
    {
        set
        {
            leftPlatform.sprite = value;
            rightPlatform.sprite = value;
        }
    }

    public PlatformType State
    {
        set
        {
            currentType = value;
            switch (currentType)
            {
                case PlatformType.SINGLE:
                    if(Random.Range(0, 2) == 1)
                    {
                        leftSideOccupied = false;
                        rightSideOccupied = true;
                        leftPlatform.gameObject.SetActive(true);
                    }
                    else
                    {
                        leftSideOccupied = true;
                        rightSideOccupied = false;
                        rightPlatform.gameObject.SetActive(true);
                    }
                    break;
                case PlatformType.DOUBLE:
                    leftSideOccupied = false;
                    rightSideOccupied = false;
                    leftPlatform.gameObject.SetActive(true);
                    rightPlatform.gameObject.SetActive(true);
                    break;
            }
        }
        get
        {
            return currentType;
        }
    }

    public Element Coins
    {
        set
        {
            if (value == Element.CONTAIN)
            {
                if (!leftSideOccupied)
                {
                    leftSideOccupied = true;
                    AddNewElementToPlatform("Coin", leftSpawnPos);
                }
                else if (!rightSideOccupied)
                {
                    rightSideOccupied = true;
                    AddNewElementToPlatform("Coin", rightSpawnPos);
                }
            }
        }
    }

    public Element Obstacle
    {
        set
        {
            if(value == Element.CONTAIN)
            {
                if (!leftSideOccupied)
                {
                    leftSideOccupied = true;
                    AddNewElementToPlatform(groundEnemies
                        [Random.Range(0, groundEnemies.Count)], leftSpawnPos);
                }
                else if (!rightSideOccupied)
                {
                    rightSideOccupied = true;
                    AddNewElementToPlatform(groundEnemies
                        [Random.Range(0, groundEnemies.Count)], rightSpawnPos);
                }
            }
        }
    }

    public Element ExtraLife
    {
        set
        {
            if (value == Element.CONTAIN)
            {
                if (!leftSideOccupied)
                {
                    leftSideOccupied = true;
                    powerUpAssigned = true;
                    AddNewElementToPlatform("Extra Life", leftSpawnPos);
                }
                else if (!rightSideOccupied)
                {
                    rightSideOccupied = true;
                    powerUpAssigned = true;
                    AddNewElementToPlatform("Extra Life", rightSpawnPos);
                }
            }
        }
    }

    public Element JumpBoost
    {
        set
        {
            if (value == Element.CONTAIN)
            {
                if (!leftSideOccupied)
                {
                    leftSideOccupied = true;
                    powerUpAssigned = true;
                    AddNewElementToPlatform("Jumping Spring", leftSpawnPos);
                }
                else if (!rightSideOccupied)
                {
                    rightSideOccupied = true;
                    powerUpAssigned = true;
                    AddNewElementToPlatform("Jumping Spring", rightSpawnPos);
                }
            }
        }
    }

    public bool PowerUpAssigned
    {
        get { return powerUpAssigned; }
    }

    #endregion

    #region Main Methods

    public void ResetPlatForm()
    {
        foreach (PoolObj obj in spawnedElements)
        {
            obj.Prefab.transform.position = Vector3.zero;
            PoolManager.Instance.ReturnToPool(obj.Tag, obj.Prefab);
        }

        spawnedElements.Clear();
        powerUpAssigned = false;
        leftSideOccupied = true;
        rightSideOccupied = true;
        leftPlatform.gameObject.SetActive(false);
        rightPlatform.gameObject.SetActive(false);
    }

    private void AddNewElementToPlatform(string tag, Transform parent)
    {
        GameObject obj = PoolManager.Instance.GetFromPool(tag);
        spawnedElements.Add(new PoolObj(tag, obj));
        obj.transform.position = parent.position;
        obj.transform.localScale = Vector3.one;
    }

    #endregion

}

public enum PlatformType
{
    SINGLE,
    DOUBLE
}

public enum Element
{
    IDLE,
    CONTAIN
}