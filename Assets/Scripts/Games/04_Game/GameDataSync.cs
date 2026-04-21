using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.Networking;

/// <summary>
/// ゲームデータを同期するクラス
/// </summary>
public class GameDataSync : UWRHelper
{
    //--------------------------------------------
    // 定数
    //--------------------------------------------

    /// <summary>
    /// この回数以上CheckKeyでエラーが起こったら、エラーメッセージを出してゲームを止める
    /// </summary>
    private const int m_RimitCheckKeyError = 5;

    //--------------------------------------------
    // データ
    //--------------------------------------------

    /// <summary>
    /// CheckKeyでエラーが起こった回数
    /// </summary>
    private int m_CheckKeyErrorCount;

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

    /// <summary>
    /// Start
    /// </summary>
    private void Start()
    {
        // 通信チェックを開始。
        // ゲームが開始された終了する。
        StartCheckConnecting();
    }

    //--------------------------------------------
    // 通信
    //--------------------------------------------

    /// <summary>
    /// 通信チェックを開始する。
    /// サーバーと接続されているかチェックする
    /// ゲーム同期を開始するタイミングでこのコルーチンを終了する
    /// </summary>
    private void StartCheckConnecting()
    {
        EndCoroutine();
        m_Coroutine = CoGetData(CoCheckConnecting, CoConnectingError);
        StartCoroutine(m_Coroutine);
    }

    /// <summary>
    /// エラー時
    /// </summary>
    /// <returns></returns>
    private IEnumerator CoConnectingError()
    {
        Debug.Log("<color=red>CoGetDataでエラー : " + m_CheckKeyErrorCount + "回目</color>");

        // エラー回数をカウント
        m_CheckKeyErrorCount++;

        // 連続エラー回数が上限を超えたらエラーメッセージ表示
        if (m_RimitCheckKeyError < m_CheckKeyErrorCount)
        {
            UtilityManager.I.ErrorMessageBox("Communication Error", $"Your opponent has exited the game. \nComing back to the matching screen.", () => SceneFadeManager.I.Load(SceneName.Room));
            while (true) { yield return null; }
        }
    }

    /// <summary>
    /// サーバーと接続されているとき
    /// </summary>
    /// <param name="uwr"></param>
    /// <returns></returns>
    private IEnumerator CoCheckConnecting(UnityWebRequest uwr)
    {
        // 正しく処理できたらエラー回数リセット
        m_CheckKeyErrorCount = 0;
        yield return null;
    }

    /// <summary>
    /// 初期化時にゲームデータを同期させる
    /// User2側で呼び出される
    /// </summary>
    /// <returns></returns>
    public IEnumerator CoSetUpDataSyncUser2()
    {
        // ゲームデータ内容を同期するまでループ
        while (true)
        {
            var uwr = CreateGetUrl(KeyData.GameKey);
            yield return WaitForRequest(uwr);

            if (CheckKey(uwr))
            {
                GameData gameData = GameData.FromJsonConvert(JsonNode.GetValue(uwr.downloadHandler.text));

                // データが作成されていれば更新
                if (gameData.WordDatas.Count != 0)
                {
                    GameInfo.Game = gameData;
                    break;
                }
            }

            yield return new WaitForSeconds(DefaultSyncSecond);
        }
    }

    /// <summary>
    /// ゲーム同期を開始する
    /// 指定秒数間隔で呼び出されるメソッド
    /// </summary>
    public void StartGameSync()
    {
        EndCoroutine();
        m_Coroutine = CoGetData(CoGameDataSync, CoConnectingError);
        StartCoroutine(m_Coroutine);
    }

    /// <summary>
    /// ゲームデータを同期させる
    /// </summary>
    /// <param name="uwr"></param>
    /// <returns></returns>
    private IEnumerator CoGameDataSync(UnityWebRequest uwr)
    {
        GameData gameData = GameData.FromJsonConvert(JsonNode.GetValue(uwr.downloadHandler.text));

        // 正しく処理できたらエラー回数リセット
        m_CheckKeyErrorCount = 0;
        yield return GameManager.I.CoSyncGame(gameData);
    }

    /// <summary>
    /// リザルト画面チェックする。
    /// </summary>
    public void StartCheckResult()
    {
        EndCoroutine();
        m_Coroutine = CoGetData(CoCheckResult, CoCheckResultNoData);
        StartCoroutine(m_Coroutine);
    }

    /// <summary>
    /// ゲームデータがあるとき
    /// </summary>
    /// <param name="uwr"></param>
    /// <returns></returns>
    private IEnumerator CoCheckResult(UnityWebRequest uwr)
    {
        GameData gameData = GameData.FromJsonConvert(JsonNode.GetValue(uwr.downloadHandler.text));

        if (gameData.IsNextGame == true)
        {
            // 次のゲームフラグがtrueならば、対戦相手がコンテニューしたという事
            // 自身もコンテニューするために、Roomシーンに遷移
            GameInfo.Game = gameData;
            SceneFadeManager.I.Load(SceneName.Room);
            while (true) { yield return null; }
        }
    }

    /// <summary>
    /// ゲームデータがないとき
    /// </summary>
    /// <returns></returns>
    private IEnumerator CoCheckResultNoData()
    {
        // ゲームデータが無ければ、対戦相手がリトライしたという事
        // 自身もリトライするために、Roomシーンに遷移
        SceneFadeManager.I.Load(SceneName.Room);
        while (true) { yield return null; }
    }

    /// <summary>
    /// ゲーム同期を停止する
    /// </summary>
    public void StopGameSync()
    {
        EndCoroutine();
    }
    
    /// <summary>
    /// コルーチンを終了する
    /// </summary>
    public void EndCoroutine()
    {
        if(m_Coroutine != null)
        {
            StopCoroutine(m_Coroutine);
            m_Coroutine = null;
        }
    }

    /// <summary>
    /// データ取得
    /// </summary>
    /// <param name="normalFunc"></param>
    /// <param name="errorFunc"></param>
    /// <returns></returns>
    private IEnumerator CoGetData(Func<UnityWebRequest, IEnumerator> normalFunc, Func<IEnumerator> errorFunc)
    {
        // 無限ループ
        while (true)
        {
            var uwr = CreateGetUrl(KeyData.GameKey);
            yield return WaitForRequest(uwr);

            if (CheckKey(uwr))
            {
                yield return normalFunc.Invoke(uwr);
            }
            else
            {
                yield return errorFunc.Invoke();
            }
            yield return new WaitForSeconds(DefaultSyncSecond);
        }
    }

    /// <summary>
    /// ゲームデータを更新する
    /// </summary>
    /// <returns></returns>
    public IEnumerator CoUpdateGameData(GameData gameData)
    {
        var uwr = CreateSetUrl(KeyData.GameKey, GameData.ToJsonConvert(gameData));
        yield return WaitForRequest(uwr);
        //yield return new WaitForSeconds(DefaultSyncSecond);
    }

    /// <summary>
    /// ゲームデータを削除
    /// </summary>
    /// <returns></returns>
    public IEnumerator CoDeleteGameData()
    {
        var uwr = CreateDeletUrl(KeyData.GameKey);
        yield return WaitForRequest(uwr);
    }
}
