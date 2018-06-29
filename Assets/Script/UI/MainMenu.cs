using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {
    private AudioSource m_ClickOnAudio;
    [SerializeField]
    private GameObject m_MainGroup;
    [SerializeField]
    private GameObject m_HelpGroup;
    [SerializeField]
    private GameObject m_LeaderboardGroup;

    private List<GameObject> groupOBJList = new List<GameObject>();
    private Stack<GameObject> groupObjStack = new Stack<GameObject>();

    private void Awake()
    {
        m_ClickOnAudio = GetComponent<AudioSource>();
        groupOBJList.Clear();
        groupObjStack.Clear();
    }

    void Start () {
        groupOBJList.Add(m_MainGroup);
        groupOBJList.Add(m_HelpGroup);
        groupOBJList.Add(m_LeaderboardGroup);

        groupObjStack.Push(m_MainGroup);
        DisPlayMenu();
    }

    public void StartGameButton()
    {
        m_ClickOnAudio.Play();
        SceneManager.LoadScene(1);
    }
    public void HelpGroupButton()
    {
        m_ClickOnAudio.Play();
        groupObjStack.Push(m_HelpGroup);
        DisPlayMenu();
    }
    public void LeaderboardGroupButton()
    {
        m_ClickOnAudio.Play();
        groupObjStack.Push(m_LeaderboardGroup);
        DisPlayMenu();
    }
    public void BackButton()
    {
        m_ClickOnAudio.Play();
        groupObjStack.Push(m_MainGroup);
        DisPlayMenu();
    }

    private void DisPlayMenu()
    {
        foreach (GameObject item in groupOBJList)
        {
            if (item == m_LeaderboardGroup)
            {
                item.transform.DOMoveX(-80, 0.8f);
            }
            else
            {
                item.transform.DOScale(0, 0.6f);
            }
        }

        if (groupObjStack.Count > 0)
        {
            GameObject obj = groupObjStack.Peek();
            if (obj == m_LeaderboardGroup)
            {
                obj.transform.DOMoveX(0, 0.8f);
            }
            else
            {
                obj.transform.DOScale(1, 0.6f);
            }
        }
    }
}
