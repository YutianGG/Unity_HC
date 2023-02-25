using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RefreshHighScore : MonoBehaviour
{
    [SerializeField] Text highScoreText;
    private void Start()
    {
        SaveManager.instance.playerData.score_refreshed += refresh;
        refresh();
    }
    private void OnDisable()
    {
        SaveManager.instance.playerData.score_refreshed -= refresh;
    }
    void refresh()
    {
        highScoreText.text = SaveManager.instance.playerData.highScore.ToString();
    }
}
