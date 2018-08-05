using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    private const int MAX_SCORE = 999999;

    public int stageNo;

    public GameObject textGameOver;
    public GameObject textClear;
    public GameObject buttons;
    public GameObject textScoreNumber;

    public enum GAME_MODE
    {
        PLAY,
        CLEAR,
        GAMEOVER,
    };
    public GAME_MODE gameMode = GAME_MODE.PLAY;

    private int score = 0;
    private int displayScore = 0;

    public AudioClip clearSE;
    public AudioClip gameoverSE;

    private AudioSource audioSource;

    // Use this for initialization
    void Start()
    {
        audioSource = this.gameObject.GetComponent<AudioSource>();
        RefreshScore();
    }

    // Update is called once per frame
    void Update()
    {
        if (score > displayScore)
        {
            displayScore += 10;
            if (displayScore > score)
            {
                displayScore = score;
            }
            RefreshScore();
        }
    }

    public void AddScore(int val)
    {
        score += val;
        if (score > MAX_SCORE)
        {
            score = MAX_SCORE;
        }
    }

    private void RefreshScore()
    {
        textScoreNumber.GetComponent<Text>().text = displayScore.ToString();
    }

    public void GameOver()
    {
        audioSource.PlayOneShot(gameoverSE);
        textGameOver.SetActive(true);
        buttons.SetActive(false);

        Invoke("GoBackStageSelect", 2.0f);
    }

    public void GameClear()
    {
        audioSource.PlayOneShot(clearSE);
        gameMode = GAME_MODE.CLEAR;
        textClear.SetActive(true);
        buttons.SetActive(false);

        if (PlayerPrefs.GetInt("CLEAR", 0) < stageNo)
        {
            PlayerPrefs.SetInt("CLEAR", stageNo);
        }
        Invoke("GoBackStageSelect", 2.0f);
    }

    void GoBackStageSelect()
    {
        SceneManager.LoadScene("StageSelectScene");
    }
}
