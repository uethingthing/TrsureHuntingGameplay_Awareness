using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ゲームを管理するクラス
/// </summary>
public class GameManager : SingletonMonoBehaviour<GameManager>
{
    //--------------------------------------------
    // 定数
    //--------------------------------------------

    [SerializeField, Header("単語表示時間")]
    public float POPUP_VISIBLE_TIME = 5.0f;

    /// <summary>
    /// 初期カード数
    /// </summary>
    private int HOLD_CARD_COUNT = 3;

    //--------------------------------------------
    // コンポーネント
    //--------------------------------------------

    [SerializeField, Header("ゲームデータ同期")]
    private GameDataSync m_GameDataSync;

    [SerializeField]
    private Transform m_stageTransform;

    [SerializeField]
    private GameObject[] m_stageSets;

    private StageManager m_stageManager;

    [SerializeField]
    private CardGroupManager m_cardGroupManager;

    [SerializeField]
    private ScoreManager m_scoreManager;

    [SerializeField]
    private StartManager m_startManager;

    [SerializeField]
    private GameObject m_useSkillObject;

    [SerializeField]
    private GameObject m_cardMenuObject;

    [SerializeField]
    private WordSetObject[] m_wordSetObject;

    [SerializeField]
    private GameObject m_uiObject;

    //--------------------------------------------
    // データ
    //--------------------------------------------

    /// <summary>
    /// ゲーム状態
    /// </summary>
    public GameState GameStatus { get; private set; }

    /// <summary>
    /// 同期が完了したか識別するFlg
    /// </summary>
    public bool SyncCompletedFlg { get; set; }

    /// <summary>
    /// 単語表示中か
    /// </summary>
    public bool IsVisiblePopup { get; set; }

    //--------------------------------------------
    // 初期化
    //--------------------------------------------

    protected override void Awake()
    {
        base.Awake();

        SyncCompletedFlg = false;
        IsVisiblePopup = false;
        GameStatus = GameState.Init;
    }

    /// <summary>
    /// Start
    /// </summary>
    private void Start()
    {
        // メインBGM再生
        AudioManager.I.PlayBgm(AudioNames.MainBgm, 0.5f);

        m_stageManager = Instantiate(m_stageSets[GameInfo.Game.StageNo], m_stageTransform).GetComponent<StageManager>();
        m_stageManager.Init();

        StartCoroutine(CoInitialize());
    }

    /// <summary>
    /// 初期化
    /// </summary>
    /// <returns></returns>
    private IEnumerator CoInitialize()
    {
        // 状態を初期化状態に設定
        GameStatus = GameState.Init;

        // 表示初期化
        m_stageManager.InitView();

        // 1度だけ勝手にカードが選択されるのを防止するためここで非表示
        m_cardMenuObject.SetActive(false);

        m_cardGroupManager.VisibleSkillCardButton(false);

        VisibleUseSkill(false);

        // 前回の状態からゲームを開始する
        if (GameInfo.IsRestart)
        {
            yield return CoRestart();
        }
        // ゲームを開始する
        else
        {
            yield return CoStart();
        }
    }

    /// <summary>
    /// ゲームを開始する
    /// </summary>
    /// <returns></returns>
    private IEnumerator CoStart()
    {
        // 自分のターン中か調べる
        CheckMyTurn();

        // User1が先に情報を設定する。
        if (GameInfo.MyTurn == Turn.User01)
        {
            yield return CoInitializeUser1();
            Debug.Log("User1初期化");
        }
        // User2はデータベース上の情報を取得し、設定。
        else if (GameInfo.MyTurn == Turn.User02)
        {
            yield return CoInitializeUser2();
            Debug.Log("User2初期化");
        }

        // Start表示
        yield return m_startManager.VisibleStart();

        // 設定されたデータを表示する
        VisibleDatas();

        // UIを表示する
        VisibleUI(true);

        // ゲーム同期を開始する
        m_GameDataSync.StartGameSync();

        // 状態をGameプレイ状態に設定
        GameStatus = GameState.Game;
    }

    /// <summary>
    /// User1の初期化
    /// </summary>
    /// <returns></returns>
    private IEnumerator CoInitializeUser1()
    {
        // 配置決めに使用する文字データリスト作成
        List<int> words = new List<int>();
        for (int i = 0; i < m_wordSetObject[GameInfo.Game.SelectLevel].Words.Count; i++)
        {
            words.Add(i);
        }

        m_stageManager.CreateData(words);

        // 所持カードリスト作成
        GameInfo.Game.UserData[0].Card = CreateCardList();
        GameInfo.Game.UserData[1].Card = CreateCardList();

        // User1側で配置とカードリストを設定し、データを送信する。
        yield return m_GameDataSync.CoUpdateGameData(GameInfo.Game);
    }

    /// <summary>
    /// 使用可能カードリスト作成
    /// </summary>
    /// <returns></returns>
    private List<CardType> CreateCardList()
    {
        List<CardType> cards = new List<CardType>();
        // Noneを含まない
        for (int i = 1; i < Enum.GetValues(typeof(CardType)).Length; i++)
        {
            cards.Add((CardType)Enum.ToObject(typeof(CardType), i));
        }

        List<CardType> cardSet = new List<CardType>();
        // 初期カード数だけカードを設定
        for (int i = 0; i < HOLD_CARD_COUNT; i++)
        {
            int cardNo = UnityEngine.Random.Range(0, cards.Count);
            cardSet.Add(cards[cardNo]);
            cards.RemoveAt(cardNo);
        }

        return cardSet;
    }

    /// <summary>
    /// User2の初期化
    /// </summary>
    /// <returns></returns>
    private IEnumerator CoInitializeUser2()
    {
        // User1側で設定したデータを同期
        yield return m_GameDataSync.CoSetUpDataSyncUser2();
    }

    /// <summary>
    /// 前回のゲームから開始する。
    /// </summary>
    /// <returns></returns>
    private IEnumerator CoRestart()
    {
        // 再起動FlgをFalseに設定
        GameInfo.IsRestart = false;

        // 自分のターン中か調べる
        CheckMyTurn();

        // 設定されたデータを表示する
        VisibleDatas();

        // UIを表示する
        VisibleUI(true);

        if (CheckGameEnd())
        {
            yield return CoResult();
        }
        else
        {
            // ゲーム同期を開始する
            m_GameDataSync.StartGameSync();

            // 状態をGameプレイ状態に設定
            GameStatus = GameState.Game;
        }

        Debug.Log("前回のデータからゲームを開始しました");
        yield return null;
    }

    /// <summary>
    /// 設定されたデータを表示する
    /// </summary>
    private void VisibleDatas()
    {
        // 所持カードの表示
        m_cardGroupManager.Visible();

        m_stageManager.VisibleDatas();
    }

    //--------------------------------------------
    // ゲーム
    //--------------------------------------------

    /// <summary>
    /// ゲームの状態チェックと更新
    /// </summary>
    /// <param name="gameData"></param>
    /// <returns></returns>
    public IEnumerator CoSyncGame(GameData gameData)
    {
        if (!SyncCompletedFlg)
        {
            GameData oldGameData = GameInfo.Game;
            GameInfo.Game = gameData;

            CheckMyTurn();

            // ターンが変わっていない場合
            if (!SyncCompletedFlg)
            {
                // 相手ターン中であればカードボタンは非表示にする
                m_cardGroupManager.VisibleSkillCardButton(false);

                // カードが使用されていれば使用中表示する
                if(!oldGameData.IsUseCard && GameInfo.Game.IsUseCard != oldGameData.IsUseCard)
                {
                    VisibleUseSkill(true);
                }

                // 単語が選択されていれば同じように表示する
                for (int i = 0; i < GameInfo.Game.WordDatas.Count; i++)
                {
                    if (GameInfo.Game.WordDatas[i].Answer != oldGameData.WordDatas[i].Answer)
                    {
                        IsVisiblePopup = true;
                        m_stageManager.VisiblePopup(i);
                        break;
                    }
                }

                // スキルが使用されていれば同じように表示する
                if(GameInfo.Game.UseCard != oldGameData.UseCard)
                {
                    if (GameInfo.Game.UseCard != CardType.None)
                    {
                        m_cardGroupManager.VisibleCardPopup(GameInfo.Game.UseCard);
                    }
                }
            }
            else
            {
                // 自分ターン中で所持カードがあればカードボタンは表示する
                if(GameInfo.MyData.Card.Count > 0)
                {
                    m_cardGroupManager.VisibleSkillCardButton(true);
                }
            }

            // スコアが更新されていれば変動があったポイントを表示する
            int addScore = GameInfo.OpponentData.Score - oldGameData.UserData[GameInfo.OpponentUserNo].Score;
            if (addScore != 0)
            {
                m_scoreManager.AddScore(addScore, false);
            }

            if (!IsVisiblePopup && GameInfo.Game.Turn == Turn.Result)
            {
                yield return CoResult();
            }
        }
        yield return null;
    }

    /// <summary>
    /// ゲーム結果
    /// </summary>
    /// <returns></returns>
    public IEnumerator CoResult()
    {
        GameStatus = GameState.Result;

        // ゲームの同期を止める
        m_GameDataSync.StopGameSync();

        // UIを非表示にする
        VisibleUI(false);

        m_stageManager.VisibleResult();

        return null;
    }

    /// <summary>
    /// 自身のターンかどうか調べる。
    /// </summary>
    private void CheckMyTurn()
    {
        // 自分のターン中なら同期完了状態にする
        if (GameInfo.MyTurn == GameInfo.Game.Turn)
        {
            SyncCompletedFlg = true;
        }
        // 相手のターン中なら同期未完了状態にする
        else
        {
            SyncCompletedFlg = false;
        }
    }

    /// <summary>
    /// ゲームが終了したか調べる
    /// </summary>
    /// <returns>TRUE: ゲーム終了 FALSE: ゲーム続行</returns>
    public bool CheckGameEnd()
    {
        bool end = true;
        // すべて選択済みか
        for (int i = 0; i < GameInfo.Game.WordDatas.Count; i++)
        {
            if (!GameInfo.Game.WordDatas[i].Answer)
            {
                end = false;
                break;
            }
        }

        return end;
    }

    public void VisibleUseSkill(bool isVisible)
    {
        m_useSkillObject.SetActive(isVisible);
    }

    private void VisibleUI(bool isVisible)
    {
        m_uiObject.SetActive(isVisible);
    }
}