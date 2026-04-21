using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    private Text m_totalPointText;

    [SerializeField]
    private Text m_youPointText;

    [SerializeField]
    private Text m_addYouPointText;

    [SerializeField]
    private Text m_opponentPointText;

    [SerializeField]
    private Text m_addOpponentPointText;

    [SerializeField]
    private Text m_totalOpponentPointText;

    //--------------------------------------------
    // データ
    //--------------------------------------------

    private int m_addYouScore;

    private int m_addOpponentScore;

    //--------------------------------------------
    // 初期化
    //--------------------------------------------

    // Start is called before the first frame update
    void Start()
    {
        m_totalPointText.text = "0";
        m_youPointText.text = "0";
        m_opponentPointText.text = "0";
        m_totalOpponentPointText.text = "0";

        m_addYouPointText.text = "";
        m_addOpponentPointText.text = "";

        m_addYouScore = 0;
        m_addOpponentScore = 0;
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
            m_youPointText.text = myData.Score.ToString();
            m_opponentPointText.text = opponentData.Score.ToString();
            m_totalOpponentPointText.text = opponentData.TotalScore.ToString();
        }
    }

    public void AddScore(int score, bool isYouTurn)
    {
        string strScore;
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
            m_addYouScore = score;
            m_addYouPointText.text = strScore;
        }
        else
        {
            m_addOpponentScore = score;
            m_addOpponentPointText.text = strScore;
        }
        StartCoroutine(InVisibleAddScore());
    }

    private IEnumerator InVisibleAddScore()
    {
        yield return new WaitForSeconds(ADD_SCORE_VISIBLE_TIME);
        m_addYouPointText.text = "";
        m_addOpponentPointText.text = "";
    }
}
