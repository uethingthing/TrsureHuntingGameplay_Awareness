using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class WordManager : MonoBehaviour
{
    //--------------------------------------------
    // コンポーネント
    //--------------------------------------------

    private GameManager m_gameManager;

    private GameDataSync m_GameDataSync;

    private PopupManager m_popupManager;

    private ScoreManager m_scoreManager;

    private CardGroupManager m_cardGroupManager;

    private StageManager m_stageManager;

    [SerializeField]
    private Text m_wordText;

    [SerializeField]
    private Text m_JPwordText;

    [SerializeField]
    private Text m_pointText;

    [SerializeField]
    private Image m_wordBackImage;

    [SerializeField]
    private GameObject m_hitObj;

    [SerializeField]
    private GameObject m_missObj;

    //--------------------------------------------
    // データ
    //--------------------------------------------

    [SerializeField]
    private WordSetObject[] m_wordSetObject;

    [SerializeField]
    private WordSetObject[] m_JPwordSetObject;

    private int m_no;

    //--------------------------------------------
    // 初期化
    //--------------------------------------------

    void Awake()
    {
        m_no = -1;

        m_wordText.text = "";
        m_JPwordText.text = "";
        m_pointText.text = "";

        m_wordBackImage.enabled = false;

        m_gameManager = FindObjectsOfType<GameManager>()[0];
        m_GameDataSync = FindObjectsOfType<GameDataSync>()[0];
        m_popupManager = FindObjectsOfType<PopupManager>()[0];
        m_scoreManager = FindObjectsOfType<ScoreManager>()[0];
        m_cardGroupManager = FindObjectsOfType<CardGroupManager>()[0];
        m_stageManager = FindObjectsOfType<StageManager>()[0];

        Button button = gameObject.GetComponent<Button>();
        button.onClick.AddListener(() => StartCoroutine(OnWordButton()));
    }

    public void Visible()
    {
        this.gameObject.SetActive(true);
    }

    public void Invisible()
    {
        this.gameObject.SetActive(false);
    }

    public void VisiblePoint()
    {
        if(m_pointText != null)
        {
            m_pointText.text = GameInfo.Game.WordDatas[m_no].Point.ToString();
        }
    }

    public void SetNo(int no)
    {
        m_no = no;
    }

    public void SetData()
    {
        if (!GameInfo.Game.WordDatas[m_no].Answer)
        {
            // まだ選択されていなければ単語を表示する
            m_wordText.text = m_wordSetObject[GameInfo.Game.SelectLevel].Words[GameInfo.Game.WordDatas[m_no].Place];
            m_JPwordText.text = m_JPwordSetObject[GameInfo.Game.SelectLevel].Words[GameInfo.Game.WordDatas[m_no].Place];
            m_wordBackImage.enabled = true;
        }
        else
        {
            m_wordText.text = "";
            m_JPwordText.text = "";
            m_wordBackImage.enabled = false;
            m_hitObj.SetActive(false);
            m_missObj.SetActive(false);
        }

        if (GameInfo.MyData.WordPlaceRead[m_no])
        {
            //// リーディングアイが使用されていれば得点を表示する
            bool isHit = GameInfo.Game.WordDatas[m_no].Point == m_stageManager.JACKPOT_SCORE;
            m_hitObj.SetActive(isHit);
            m_missObj.SetActive(!isHit);
        }
        else
        {
            m_hitObj.SetActive(false);
            m_missObj.SetActive(false);
        }
    }

    //--------------------------------------------
    // ボタン
    //--------------------------------------------

    /// <summary>
    /// 単語選択時
    /// </summary>
    /// <returns></returns>
    private IEnumerator OnWordButton()
    {
        // 自分のターンかつ、単語のポップアップが表示されていないかつ、まだ単語が選択されていないとき選択可能
        if (m_gameManager.SyncCompletedFlg && !m_gameManager.IsVisiblePopup && !GameInfo.Game.WordDatas[m_no].Answer)
        {
            CardType card = GetCard();
            if(card == CardType.ReadingEye)
            {
                // リーディングアイ使用していた場合、あたり/はずれを表示する
                bool isHit = GameInfo.Game.WordDatas[m_no].Point == m_stageManager.JACKPOT_SCORE;
                m_hitObj.SetActive(isHit);
                m_missObj.SetActive(!isHit);
                
                GameInfo.Game.IsUseCard = false;
                int userNo = GameInfo.MyUserNo;
                GameInfo.Game.UserData[userNo].WordPlaceRead[m_no] = true;
                GameInfo.Game.UserData[userNo].Card.RemoveAt(GameInfo.Game.SelectCardNo);

                m_gameManager.VisibleUseSkill(false);
                // 所持カードの表示更新
                m_cardGroupManager.Visible();

                yield return m_GameDataSync.CoUpdateGameData(GameInfo.Game);
            }
            else
            {
                m_gameManager.IsVisiblePopup = true;
                GameInfo.Game.WordDatas[m_no].Answer = true;

                // 相手にも単語を表示させるためデータを送信
                yield return m_GameDataSync.CoUpdateGameData(GameInfo.Game);

                VisiblePopup();
            }
        }
    }

    //--------------------------------------------
    // 
    //--------------------------------------------

    /// <summary>
    /// 単語拡大表示
    /// </summary>
    public void VisiblePopup()
    {
        bool isHit = GameInfo.Game.WordDatas[m_no].Point == m_stageManager.JACKPOT_SCORE;
        if(isHit)
        {
            AudioManager.I.PlaySe(AudioNames.HitSE);
        }
        else
        {
            AudioManager.I.PlaySe(AudioNames.ButtonSE);
        }
        m_popupManager.Visible(GameInfo.Game.WordDatas[m_no].Place);
        StartCoroutine(CoVisiblePopup(GameInfo.Game.Turn));
    }

    /// <summary>
    /// 単語拡大表示
    /// </summary>
    /// <returns></returns>
    private IEnumerator CoVisiblePopup(Turn turn)
    {
        yield return new WaitForSeconds(m_gameManager.POPUP_VISIBLE_TIME);
        m_gameManager.VisibleUseSkill(false);
        m_popupManager.Invisible();
        m_wordText.text = "";
        m_JPwordText.text = "";
        m_wordBackImage.enabled = false;
        m_hitObj.SetActive(false);
        m_missObj.SetActive(false);

        // 選択した側の場合
        if (GameInfo.MyTurn == turn)
        {
            int userNo = GameInfo.MyUserNo;

            // 選択した単語に設定されていたスコアを加算
            int point = CalculationScore(GameInfo.Game.WordDatas[m_no].Point);
            GameInfo.Game.UserData[userNo].Score += point;
            GameInfo.Game.UserData[userNo].TotalScore += point;
            m_scoreManager.AddScore(point, true);

            CardType card = GetCard();
            // カードを使用していた場合リストから削除する
            if (GameInfo.Game.IsUseCard)
            {
                if(card == CardType.Double)
                {
                    // リザルト表示に使用する為、Doubleカードの使用履歴を残す
                    GameInfo.Game.UserData[userNo].IsUseDouble = true;
                }
                else if(card == CardType.Rare)
                {
                    // リザルト表示に使用する為、Rareカードの使用履歴を残す
                    GameInfo.Game.UserData[userNo].IsUseRare = true;
                }

                GameInfo.Game.IsUseCard = false;
                GameInfo.Game.UserData[userNo].Card.RemoveAt(GameInfo.Game.SelectCardNo);
                // 所持カードの表示更新
                m_cardGroupManager.Visible();
            }
            GameInfo.Game.SelectCardNo = -1;

            if (!m_gameManager.CheckGameEnd())
            {
                // ゲームが終了していなければターンの変更
                if (!GameInfo.IsSingleMode && card != CardType.Combo)
                {
                    // デバッグ用モードでないかつ特定カードが使用されていなければターンを変更
                    GameInfo.Game.Turn = GameInfo.OpponentTurn;
                }
                // データを送信する。
                yield return m_GameDataSync.CoUpdateGameData(GameInfo.Game);
                m_gameManager.SyncCompletedFlg = false;
            }
            else
            {
                GameInfo.Game.Turn = Turn.Result;
                // ゲームが終了していればデータを送信し、結果画面へ
                yield return m_GameDataSync.CoUpdateGameData(GameInfo.Game);
                yield return m_gameManager.CoResult();
            }
        }

        m_gameManager.IsVisiblePopup = false;
    }

    /// <summary>
    /// 獲得ポイント計算
    /// </summary>
    /// <param name="point"></param>
    /// <param name="turn"></param>
    /// <returns></returns>
    private int CalculationScore(int point)
    {
        // カードの計算
        CardType card = GetCard();
        switch (card)
        {
            case CardType.Double:
                // ポイント2倍
                point *= 2;
                break;

            case CardType.Rare:
                // 追加ポイント
                point += 10;
                break;

            case CardType.Protect:
                // マイナスにならない
                if (point < 0)
                {
                    point = 0;
                }
                break;
        }
        return point;
    }

    /// <summary>
    /// 使用したカードの種類取得
    /// </summary>
    /// <param name="turn"></param>
    /// <returns></returns>
    private CardType GetCard()
    {
        CardType card = CardType.None;
        if (GameInfo.Game.IsUseCard)
        {
            card = GameInfo.MyData.Card[GameInfo.Game.SelectCardNo];
        }
        return card;
    }
}
