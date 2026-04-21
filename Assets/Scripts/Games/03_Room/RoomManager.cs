using UnityEngine;
using System.Collections;
using static RoomCanvasManager;
using static GameDataManager;

/// <summary>
/// Room管理クラス
/// </summary>
public class RoomManager : MonoBehaviour
{
    //--------------------------------------------
    // コンポーネント
    //--------------------------------------------

    [SerializeField, Header("キャンバス管理")]
    private RoomCanvasManager m_roomCanvasManager;

    private UserDataManager m_userDataManager;

    private GameDataManager m_gameDataManager;

    //--------------------------------------------
    // コルーチン
    //--------------------------------------------

    /// <summary>
    /// コルーチン
    /// </summary>
    private IEnumerator m_Coroutine = null;

    //--------------------------------------------
    // 初期化
    //--------------------------------------------

    void Awake()
    {
        m_userDataManager = GetComponent<UserDataManager>();
        m_gameDataManager = GetComponent<GameDataManager>();

        m_userDataManager.ErrorAction = ChangeTitleScene;
        m_gameDataManager.ErrorAction = ChangeTitleScene;
    }

    /// <summary>
    /// Start
    /// </summary>
    protected void Start()
    {
        AudioManager.I.PlayBgm(AudioNames.MainBgm, 0.5f);
        StartMatching();
    }

    //--------------------------------------------
    // マッチング
    //--------------------------------------------

    /// <summary>
    /// マッチング開始
    /// </summary>
    private void StartMatching()
    {
        m_roomCanvasManager.ChangeView(Room.GetUserData);
        StopMatching();
        m_Coroutine = CoInitialize();
        StartCoroutine(CoInitialize());
    }

    /// <summary>
    /// マッチング中止
    /// </summary>
    private void StopMatching()
    {
        if (m_Coroutine != null)
        {
            StopCoroutine(m_Coroutine);
            m_Coroutine = null;
        }
    }

    /// <summary>
    /// 初期化
    /// </summary>
    /// <returns></returns>
    private IEnumerator CoInitialize()
    {
        // シーン遷移が完了するまで待機...
        yield return new WaitUntil(() => !SceneFadeManager.I.IsFading);

        if (!GameInfo.Game.IsNextGame)
        {
            // ゲームデータ初期化
            GameInfo.Init();

            // ゲームKey取得
            yield return m_userDataManager.CoGetGameKey();

            // ユーザーID取得
            yield return m_userDataManager.CoGetUserID();

            if (GameInfo.CheckPlayerType)
            {
                // プレイヤータイプ（講師か生徒か）取得
                yield return m_userDataManager.CoGetPlayerType();
            }

            // ユーザーデータ取得完了したので対戦相手接続待ちUI表示
            m_roomCanvasManager.ChangeView(Room.WaitingConnect);

            // ゲームを前回の状態から開始するか調べ、前回の状態から開始しない場合はゲームデータを削除する
            yield return m_gameDataManager.CoCheckRestart();

            // 入室処理
            yield return CoEntryRoom();
        }
        else
        {
            // continueで遷移した場合は先攻後攻のみ選択する
            GameInfo.Game.IsNextGame = false;

            if(GameInfo.MyTurn == Turn.User01)
            {
                m_roomCanvasManager.ChangeUserNameText(USER_1);
            }
            else
            {
                m_roomCanvasManager.ChangeUserNameText(USER_2);
            }

            yield return m_gameDataManager.CoStartSelectTurn(false);
            Debug.Log("先行/後攻選択完了");
        }

        // ゲームシーンへ遷移
        yield return CoGoToGameScene();
    }

    /// <summary>
    /// ルーム入室
    /// </summary>
    /// <returns></returns>
    private IEnumerator CoEntryRoom()
    {
        yield return m_gameDataManager.CoCheckGame();
        Debug.Log("ゲームチェック完了");

        // ここでゲームデータが削除されたらタイトルに戻るように設定している。
        StartCoroutine(m_gameDataManager.CoCheckDeleteGame());

        yield return m_gameDataManager.CoEntryUser();
        Debug.Log("ユーザ登録完了");
        yield return m_gameDataManager.CoMatching();
        Debug.Log("マッチング完了");
        yield return m_gameDataManager.CoSetGameInfoGame();
        Debug.Log("ゲームデータ取得完了");
        yield return m_gameDataManager.CoStartSelectLevel();
        Debug.Log("レベル選択完了");
        yield return m_gameDataManager.CoStartSelectTurn(true);
        Debug.Log("先行/後攻選択完了");
    }

    //--------------------------------------------
    // 遷移
    //--------------------------------------------

    /// <summary>
    /// ゲームシーンへ遷移
    /// </summary>
    /// <returns></returns>
    private IEnumerator CoGoToGameScene()
    {
        // ユーザー登録されていたらゲームシーン遷移。されていなければこのシーンを再度読み込み
        if (GameInfo.MyUserID != string.Empty)
        {
            SceneFadeManager.I.Load(SceneName.Game);
        }
        yield break;
    }

    /// <summary>
    /// タイトルシーンへ遷移
    /// </summary>
    public void ChangeTitleScene()
    {
        SceneFadeManager.I.Load(SceneName.Title);
        StopMatching();
    }
}
