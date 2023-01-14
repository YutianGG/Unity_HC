using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class EndUI : Windows<EndUI>
{
    [SerializeField] Text scoreText;
    [SerializeField] Text highscoreText;
    [SerializeField] Animator ani;
    bool hideNewScore = false;
    public override void Open()
    {
        base.Open();
        Cursor.lockState = CursorLockMode.None;
        scoreText.text = Mathf.Round(PlayerControl.instance.score).ToString();
        if(PlayerControl.instance.score <= PlayerControl.instance.highScore)
            highscoreText.text = Mathf.Round(PlayerControl.instance.highScore).ToString();
        else
        {
            //新紀錄
            PlayerControl.instance.highScore = PlayerControl.instance.score;
            highscoreText.text = Mathf.Round(PlayerControl.instance.highScore).ToString();
            hideNewScore = true;
        }
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
