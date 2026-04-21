using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResultDialogManager : MonoBehaviour
{
    //--------------------------------------------
    // コンポーネント
    //--------------------------------------------

    /// <summary>
    /// ゲームデータ同期
    /// </summary>
    private GameDataSync m_gameDataSync;

    [SerializeField, Header("勝敗テキスト")]
    private Text m_messageText;

    [SerializeField, Header("ボタンテキスト")]
    private Text m_buttonText;

    [SerializeField, Header("ボタン")]
    private Button m_button;

    //--------------------------------------------
    // データ
    //--------------------------------------------

    private bool m_isContinue;

    //--------------------------------------------
    // 初期化
    //--------------------------------------------

    private void Awake()
    {
        m_button.onClick.AddListener(() => StartCoroutine(CoOnClickButton()));

        m_gameDataSync = FindObjectsOfType<GameDataSync>()[0];
    }

    public void Init(bool isContinue, string messageText, string buttonText)
    {
        m_isContinue = isContinue;
        m_messageText.text = messageText;
        m_buttonText.text = buttonText;
    }

    /// <summary>
    /// ボタン反応 表示/非表示
    /// </summary>
    private void SetIntaractableButton(bool enabled)
    {
        m_button.interactable = enabled;
    }

    //--------------------------------------------
    // ボタン
    //--------------------------------------------

    /// <summary>
    /// リトライボタン
    /// </summary>
    private IEnumerator CoOnClickButton()
    {
        SetIntaractableButton(false);
        AudioManager.I.PlaySe(AudioNames.ButtonSE);

        if (m_isContinue)
        {
            // コンテニュー
            GameInfo.Game.WinOrLose.Add(GameInfo.CheckWinner(false));
            GameInfo.Game.IsNextGame = true;
            GameInfo.Game.ResetData();
            yield return m_gameDataSync.CoUpdateGameData(GameInfo.Game);
        }
        else
        {
            // リトライ
            yield return m_gameDataSync.CoDeleteGameData();
        }

        SceneFadeManager.I.Load(SceneName.Room);
    }
}
