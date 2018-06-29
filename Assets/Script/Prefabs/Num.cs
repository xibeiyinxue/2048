using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Num : MonoBehaviour {
    public int x = 0, y = 0;
    public int num = 0;
    [SerializeField]
    private Sprite[] _sprites;
    private Image m_NumImage;
    private bool _isDisappear = false;
    public bool IsDisappear { get { return _isDisappear; } set { _isDisappear = value; } }

    private void Awake()
    {
        m_NumImage = GetComponent<Image>();
    }

    void Start () {
        InitShow();
        InitPos();
	}

    private void InitPos() /*初始位置*/
    {
        transform.localPosition = new Vector3(-300 + x * 200, 350 - y * 250, 0);
    }

    private void InitShow() /*初始图片*/
    {
        switch (num)
        {
            case 1:
                m_NumImage.sprite = _sprites[0];
                break;
            case 2:
                m_NumImage.sprite = _sprites[1];
                break;
            case 3:
                m_NumImage.sprite = _sprites[2];
                break;
            case 4:
                m_NumImage.sprite = _sprites[3];
                break;
            case 5:
                m_NumImage.sprite = _sprites[4];
                break;
            case 6:
                m_NumImage.sprite = _sprites[5];
                break;
            case 7:
                m_NumImage.sprite = _sprites[6];
                break;
            case 8:
                m_NumImage.sprite = _sprites[7];
                break;
            case 9:
                m_NumImage.sprite = _sprites[8];
                break;
            case 10:
                m_NumImage.sprite = _sprites[9];
                break;
            case 11:
                m_NumImage.sprite = _sprites[10];
                break;
            case 12:
                m_NumImage.sprite = _sprites[11];
                break;
        }
    }

    public bool MoveToPosition(int targetX, int targetY, bool isNeedUpdateComponentArray = true)
    {
        bool temp = x != targetX || y != targetY; // x = targetX && y = targetY,temp = false;
        GameController.Instance.m_NumArray[x, y]--;
        GameController.Instance.m_NumArray[targetX, targetY]++;
        x = targetX;
        y = targetY;
        if (isNeedUpdateComponentArray) //在 GameController 中增加自己的位置
        {
            GameController.Instance.m_NumComponentArray[x, y] = this;
        }
        //移动至规定区域或在规定区域内生成
        transform.DOLocalMove(new Vector3(-300 + x * 200, 350 - y * 250, 0), 0.2f).OnComplete(OnTweenMoveCompleted); 
        return temp;
    }
    public void OnTweenMoveCompleted() //在 0.2f 的时间内把自己的 Scale 值变为 0
    {
        if (IsDisappear)
        {
            transform.DOScale(0, 0.2f).OnComplete(OnTweenOutCompleted);
        }
    }
    public void OnTweenOutCompleted() //减去在 GameController 中二维数组中自身所占用的位置
    {
        GameController.Instance.m_NumArray[x, y]--;
        Destroy(this.gameObject);
    }
}
