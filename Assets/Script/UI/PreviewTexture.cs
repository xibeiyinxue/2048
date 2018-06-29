using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PreviewTexture : MonoBehaviour {
    [SerializeField]
    private Button m_Preview = null;
    [SerializeField]
    private Text m_MainText;
    [SerializeField]
    private Text m_text;
   
    private void Awake()
    {
        m_Preview.gameObject.SetActive(false);
    }

    private void Start()
    {
        StartCoroutine(AlphaSwitch(m_MainText));
    }

    //开启观赏图层
    public void OnOpenGamePreview()
    {
        LevelDirection.Instance.Paused = true;
        m_Preview.gameObject.SetActive(true);
        StartCoroutine(AlphaSwitch(m_text));
    }
    //关闭图层
    public void OnCloseGamePreview()
    {
        m_Preview.gameObject.SetActive(false);
        StopCoroutine(AlphaSwitch(m_text));
        LevelDirection.Instance.Paused = false;
    }
    //闪烁字体协程
    IEnumerator AlphaSwitch(Text text)
    {
        text.color = new Color(text.color.r, text.color.g, text.color.b, 1);
        for (int i = 1; i < 9; i++)
        {
            if (i % 2 == 1)
            {
                if (i == 7)
                {
                    i = 1;
                }
                float alpha = 1;
                while (alpha > 0)
                {
                    text.color = new Color(text.color.r, text.color.g, text.color.b, alpha -= Time.deltaTime);
                    yield return null;
                }
            }
            else if(i % 2 == 0)
            {
                float alpha = 0;
                while(alpha < 1)
                {
                    text.color = new Color(text.color.r, text.color.g, text.color.b, alpha += Time.deltaTime);
                    yield return null;
                }
            }
            
        }
    }
}
