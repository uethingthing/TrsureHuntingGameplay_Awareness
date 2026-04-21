using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

/// <summary>
/// 結果表示用UI
/// </summary>
public class ResultManager : MonoBehaviour
{
    private enum Result
    {
        Win,
        Lose,
        Draw
    }

    //--------------------------------------------
    // コンポーネント
    //--------------------------------------------

    /// <summary>
    /// ゲームデータ同期
    /// </summary>
    private GameDataSync m_gameDataSync;

    [SerializeField]
    private ResultDialogManager m_resultDialogManager;

    [SerializeField]
    private Image m_resultImage;

    [SerializeField]
    private Image m_resultBgImage;

    [SerializeField, Header("リザルト文言画像")]
    private Sprite[] m_sprites;

    [SerializeField, Header("リザルトBG画像")]
    private Sprite[] m_bgSprites;

    //--------------------------------------------
    // 初期化
    //--------------------------------------------

    private void Awake()
    {
        m_gameDataSync = FindObjectsOfType<GameDataSync>()[0];

        gameObject.SetActive(false);
    }

    public void Visible()
    {
        // 結果画面で対戦相手がリトライボタンを押したら
        // 自身も出るようにしている。
        m_gameDataSync.StartCheckResult();

        bool total = true;
        bool isContinue = false;
        string messageText = "DRAW";
        string buttonText = "Retry";
        m_resultImage.sprite = m_sprites[(int)Result.Draw];
        m_resultBgImage.sprite = m_bgSprites[(int)Result.Draw];

        // 再戦ボタンを表示
        if (GameInfo.Game.WinOrLose.Count < 2)
        {
            total = false;
            buttonText = "Continue";
            isContinue = true;
        }

        Turn winner = GameInfo.CheckWinner(total);
        if (winner == GameInfo.MyTurn)
        {
            messageText = "WIN";
            m_resultImage.sprite = m_sprites[(int)Result.Win];
            m_resultBgImage.sprite = m_bgSprites[(int)Result.Win];
        }
        else if (winner == GameInfo.OpponentTurn)
        {
            messageText = "LOSE";
            m_resultImage.sprite = m_sprites[(int)Result.Lose];
            m_resultBgImage.sprite = m_bgSprites[(int)Result.Lose];
        }
        m_resultDialogManager.Init(isContinue, messageText, buttonText);

        gameObject.SetActive(true);
    }

    public void Invisible()
    {
        gameObject.SetActive(false);
    }
}