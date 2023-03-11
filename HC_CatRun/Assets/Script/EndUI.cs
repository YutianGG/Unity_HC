using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class EndUI : Windows<EndUI>
{
    [SerializeField] Text scoreText;
    [SerializeField] Text highscoreText;
    [SerializeField] Text stoneGet;
    [SerializeField] Animator ani;
    bool hideNewScore = false;
    int stoneHold = 0;

    protected override void Start()
    {
        base.Start();
        stoneHold = SaveManager.instance.playerData.stone;
    }

    public override void Open()
    {
        base.Open();
        Cursor.lockState = CursorLockMode.None;
        scoreText.text = Mathf.Round(PlayerControl.instance.score).ToString();
        if(PlayerControl.instance.score < SaveManager.instance.playerData.highScore)
            highscoreText.text = Mathf.Round(SaveManager.instance.playerData.highScore).ToString("F0");
        else
        {
            //新紀錄
            SaveManager.instance.playerData.highScore = (int)PlayerControl.instance.score;
            highscoreText.text = Mathf.Round(SaveManager.instance.playerData.highScore).ToString("F0");
            hideNewScore = true;
        }
        stoneGet.text = "+" + (SaveManager.instance.playerData.stone - stoneHold);
        //儲存玩家資料
        SaveManager.instance.Save();
    }
    public override void OnOpen()
    {
        base.OnOpen();
        Time.timeScale = 0.001f;
        if (hideNewScore)
            ani.SetTrigger("Play");
        
    }
    public void Replay()
    {
        //載入當前關卡
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
