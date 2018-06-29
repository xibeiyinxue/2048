using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreText : MonoBehaviour {
    [SerializeField]
    private Text m_MaxScore;
    [SerializeField]
    private Text m_CurrentScore;

    private PlayerData m_data;
    private LevelDirection m_Direction;

    private void Awake()
    {
        m_Direction =LevelDirection.Instance;
        m_data = Resources.Load<PlayerData>("Prefabs/PlayerData");
    }

    void Start () {
        
    }
	
	void Update () {
        UpdateCurrentScore();
    }

    private void UpdateCurrentScore()
    {
        m_MaxScore.text = m_data.maxScore.ToString();
        m_CurrentScore.text = m_Direction.CurrentScore.ToString();
    }
}
