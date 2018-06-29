using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LeaderboardMenu : MonoBehaviour {
    [SerializeField]
    private GameObject content;

    private LevelDirection m_Director;
    private PlayerData m_Data;
    private List<Text> scoreTexts = new List<Text>();
    private List<Text> numTexts = new List<Text>();
    private List<Text> timeTexts = new List<Text>();

    private void Awake()
    {
        m_Data = Resources.Load<PlayerData>("Prefabs/PlayerData");
        foreach (Transform item in content.transform)
        {
            timeTexts.Add(item.GetComponent<Text>());
            scoreTexts.Add(item.GetChild(0).GetComponent<Text>());
            numTexts.Add(item.GetChild(1).GetComponent<Text>());
        }
    }

    void Start () {
        m_Data.LeaderboardDatas.Sort();
        m_Data.LeaderboardDatas.Reverse();
        for (int i = 0; (i < scoreTexts.Count) && (i < m_Data.LeaderboardDatas.Count); i++)
        {
            timeTexts[i].text = m_Data.LeaderboardDatas[i].nowTime;
            scoreTexts[i].text ="分\n数\n··\n" + "<size=70>"+m_Data.LeaderboardDatas[i].currentScore.ToString()+"</size>";
            numTexts[i].text = m_Data.LeaderboardDatas[i].numText;
        }
	}
}
