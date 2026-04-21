using UnityEngine.UI;
using UnityEngine;
using System.Collections;

/// <summary>
/// ゲームデータを表示する
/// </summary>
public class GameDataView : UWRHelper
{
    [SerializeField, Header("ゲームデータ表示用テキスト")]
    private Text m_gameDataText = null;

    /// <summary>
    /// コルーチン
    /// </summary>
    private IEnumerator m_Coroutine = null;

    /// <summary>
    /// 表示
    /// </summary>
    private void OnEnable()
    {
        StartCoroutine();
    }

    /// <summary>
    /// 非表示
    /// </summary>
    private void OnDisable()
    {
        EndCoroutine();
    }

    /// <summary>
    /// コルーチン開始
    /// </summary>
    private void StartCoroutine()
    {
        EndCoroutine();
        m_Coroutine = CoGameDataView();
        StartCoroutine(m_Coroutine);
    }

    /// <summary>
    /// コルーチンを終了する
    /// </summary>
    private void EndCoroutine()
    {
        if (m_Coroutine != null)
        {
            StopCoroutine(m_Coroutine);
            m_Coroutine = null;
        }
    }

    /// <summary>
    /// サーバー情報のゲームデータを表示
    /// </summary>
    /// <returns></returns>
    private IEnumerator CoGameDataView()
    {
        while (true)
        {
            var uwr = CreateGetUrl(KeyData.GameKey);
            yield return WaitForRequest(uwr);

            if (CheckKey(uwr))
            {
                // このゲームではなく別のゲームデータがサーバー上に格納されていたら...
                if (CheckDifferenceGameData(uwr))
                {
                    m_gameDataText.text = "There is another game data on the server.";
                }
                // 正常なゲームデータがサーバー上に格納されていたら...
                else
                {
                    GameData gameData = GameData.FromJsonConvert(JsonNode.GetValue(uwr.downloadHandler.text));
                    string text = gameData.GetStr();
                    text +=
                        $"InfoData\n" +
                        $" MyRoomID: {KeyData.GameKey}\n" +
                        $" {nameof(GameInfo.MyUserID)}: {GameInfo.MyUserID}\n" +
                        $" {nameof(GameInfo.MyPlayerType)}: {GameInfo.MyPlayerType}\n" +
                        $" {nameof(GameInfo.MyTurn)}: {GameInfo.MyTurn}\n";
                    m_gameDataText.text = text;
                }
            }
            else
            {
                m_gameDataText.text = "No GameData.";
            }

            yield return new WaitForSeconds(DefaultSyncSecond);
        }
    }
}