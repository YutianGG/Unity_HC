using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveManager 
{
    #region Singleton
    public static SaveManager instance
    {
        get
        {
            if (_instance == null)
                _instance = new SaveManager();
            return _instance;
        }
    }
    static SaveManager _instance;
    #endregion

    public PlayerData playerData = new PlayerData();
    public void Load()
    {
        string json = PlayerPrefs.GetString("PlayerData", "0");
        if (json == "0")
           playerData = new PlayerData(0);
        else
           playerData = JsonUtility.FromJson<PlayerData>(json);
    }
    public void Save()
    {
        //轉換為JSON
        string json = JsonUtility.ToJson(playerData, true);
        PlayerPrefs.SetString("PlayerData", json);
    }
}

[System.Serializable]
/// <summary>
/// 玩家資料
/// </summary>
public struct PlayerData
{
    //最高分
    public int highScore
    {
        get { return _highScore; }
        set
        {
            _highScore = value;
            if (score_refreshed != null)
                score_refreshed.Invoke();
        }
    }
    public System.Action score_refreshed;
    [SerializeField] public int _highScore;
    //金幣數
    public int stone
    {
        get { return _stone; }
        set
        {
            _stone = value;
            if (stone_refreshed != null)
                stone_refreshed.Invoke();
        }
    }
    [SerializeField] public int _stone;

    public System.Action stone_refreshed;

    public PlayerData(int x)
    {
        _highScore = 0;
        _stone = 0;
        score_refreshed = null;
        stone_refreshed = null;
    }
}