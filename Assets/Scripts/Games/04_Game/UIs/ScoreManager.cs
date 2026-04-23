using System.Collections;
using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    //--------------------------------------------
    // 定数
    //--------------------------------------------

    [SerializeField, Header("加算得点表示時間")]
    public float ADD_SCORE_VISIBLE_TIME = 3.0f;

    //--------------------------------------------
    // コンポーネント
    //--------------------------------------------

    [SerializeField]
    private TextMeshProUGUI m_totalPointText;

    [SerializeField]
    private TextMeshProUGUI m_totalOpponentPointText;

    [SerializeField]
    private TextMeshProUGUI m_addPointText;

    [SerializeField]
    private TextMeshProUGUI m_addOpponentPointText;

    //--------------------------------------------
    // 初期化
    //--------------------------------------------
    void Start()
    {
        m_totalPointText.text = "0";
        m_totalOpponentPointText.text = "0";

        m_addPointText.text = string.Empty;
        m_addOpponentPointText.text = string.Empty;
    }

    //--------------------------------------------
    // 更新
    //--------------------------------------------

    // Update is called once per frame
    void Update()
    {
        if(GameManager.I.GameStatus != GameState.Init)
        {
            UserData myData = GameInfo.MyData;
            UserData opponentData = GameInfo.OpponentData;
            // スコア表示更新
            m_totalPointText.text = myData.TotalScore.ToString();
            m_totalOpponentPointText.text = opponentData.TotalScore.ToString();
        }
    }

    public void AddScore(int score, bool isYouTurn)
    {
        string strScore = string.Empty;
        if (score > 0)
        {
            strScore = "+" + score.ToString();
        }
        else
        {
            strScore = score.ToString();
        }

        if (isYouTurn)
        {
            m_addPointText.text = strScore;
        }
        else
        {
            m_addOpponentPointText.text = strScore;
        }
        StartCoroutine(InVisibleAddScore());
    }

    private IEnumerator InVisibleAddScore()
    {
        yield return new WaitForSeconds(ADD_SCORE_VISIBLE_TIME);
        m_addPointText.text = string.Empty;
        m_addOpponentPointText.text = string.Empty;
    }
}
