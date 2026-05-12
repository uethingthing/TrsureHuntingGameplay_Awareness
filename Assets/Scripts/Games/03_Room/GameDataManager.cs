using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;
using static RoomCanvasManager;

public class GameDataManager : UWRHelper
{
    //--------------------------------------------
    // enum
    //--------------------------------------------

    private enum RegistrationType
    {
        User01,
        User02,
    };

    private enum Select
    {
        Level,
        Turn
    }

    //--------------------------------------------
    // 定数
    //--------------------------------------------

    /// <summary>
    /// User1
    /// </summary>
    public const string USER_1 = "User1";

    /// <summary>
    /// User2
    /// </summary>
    public const string USER_2 = "User2";

    private const int MAX_STAGE = 3;

    //--------------------------------------------
    // コンポーネント
    //--------------------------------------------

    [SerializeField, Header("キャンバス管理")]
    private RoomCanvasManager m_roomCanvasManager;

    [SerializeField, Header("先行/後攻を設定するUI")]
    private SelectTurnUI m_selectTurnUI;

    [SerializeField, Header("レベルを設定するUI")]
    private SelectLevelUI m_selectLevelUI;

    //--------------------------------------------
    // 処理
    //--------------------------------------------

    public UnityAction ErrorAction { get; set; }

    //--------------------------------------------
    // 設定
    //--------------------------------------------

    /// <summary>
    /// デバッグ用user2
    /// </summary>
    private string m_user2;


    //--------------------------------------------
    // 変数
    //--------------------------------------------

    /// <summary>
    /// ステージ番号保持用
    /// </summary>
    private static int m_StageNoCache = -1;

    //--------------------------------------------
    // 初期化
    //--------------------------------------------

    private void Start()
    {
        // デバッグ用
        if (GameInfo.IsSingleMode)
        {
            while (true)
            {
                m_user2 = UnityEngine.Random.Range(1111, 10000).ToString();
                if (GameInfo.MyUserID != m_user2)
                {
                    break;
                }
            }
        }
    }

    //--------------------------------------------
    // ユーザー登録
    //--------------------------------------------

    /// <summary>
    /// ユーザー登録
    /// </summary>
    /// <returns></returns>
    public IEnumerator CoEntryUser()
    {
        IEnumerator CoEntryUser(UnityWebRequest uwr)
        {
            GameData gameData = GameData.FromJsonConvert(JsonNode.GetValue(uwr.downloadHandler.text));

            // 各UserにIDが設定されていなければ...
            // IDを設定、ルームデータ更新
            if (string.IsNullOrEmpty(gameData.UserID_01))
            {
                SettingUser(RegistrationType.User01);
                gameData.UserID_01 = GameInfo.MyUserID;
                // ゲームデータを削除するタイムリミットを24時間後に設定(#72対応)
                gameData.TimeLimit = DateTime.Now.AddHours(24).ToString();

                // シングルモード使用時設定
                if (GameInfo.IsSingleMode)
                {
                    gameData.UserID_02 = m_user2;
                }
            }
            else if (string.IsNullOrEmpty(gameData.UserID_02) && GameInfo.MyUserID != gameData.UserID_01)
            {
                SettingUser(RegistrationType.User02);
                gameData.UserID_02 = GameInfo.MyUserID;
            }
            // User1が入室した後、User2が入室する前にゲームをリロードし、User1の情報がある状態で再度登録しようとしたとき
            // 先攻後攻を選択していないはずなので、TurnがNoneから変わっていなければUser1として通す
            else if (string.IsNullOrEmpty(gameData.UserID_02) && gameData.UserID_01 == GameInfo.MyUserID && gameData.Turn == Turn.None)
            {
                SettingUser(RegistrationType.User01);
            }
            // 対戦相手とユーザーIDが同じならタイトルに戻る
            else
            {
                // エラー処理
                UtilityManager.I.ErrorMessageBox("User Error", $"I'm trying to register the same user ID. \nBack to the title.", ErrorAction);
                while (true) { yield return null; }
            }

            if (GameInfo.CheckPlayerType)
            {
                // 次にプレイヤータイプの確認
                yield return CoCheckPlayerType(gameData);
            }
            else
            {
                // プレイヤータイプの情報が不要な場合はここでデータ送信
                yield return CoUpdateGameData(gameData);
            }
        }

        yield return CoGetData(CoEntryUser);
    }

    /// <summary>
    /// プレイヤータイプの確認
    /// </summary>
    /// <returns></returns>
    private IEnumerator CoCheckPlayerType(GameData gameData)
    {
        // プレイヤータイプが設定されていなかったらエラー
        if (!(GameInfo.MyPlayerType == PlayerType.Teacher || GameInfo.MyPlayerType == PlayerType.Student))
        {
            UtilityManager.I.ErrorMessageBox("User Error", $"Player type is not set. \nBack to the title.", ErrorAction);
            while (true) { yield return null; }
        }

        // User1の場合は、そのままPlayerType設定
        if (GameInfo.MyTurn == Turn.User01)
        {
            gameData.User1PlayerType = GameInfo.MyPlayerType;

            // シングルモード使用時設定
            if (GameInfo.IsSingleMode)
            {
                gameData.User2PlayerType = GameInfo.MyPlayerType == PlayerType.Teacher ? PlayerType.Student : PlayerType.Teacher;
            }
        }
        // User2の場合
        // User1とプレイヤータイプが被っていないかチェック
        else if (GameInfo.MyTurn == Turn.User02 && GameInfo.MyPlayerType != gameData.User1PlayerType)
        {
            gameData.User2PlayerType = GameInfo.MyPlayerType;
        }
        else
        {
            // エラー処理
            UtilityManager.I.ErrorMessageBox("User Error", $"Same player type as opponent. \nBack to the title.", ErrorAction);
            while (true) { yield return null; }
        }

        m_roomCanvasManager.ChangePlayerTypeText(GameInfo.MyPlayerType.ToString());
        yield return CoUpdateGameData(gameData);
    }

    //--------------------------------------------
    // マッチング
    //--------------------------------------------

    /// <summary>
    /// マッチング処理
    /// </summary>
    /// <returns></returns>
    public IEnumerator CoMatching()
    {
        IEnumerator CoMatching(UnityWebRequest uwr)
        {
            GameData gameData = GameData.FromJsonConvert(JsonNode.GetValue(uwr.downloadHandler.text));

            // 重複入室対応用
            // User1に登録したはずのユーザー情報が上書きされていたら、再度User2として登録する。
            if (GameInfo.MyTurn == Turn.User01 && gameData.UserID_01 != GameInfo.MyUserID)
            {
                if (string.IsNullOrEmpty(gameData.UserID_02) && GameInfo.MyUserID != gameData.UserID_01)
                {
                    SettingUser(RegistrationType.User02);
                    gameData.UserID_02 = GameInfo.MyUserID;
                    gameData.User2PlayerType = GameInfo.MyPlayerType;
                    Debug.Log("ユーザー情報が上書きされていました。再度User2として登録します。");
                    yield return CoUpdateGameData(gameData);
                }
                // 対戦相手とユーザーIDが同じならタイトルに戻る
                else
                {
                    // UserIDが同じと警告するエラーメッセージ
                    UtilityManager.I.ErrorMessageBox("User Error", $"I'm trying to register the same user ID. \nBack to the title.", ErrorAction);
                    while (true) { yield return null; }
                }
            }

            if (!string.IsNullOrEmpty(gameData.UserID_01) && !string.IsNullOrEmpty(gameData.UserID_02))
            {
                GameInfo.Game.UserID_01 = gameData.UserID_01;
                GameInfo.Game.UserID_02 = gameData.UserID_02;
            }

            yield return null;
        }

        // 無限ループ
        // ゲームデータを同期させる
        while (true)
        {
            yield return CoGetData(CoMatching);

            // 両ユーザーが登録されていたら処理を抜ける
            if (!string.IsNullOrEmpty(GameInfo.Game.UserID_01) && !string.IsNullOrEmpty(GameInfo.Game.UserID_02))
            {
                yield break;
            }

            yield return new WaitForSeconds(DefaultSyncSecond);
        }
    }

    //--------------------------------------------
    // レベル設定
    //--------------------------------------------

    /// <summary>
    /// レベル選択
    /// </summary>
    /// <returns></returns>
    public IEnumerator CoStartSelectLevel()
    {
        IEnumerator CoSelectLevel()
        {
            m_roomCanvasManager.ChangeView(Room.SelectLevel);
            yield return m_selectLevelUI.CoSelect();
        }

        // 生徒が設定
        // 先生は設定されたデータを同期する
        switch (GameInfo.MyPlayerType)
        {
            case PlayerType.Student:
                yield return CheckTimeLimit(CoSelectLevel);
                // ステージを決める 連続して同じステージにならないように同じステージなら引き直す
                int randomStage = 0;
                do
                {
                    randomStage = UnityEngine.Random.Range(0, MAX_STAGE);
                } while (m_StageNoCache == randomStage);
                m_StageNoCache = randomStage;
                GameInfo.Game.StageNo = randomStage;
                yield return CoUpdateGameData(GameInfo.Game);
                break;

            case PlayerType.Teacher:
                // 相手の選択を待つUI表示
                m_roomCanvasManager.ChangeView(Room.WaitingSelectTurn);
                yield return CheckTimeLimit(CoSettingLevelSync);
                break;
        }
    }

    /// <summary>
    /// 設定を同期する
    /// 設定されるまで待機する
    /// </summary>
    /// <returns></returns>
    private IEnumerator CoSettingLevelSync()
    {
        IEnumerator CoSettingLevel(UnityWebRequest uwr)
        {
            GameData gameData = GameData.FromJsonConvert(JsonNode.GetValue(uwr.downloadHandler.text));

            // User1側でレベルを設定していればゲームを同期させる
            if (gameData.SelectLevel != -1)
            {
                GameInfo.Game.SelectLevel = gameData.SelectLevel;
                GameInfo.Game.StageNo = gameData.StageNo;
            }

            yield return null;
        }

        // ゲームデータ内容を同期するまでループ
        while (true)
        {
            yield return CoGetData(CoSettingLevel);
            if (GameInfo.Game.SelectLevel != -1)
            {
                break;
            }

            yield return new WaitForSeconds(DefaultSyncSecond);
        }
    }

    //--------------------------------------------
    // ターン設定
    //--------------------------------------------

    /// <summary>
    /// 先攻後攻選択
    /// </summary>
    /// <returns></returns>
    public IEnumerator CoStartSelectTurn(bool isCheckUser)
    {
        IEnumerator CoSelectTurn()
        {
            m_roomCanvasManager.ChangeView(Room.SelectTurn);
            yield return m_selectTurnUI.CoSelect();
        }

        if (CheckTurnSetting(isCheckUser))
        {
            yield return CheckTimeLimit(CoSelectTurn);
            yield return CoUpdateGameData(GameInfo.Game);
        }
        else
        {
            // 相手の選択を待つUI表示
            m_roomCanvasManager.ChangeView(Room.WaitingSelectTurn);
            yield return CheckTimeLimit(CoSettingTurnSync);
        }
    } 

    /// <summary>
    /// 設定を同期する
    /// 設定されるまで待機する
    /// </summary>
    /// <returns></returns>
    private IEnumerator CoSettingTurnSync()
    {
        IEnumerator CoSettingTurn(UnityWebRequest uwr)
        {
            GameData gameData = GameData.FromJsonConvert(JsonNode.GetValue(uwr.downloadHandler.text));

            // User1側で先行/後攻を設定していればゲームを同期させる
            if (gameData.Turn != Turn.None)
            {
                GameInfo.Game.Turn = gameData.Turn;
            }

            yield return null;
        }

        // ゲームデータ内容を同期するまでループ
        while (true)
        {
            yield return CoGetData(CoSettingTurn);
            if (GameInfo.Game.Turn != Turn.None)
            {
                break;
            }

            yield return new WaitForSeconds(DefaultSyncSecond);
        }
    }

    /// <summary>
    /// 設定できるかをチェックする
    /// </summary>
    /// <param name="isCheckUser"></param>
    /// <returns></returns>
    private bool CheckTurnSetting(bool isCheckUser)
    {
        bool setting = false;
        if (isCheckUser)
        {
            // User1で設定
            // User2は設定されたデータを同期する
            if (GameInfo.MyTurn == Turn.User01)
            {
                setting = true;
            }
        }
        else
        {
            // 最後の試合の勝者、もしくはドローだった場合user1がターンを選ぶ
            Turn winner = GameInfo.Game.WinOrLose[GameInfo.Game.WinOrLose.Count - 1];
            if (winner == GameInfo.MyTurn || (winner == Turn.None && GameInfo.MyTurn == Turn.User01))
            {
                setting = true;
            }
        }
        return setting;
    }

    //--------------------------------------------
    // 再起動
    //--------------------------------------------

    /// <summary>
    /// ゲームを再起動（ゲームを前回の状態からプレイ）するか調べる
    /// </summary>
    /// <returns></returns>
    public IEnumerator CoCheckRestart()
    {
        IEnumerator CoCheckRestart(UnityWebRequest uwr)
        {
            // 別のゲームデータがサーバー上に格納されていたらゲームデータを削除する。
            if (CheckDifferenceGameData(uwr))
            {
                Debug.Log("別のゲームデータがサーバー上にあったため、ゲームデータを削除します。");
                yield return CoDeleteGameData();
            }
            // 正常なゲームデータが格納されていたら...
            else
            {
                GameData gameData = GameData.FromJsonConvert(JsonNode.GetValue(uwr.downloadHandler.text));

                if (!string.IsNullOrEmpty(gameData.TimeLimit))
                {
                    DateTime limit = DateTime.Parse(gameData.TimeLimit);
                    // タイムリミットを超えていた場合はデータを削除(#72対応)
                    if (DateTime.Now > limit)
                    {
                        yield return CoDeleteGameData();
                        Debug.Log("24時間が立ちましたゲームデータを削除");
                        yield break;
                    }
                }

                // 両ユーザIDが設定されている
                if (!string.IsNullOrEmpty(gameData.UserID_01) && !string.IsNullOrEmpty(gameData.UserID_02))
                {
                    // 使用されているゲームデータのRoomIDが前回使用したRoomIdと一致しているか
                    if (KeyData.GameKey == gameData.RoomID)
                    {
                        // 前回使用したUserIdがUserID_01と一致したなら前回の状態からUser1としてゲームに復帰する
                        if (GameInfo.MyUserID == gameData.UserID_01)
                        {
                            GameInfo.MyPlayerType = gameData.User1PlayerType;
                            SettingUser(RegistrationType.User01);
                            GameInfo.IsRestart = true;
                        }
                        // 前回使用したUserIdがUserID_02と一致したなら前回の状態からUser2としてゲームに復帰する
                        else if (GameInfo.MyUserID == gameData.UserID_02)
                        {
                            GameInfo.MyPlayerType = gameData.User2PlayerType;
                            SettingUser(RegistrationType.User02);
                            GameInfo.IsRestart = true;
                        }

                        if (GameInfo.IsRestart)
                        {
                            m_roomCanvasManager.ChangePlayerTypeText(GameInfo.MyPlayerType.ToString());

                            if (gameData.Turn != Turn.None)
                            {
                                // 先行/後攻が設定されていればゲーム復帰
                                yield return CoSetGameInfoGame();
                                yield return CoGoToGameScene();
                                while (true) { yield return null; }
                            }
                            else if (gameData.IsNextGame)
                            {
                                // 先行/後攻が設定されておらず、次のゲームフラグが立っていれば先行/後攻設定し、ゲームへ
                                yield return CoSetGameInfoGame();
                                GameInfo.Game.IsNextGame = false;
                                GameInfo.IsRestart = false;
                                yield return CoStartSelectTurn(false);
                                yield return CoGoToGameScene();
                                while (true) { yield return null; }
                            }
                        }
                    }

                    GameInfo.IsRestart = false;
                    // それ以外ならゲームデータを削除
                    yield return CoDeleteGameData();
                    yield return new WaitForSeconds(DefaultSyncSecond);
                }
            }
        }

        yield return CoGetData(CoCheckRestart);
    }

    /// <summary>
    /// ユーザーの設定
    /// </summary>
    /// <param name="type"></param>
    private void SettingUser(RegistrationType type)
    {
        if(type == RegistrationType.User01)
        {
            GameInfo.MyTurn = Turn.User01;
            m_roomCanvasManager.ChangeUserNameText(USER_1);
        }
        else
        {
            GameInfo.MyTurn = Turn.User02;
            m_roomCanvasManager.ChangeUserNameText(USER_2);
        }
    }

    //--------------------------------------------
    // ゲームデータ
    //--------------------------------------------

    /// <summary>
    /// ゲームデータが消されていない調べる
    /// 消されていたら処理を中断し、タイトルに戻る
    /// </summary>
    /// <returns></returns>
    public IEnumerator CoCheckDeleteGame()
    {
        // 無限ループ
        while (true)
        {
            var uwr = CreateGetUrl(KeyData.GameKey);
            yield return WaitForRequest(uwr);

            // ゲームデータが無ければエラーメッセージを表示
            if (!CheckKey(uwr))
            {
                UtilityManager.I.ErrorMessageBox("Time out", $"There is no response from the opponent.\nReturn to the title.", ErrorAction);
                while (true) { yield return null; }
            }
            yield return new WaitForSeconds(DefaultSyncSecond);
        }
    }

    /// <summary>
    /// ゲームチェック
    /// </summary>
    /// <returns></returns>
    public IEnumerator CoCheckGame()
    {
        var uwr = CreateGetUrl(KeyData.GameKey);
        yield return WaitForRequest(uwr);

        // ゲームデータ作成済みならこの処理を抜ける
        if (CheckKey(uwr)) { yield break; }

        // ゲームデータ作成
        yield return CoCreateGameData();
    }

    /// <summary>
    /// ゲームデータ作成
    /// </summary>
    public IEnumerator CoCreateGameData()
    {
        GameData data = new GameData();
        data.RoomID = KeyData.GameKey;

        var uwr = CreateSetUrl(KeyData.GameKey, GameData.ToJsonConvert(data));
        Debug.Log("ゲームデータ作成 , 使用URL : " + uwr.url);
        yield return WaitForRequest(uwr);
    }

    /// <summary>
    /// ゲームデータ更新
    /// </summary>
    /// <param name="gameData"></param>
    /// <returns></returns>
    public IEnumerator CoUpdateGameData(GameData gameData)
    {
        var uwr = CreateSetUrl(KeyData.GameKey, GameData.ToJsonConvert(gameData));
        Debug.Log("ゲームデータ更新 , 使用URL : " + uwr.url);
        yield return WaitForRequest(uwr);

        if (!CheckKey(uwr))
        {
            // エラー処理
            UtilityManager.I.ErrorMessageBox("Communication Error", $"No game information found. \nBack to the title.", ErrorAction);
            while (true) { yield return null; }
        }
    }

    /// <summary>
    /// ゲームデータを削除
    /// </summary>
    /// <returns></returns>
    public IEnumerator CoDeleteGameData()
    {
        // ゲームデータ削除
        var uwr = CreateDeletUrl(KeyData.GameKey);
        Debug.Log("ゲームデータ削除 , 使用URL : " + uwr.url);
        yield return WaitForRequest(uwr);
    }

    /// <summary>
    /// ゲーム情報のGameDataを設定
    /// データベース上のゲームデータを取得し、GameInfo.Gameに格納している。
    /// </summary>
    /// <returns></returns>
    public IEnumerator CoSetGameInfoGame()
    {
        // ゲームデータを取得し格納する。
        while (true)
        {
            var uwr = CreateGetUrl(KeyData.GameKey);
            yield return WaitForRequest(uwr);

            if (CheckKey(uwr))
            {
                // データベース上のゲームデータ格納
                GameInfo.Game = GameData.FromJsonConvert(JsonNode.GetValue(uwr.downloadHandler.text));
                break;
            }
        }
    }

    /// <summary>
    /// データ取得
    /// </summary>
    /// <param name="func"></param>
    /// <returns></returns>
    private IEnumerator CoGetData(Func<UnityWebRequest, IEnumerator> func, bool checkKey = true)
    {
        UnityWebRequest uwr = CreateGetUrl(KeyData.GameKey);
        yield return WaitForRequest(uwr);

        if (CheckKey(uwr) == checkKey)
        {
            yield return func.Invoke(uwr);
        }
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
}
