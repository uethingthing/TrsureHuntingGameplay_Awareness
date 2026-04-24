using System.Collections;
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

    [SerializeField, Header("ボタン")]
    private Button m_button;

    [SerializeField, Header("TouchScreen")]
    private Button m_touchScreenButton;

    //--------------------------------------------
    // データ
    //--------------------------------------------

    private bool m_isContinue = false;

    private bool m_isProcessing = false;

    //--------------------------------------------
    // 初期化
    //--------------------------------------------

    private void Awake()
    {
        m_button.onClick.AddListener(() => OnClickButton());
        m_touchScreenButton.onClick.AddListener(() => OnClickButton());

        m_gameDataSync = FindObjectsOfType<GameDataSync>()[0];
    }

    public void Init(bool isContinue, string messageText, string buttonText)
    {
        m_isContinue = isContinue;
    }

    public void SetRetryButton()
    {
        bool isRetry = GameInfo.Game.WinOrLose.Count == 2;
        m_button.gameObject.SetActive(isRetry);
        m_touchScreenButton.gameObject.SetActive(!isRetry);
    }

    /// <summary>
    /// ボタン反応 表示/非表示
    /// </summary>
    private void SetIntaractableButton(bool enabled)
    {
        m_button.interactable = enabled;
        m_touchScreenButton.interactable = enabled;
    }

    //--------------------------------------------
    // ボタン
    //--------------------------------------------
    private void OnClickButton()
    {
        if (m_isProcessing)
            return;

        StartCoroutine(CoOnClickButton());
    }

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
