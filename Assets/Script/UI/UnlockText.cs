using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnlockText : MonoBehaviour {

    private Text m_Text;
    private int nowNum;
    private PlayerData m_Data;

    private void Awake()
    {
        m_Text = GetComponent<Text>();
        m_Data = Resources.Load<PlayerData>("Prefabs/PlayerData");
    }

	void Update () {
        if (!LevelDirection.Instance.GameOver && nowNum+1 <= GameController.Instance.m_CurrentNum)
        {
            nowNum = GameController.Instance.m_CurrentNum;
            m_Text.text += m_Data.numTexts[nowNum - 1];
        }
        else
        {
            LevelDirection.Instance.EndNumString = m_Text.text;
            m_Data.endText = m_Text.text;
        }
	}
}
