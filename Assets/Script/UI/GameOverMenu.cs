using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverMenu : MonoBehaviour {
    [SerializeField]
    private Text m_ScoreText;
    [SerializeField]
    private Text m_UnlockText;

    private AudioSource m_ChickOnAudio;

    private void Awake()
    {
        m_ChickOnAudio = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (LevelDirection.Instance.GameOver)
        {
            OnGameOver();
        }
    }

    private void OnGameOver()
    {
        m_ScoreText.text = LevelDirection.Instance.CurrentScore.ToString();
        m_UnlockText.text = LevelDirection.Instance.EndNumString;
    } 

    public void BackMainMenu()
    {
        m_ChickOnAudio.Play();
        SceneManager.LoadScene(0);
    }
}
