using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VFXManager : MonoBehaviour
{

    #region Singleton

    public static VFXManager Instance;
    private void Awake()
    {
        if (!Instance)
            Instance = this;
        else
            Destroy(gameObject);
    }

    #endregion

    #region Main Attributes

    public List<GameVFX> gameVFXes;
    private Dictionary<string, GameObject> gameVFXs;

    #endregion

    #region Main Methods

    private void Start()
    {
        gameVFXs = new Dictionary<string, GameObject>();
        foreach (GameVFX gameVFX in gameVFXes)
        {
            gameVFXs[gameVFX.Name] = gameVFX.vfx;
        }
    }

    public void DisplayVFX(string _vfxName, Vector3 vfxPos)
    {
        gameVFXs[_vfxName].transform.position = vfxPos;
        gameVFXs[_vfxName].SetActive(false);
        gameVFXs[_vfxName].SetActive(true);
    }

    #endregion

}

[System.Serializable]
public struct GameVFX
{
    public string Name;
    public GameObject vfx;
}