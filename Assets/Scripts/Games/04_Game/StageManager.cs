using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    [System.Serializable]
    private struct ScoreSet
    {
        public int count;
        public int score;

        public ScoreSet(int count, int score)
        {
            this.count = count;
            this.score = score;
        }
    }

    //--------------------------------------------
    // 定数
    //--------------------------------------------
    [SerializeField, Header("あたり")]
    private ScoreSet JACKPOT = new ScoreSet(1, 10);
    public int JACKPOT_SCORE { get { return JACKPOT.score; } }

    [SerializeField, Header("はずれ")]
    private ScoreSet MISS = new ScoreSet(1, -1);
    public int MISS_SCORE { get { return MISS.score; } }

    [SerializeField, Header("基本スコア")]
    private int DEFAULT_SCORE = 1;

    //--------------------------------------------
    // コンポーネント
    //--------------------------------------------

    [SerializeField, Header("結果表示用UI")]
    private ResultManager m_result;

    [SerializeField]
    private PopupManager m_popupManager;

    [SerializeField]
    private WordManager[] m_wordManagers;

    //--------------------------------------------
    // 初期化
    //--------------------------------------------

    // Start is called before the first frame update
    public void Init()
    {
        for (int i = 0; i < m_wordManagers.Length; i++)
        {
            m_wordManagers[i].SetNo(i);
        }
    }

    public void InitView()
    {
        m_popupManager.Invisible();
        m_result.Invisible();
    }

    public void CreateData(List<int> words)
    {
        // 配置決めに使用する得点リストの作成
        List<int> points = CreatePointList();

        // 配置を設定する
        for (int i = 0; i < m_wordManagers.Length; i++)
        {
            int wordNo = UnityEngine.Random.Range(0, words.Count);
            int pointNo = UnityEngine.Random.Range(0, points.Count);
            GameInfo.Game.WordDatas.Add(new WordData(words[wordNo], points[pointNo], false));
            words.RemoveAt(wordNo);
            points.RemoveAt(pointNo);

            GameInfo.Game.UserData[0].WordPlaceRead.Add(false);
            GameInfo.Game.UserData[1].WordPlaceRead.Add(false);
        }
    }

    /// <summary>
    /// 得点リストの作成
    /// </summary>
    /// <returns></returns>
    private List<int> CreatePointList()
    {
        List<int> points = new List<int>();
        // 当たりの設定
        for (int i = 0; i < JACKPOT.count; i++)
        {
            points.Add(JACKPOT.score);
        }
        // 外れの設定
        for (int i = 0; i < MISS.count; i++)
        {
            points.Add(MISS.score);
        }
        // 合計設定可能個所数から当たりとはずれを引いた残りを基本の点数で設定する
        int defaultCount = m_wordManagers.Length - JACKPOT.count - MISS.count;
        for (int i = 0; i < defaultCount; i++)
        {
            points.Add(DEFAULT_SCORE);
        }
        return points;
    }

    //--------------------------------------------
    // 表示
    //--------------------------------------------

    /// <summary>
    /// 設定されたデータを表示する
    /// </summary>
    public void VisibleDatas()
    {
        // 単語を表示
        for (int i = 0; i < m_wordManagers.Length; i++)
        {
            m_wordManagers[i].SetData();
        }
    }

    public void VisiblePopup(int no)
    {
        m_wordManagers[no].VisiblePopup();
    }

    public void VisibleResult()
    {
        m_result.Visible();
    }
}
