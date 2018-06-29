using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

//游戏进程状态机
public enum GameStatus
{
    Play,
    Paused,
}
//滑动状态机
public enum TouchDir
{
    Left,
    Right,
    Top,
    Bottom,
    None,
}

public class GameController : MonoBehaviour {
    public static GameController Instance;

    [SerializeField]
    private GameObject numPrefab;
    private PlayerData m_Data;
    private AudioSource m_Audio;
    private int m_IndexNum;
    public int m_CurrentNum;
    private GameStatus status = GameStatus.Play;
    private Vector3 mouseDownPosition;

    public int[,] m_NumArray = new int[4, 4]
    {
        {0,0,0,0 },
        {0,0,0,0 },
        {0,0,0,0 },
        {0,0,0,0 }
    };
    public Num[,] m_NumComponentArray = new Num[4, 4]
    {
        {null,null,null,null },
        {null,null,null,null },
        {null,null,null,null },
        {null,null,null,null }
    };

    private void Awake()
    {
        Instance = this;
        m_Data = Resources.Load<PlayerData>("Prefabs/PlayerData");
        m_Audio = GetComponent<AudioSource>();
    }

    void Start () {
        GenerateNumber();
        GenerateNumber();
        GenerateNumber();
        m_IndexNum = m_Data.numTexts.Count;
        m_CurrentNum = 1;
    }
	
	void Update () {
        OnGameStatus();
        if (LevelDirection.Instance.Paused)
        {
            status = GameStatus.Paused;
        }
        else
        {
            status = GameStatus.Play;
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            ShowGameOverPanel();
        }
    }

    //生成新 Num
    private void GenerateNumber(int posX = -1,int posY = -1,int number = 1)
    {
        int numberX = -1, numberY = -1;
        if (posX == -1 || posY == -1)
        {
            int countOfEmptyNum = 0;
            //使用二维数组进行 For 循环
            for (int x = 0; x < 4; x++)
            {
                for (int y = 0; y < 4; y++)
                {
                    if (m_NumArray[x,y] == 0)
                    {
                        //遍历二维数组所有内容，若为空 count 则增加一个计数
                        countOfEmptyNum++;
                    }
                }
            }
            //如果遍历完二维数组，没有一个空的位置，则退出该方法
            if (countOfEmptyNum == 0) return;
            //随机一个数（或者可以称为随机位置）
            int randomNum = Random.Range(1, countOfEmptyNum + 1);
            int index = 0;
            for (int x = 0; x < 4; x++)
            {
                for (int y = 0; y < 4; y++)
                {
                    if (m_NumArray[x,y] == 0)
                    {
                        //遍历二维数组，若该位置为空则 index 计数增加
                        index++;
                        if (index == randomNum)
                        {
                            //如果 index 和随机的计数（位置）相同
                            numberX = x;
                            numberY = y;
                            goto flag; //goto语句
                        }
                    }
                }
            }
        }
        else
        {
            numberX = posX;
            numberY = posY;
        }
    flag:
        GameObject numpre = Instantiate(numPrefab); //生成一个 2048 预置体
        numpre.transform.DOScale(1, 0.2f);
        numpre.transform.SetParent(this.transform);
        Num num = numpre.GetComponent<Num>();
        num.num = number;
        num.x = numberX;
        num.y = numberY;
        m_NumArray[numberX, numberY]++;
        m_NumComponentArray[numberX, numberY] = num;
    }
    //检测胜利
    private bool CheckGameWin()
    {
        for (int x = 0; x < 4; x++)
        {
            for (int y = 0; y < 4; y++)
            {
                if (m_NumArray[x, y] != 0)
                {
                    if (m_NumComponentArray[x, y].num == 12)
                    {
                        return true;
                    }
                }
            }
        }
        return false;
    }
    //检测失败
    private bool CheckGameOver()
    {
        for (int x = 0; x < 4; x++)
        {
            for (int y = 0; y < 4; y++)
            {
                if (m_NumArray[x,y] == 0)
                {
                    return false;
                }
            }
        }
        for (int x = 0; x < 4; x++)
        {
            for (int y = 0; y < 3; y++)
            {
                if (m_NumComponentArray[x,y].num == m_NumComponentArray[x,y+1].num)
                {
                    return false;
                }
            }
        }
        for (int y = 0; y < 4; y++)
        {
            for (int x = 0; x < 3; x++)
            {
                if (m_NumComponentArray[x,y].num == m_NumComponentArray[x+1,y].num)
                {
                    return false;
                }
            }
        }
        return true;
    }
    //游戏状态
    private void OnGameStatus()
    {
        switch (status)
        {
            case GameStatus.Play:
                MoveNum();
                break;
            case GameStatus.Paused:
                break;
        }
    }
    //行动状态
    private void MoveNum()
    {
        bool isAnyNumMove = false;
        int countCombine = 0;
        if (Input.GetMouseButtonDown(0))
        {
            mouseDownPosition = Input.mousePosition;
        }
        else if (!Input.GetMouseButtonUp(0)) return;
        TouchDir dir = GetTouchDir();
        switch (dir)
        {
            //向左移动
            case TouchDir.Left:
                for (int y = 0; y < 4; y++)
                {
                    Num preNum = null;
                    int index = -1;
                    for (int x = 0; x < 4; x++)
                    {
                        bool isNeedUpdateComponentArray = true;
                        if (m_NumArray[x,y] == 0)
                            continue;
                        if (preNum == null)
                        {
                            preNum = m_NumComponentArray[x, y];
                            index++;
                        }
                        else
                        {
                            if (preNum.num == m_NumComponentArray[x, y].num)
                            {
                                //如果当前 2048 预置体的值等于该 Num 表格中的 num 的值，则可以进行合并
                                countCombine++;
                                GenerateNumber(index, y, preNum.num + 1); //表示 Y 轴不变，X 轴移动到 index 记数后的位置
                                if (m_CurrentNum < preNum.num + 1)
                                {
                                    //如果当前 2048 预置体小过计数后合并的值
                                    m_CurrentNum = preNum.num + 1;
                                }
                                LevelDirection.Instance.CurrentScore += (int)Mathf.Pow(2, preNum.num + 1);
                                isNeedUpdateComponentArray = false;

                                preNum.IsDisappear = true;
                                m_NumComponentArray[x, y].IsDisappear = true;
                                preNum = null;
                            }
                            else
                            {
                                preNum = m_NumComponentArray[x, y];
                                index++;
                            }
                        }
                        // move to (index,y)
                        if (m_NumComponentArray[x,y].MoveToPosition(index,y,isNeedUpdateComponentArray) )
                        {
                            isAnyNumMove = true;
                        }
                    }
                }
                break;
            case TouchDir.Right:
                for (int y = 0; y < 4; y++)
                {
                    Num preNum = null;
                    int index = 4;
                    for (int x = 3; x >= 0; x--)
                    {
                        bool isNeedUpdateComponentArray = true;
                        if (m_NumArray[x, y] == 0) continue;
                        if (preNum == null)
                        {
                            preNum = m_NumComponentArray[x, y];
                            index--;
                        }
                        else
                        {
                            if (preNum.num == m_NumComponentArray[x, y].num)
                            {
                                countCombine++;
                                GenerateNumber(index, y, preNum.num + 1);
                                if (m_CurrentNum < preNum.num + 1)
                                {
                                    m_CurrentNum = preNum.num + 1;
                                }
                                LevelDirection.Instance.CurrentScore += (int)Mathf.Pow(2, preNum.num + 1);
                                isNeedUpdateComponentArray = false;

                                preNum.IsDisappear = true;
                                m_NumComponentArray[x, y].IsDisappear = true;
                                preNum = null;
                            }
                            else
                            {
                                preNum = m_NumComponentArray[x, y];
                                index--;
                            }
                        }
                        // move to (index,y)
                        if (m_NumComponentArray[x, y].MoveToPosition(index, y, isNeedUpdateComponentArray))
                        {
                            isAnyNumMove = true;
                        }
                    }
                }
                break;
            case TouchDir.Top:
                for (int x = 0; x < 4; x++)
                {
                    Num preNum = null;
                    int index = -1;
                    for (int y = 0; y < 4; y++)
                    {
                        bool isNeedUpdateComponentArray = true;
                        if (m_NumArray[x, y] == 0) continue;
                        if(preNum == null)
                        {
                            preNum = m_NumComponentArray[x, y];
                            index++;
                        }
                        else
                        {
                            if (preNum.num == m_NumComponentArray[x,y].num)
                            {
                                countCombine++;
                                GenerateNumber(x, index, preNum.num + 1);
                                if (m_CurrentNum < preNum.num +1)
                                {
                                    m_CurrentNum = preNum.num + 1;
                                }
                                LevelDirection.Instance.CurrentScore += (int)Mathf.Pow(2, preNum.num + 1);
                                isNeedUpdateComponentArray = false;

                                preNum.IsDisappear = true;
                                m_NumComponentArray[x, y].IsDisappear = true;
                                preNum = null;
                            }
                            else
                            {
                                preNum = m_NumComponentArray[x, y];
                                index++;
                            }
                        }
                        if (m_NumComponentArray[x,y].MoveToPosition(x,index,isNeedUpdateComponentArray))
                        {
                            isAnyNumMove = true;
                        }
                    }
                }
                break;
            case TouchDir.Bottom:
                for (int x = 0; x < 4; x++)
                {
                    Num preNum = null;
                    int index = 4;
                    for (int y = 3; y >= 0 ; y--)
                    {
                        bool isNeedUpdateComponentArray = true;
                        if (m_NumArray[x, y] == 0) continue;
                        if (preNum == null)
                        {
                            preNum = m_NumComponentArray[x, y];
                            index--;
                        }
                        else
                        {
                            if (preNum.num == m_NumComponentArray[x,y].num)
                            {
                                countCombine++;
                                GenerateNumber(x, index, preNum.num + 1);
                                if (m_CurrentNum < preNum.num+1)
                                {
                                    m_CurrentNum = preNum.num + 1;
                                }
                                LevelDirection.Instance.CurrentScore += (int)Mathf.Pow(2, preNum.num + 1);
                                isNeedUpdateComponentArray = false;

                                preNum.IsDisappear = true;
                                m_NumComponentArray[x, y].IsDisappear = true;
                                preNum = null;
                            }
                            else
                            {
                                preNum = m_NumComponentArray[x, y];
                                index--;
                            }
                        }
                        if (m_NumComponentArray[x,y].MoveToPosition(x,index,isNeedUpdateComponentArray))
                        {
                            isAnyNumMove = true;
                        }
                    }
                }
                break;
            case TouchDir.None:
                return;
        }
        if (countCombine > 0)
        {
            m_Audio.Play();
        }
        if (isAnyNumMove)
        {
            GenerateNumber();
        }
        if (CheckGameWin() || CheckGameOver())
        {
            ShowGameOverPanel();
        }
    }

    private void ShowGameOverPanel()
    {
        LevelDirection.Instance.GameOver = true;
    }

    private TouchDir GetTouchDir()
    {
        if (Input.GetMouseButtonUp(0))
        {
            Vector3 touchOffset = Input.mousePosition - mouseDownPosition;
            if (Mathf.Abs(touchOffset.x)>Mathf.Abs(touchOffset.y) && Mathf.Abs(touchOffset.x) >50)
            {
                if (touchOffset.x > 0) return TouchDir.Right;
                else if (touchOffset.x < 0) return TouchDir.Left;
            }
            else if(Mathf.Abs(touchOffset.x)< Mathf .Abs(touchOffset.y) && Mathf.Abs(touchOffset.y)>50)
            {
                if (touchOffset.y > 0) return TouchDir.Top;
                else if (touchOffset.y < 0) return TouchDir.Bottom;
            }
        }
        return TouchDir.None;
    }
}
