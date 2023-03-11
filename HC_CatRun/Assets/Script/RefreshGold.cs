using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RefreshGold : MonoBehaviour
{
    [SerializeField] Text goldText;
    private void Start()
    {
        SaveManager.instance.playerData.stone_refreshed += refresh;
        refresh();
    }
    private void OnDisable()
    {
        SaveManager.instance.playerData.stone_refreshed -= refresh;
    }
    void refresh()
    {
        goldText.text = SaveManager.instance.playerData.stone.ToString();
    }
}