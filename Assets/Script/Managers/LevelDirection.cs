using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelDirection : Singleton<LevelDirection> {
    #region Base
    [SerializeField]
    private GameObject m_GameOverMenu;
    [SerializeField]
    private GameObject m_LettersBG;
    [SerializeField]
    private GameObject m_WatchUI;

    private string _endNumString;
    public string EndNumString { get { return _endNumString; }set { _endNumString = value; } }

    private bool _paused;
    public bool Paused { get { return _paused; }  set { _paused = value; }  }

    private bool _gameOver;
    public bool GameOver { get { return _gameOver; } set { _gameOver = value; } }
    
    #endregion

    private PlayerData m_data;

    private int _currentScore = 0;
    public int CurrentScore { get { return _currentScore; }
        set
        {
            _currentScore = value;
            if (_currentScore > _maxScore)
            {
                m_data.maxScore = value;
                _maxScore = value;
            }
        }
    }
    private int _maxScore;
    public int MaxScore { get { return _maxScore; } }

    private void Awake()
    {
        m_data = Resources.Load<PlayerData>("Prefabs/PlayerData");
        _maxScore = m_data.maxScore;
        m_LettersBG.transform.DOScale(1,1);
        _paused = false;
        _gameOver = false;
    }

    void Start () {

	}

    private void Update()
    {
        if (GameOver && !Paused)
        {
            GameOverTrue();
        }
    }

    private void GameOverTrue()
    {
        Paused = true;
        m_WatchUI.transform.DOScale(0,0.5f);
        m_data.currentScore = CurrentScore;
        m_LettersBG.transform.DOScale(0, 1);
        m_GameOverMenu.transform.DOLocalMove(new Vector3(0,-175,0), 3f);
        AddHistoryScore();
    }

    private void AddHistoryScore()
    {
        if (CurrentScore <= 0) return;

        if (m_data.LeaderboardDatas.Count >= 10)
        {
            for (int i = 0; i < m_data.LeaderboardDatas.Count; i++)
            {
                if (CurrentScore > m_data.LeaderboardDatas[i].currentScore)
                {
                    LeaderboardData leaderboardData = new LeaderboardData();
                    leaderboardData.currentScore = _currentScore;
                    leaderboardData.nowTime = System.DateTime.Now.ToString("y年m月dd 日 \n h··mm··ss tt");
                    leaderboardData.numText = _endNumString;
                    m_data.LeaderboardDatas.Add(leaderboardData);
                    break;
                }
            }
            if (m_data.LeaderboardDatas.Count > 10)
            {
                m_data.LeaderboardDatas.RemoveAt(m_data.LeaderboardDatas.Count - 2);
            }
        }
        else
        {
            LeaderboardData leaderboardData = new LeaderboardData();
            leaderboardData.currentScore = _currentScore;
            leaderboardData.nowTime = System.DateTime.Now.ToString("y年m月dd 日 \n h··mm··ss tt");
            leaderboardData.numText = _endNumString;
            m_data.LeaderboardDatas.Add(leaderboardData);
        }
    }
}
