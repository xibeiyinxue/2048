using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerData", menuName = "CreatScriptable/PlayerData", order = 1)]
public class PlayerData : ScriptableObject
{
    public int maxScore;
    public int currentScore; /*最高分*/
    public string endText; /*结束时解锁的文本*/
    public List<string> numTexts = new List<string>(); /*已解锁的文字*/
    public List<LeaderboardData> LeaderboardDatas = new List<LeaderboardData>();
}

[Serializable]
public struct LeaderboardData : IComparable<LeaderboardData>
{
    public string numText;
    public string nowTime;
    public int currentScore;

    public int CompareTo(LeaderboardData x)
    {
        return currentScore.CompareTo(x.currentScore);
    }
}