using System.Collections;
using UnityEngine.UI;
using UnityEngine;

/// <summary>
/// 先行/後攻選択用UI
/// </summary>
public class SelectTurnUI : MonoBehaviour
{
    //--------------------------------------------
    // コンポーネント
    //--------------------------------------------

    [SerializeField, Header("先行ボタン")]
    private Button m_FirstAttackButton = null;

    [SerializeField, Header("後攻ボタン")]
    private Button m_SecondAttackButton = null;

    //--------------------------------------------
    // データ
    //--------------------------------------------

    /// <summary>
    /// 選択用ターン
    /// </summary>
    private Turn m_SelectTurn = Turn.None;

    //--------------------------------------------
    // 初期化
    //--------------------------------------------

    void Awake()
    {
        m_FirstAttackButton.onClick.AddListener(() => OnClick_FirstAttackButton());
        m_SecondAttackButton.onClick.AddListener(() => OnClick_SecondAttackButton());
    }

    //--------------------------------------------
    // 
    //--------------------------------------------

    /// <summary>
    /// ボタンの反応　有効/無効設定
    /// </summary>
    /// <param name="enabled"></param>
    private void SetIntaractivableButton(bool enabled)
    {
        m_FirstAttackButton.interactable = enabled;
        m_SecondAttackButton.interactable = enabled;
    }

    /// <summary>
    /// 先行 or 後攻を選択する
    /// </summary>
    /// <returns></returns>
    public IEnumerator CoSelect()
    {
        // ボタン登録
        SetIntaractivableButton(true);

        // 先行 or 後攻が選択されるまでループ
        yield return new WaitUntil(() => m_SelectTurn != Turn.None);

        // ターンを設定
        GameInfo.Game.Turn = m_SelectTurn;
    }

    //--------------------------------------------
    // ボタン
    //--------------------------------------------

    /// <summary>
    /// 先行ボタン押下時に呼び出されるメソッド
    /// </summary>
    private void OnClick_FirstAttackButton()
    {
        AudioManager.I.PlaySe(AudioNames.ButtonSE);
        SetIntaractivableButton(false);
        m_SelectTurn = GameInfo.MyTurn;
    }

    /// <summary>
    /// 後攻ボタン押下時に呼び出されるメソッド
    /// </summary>
    private void OnClick_SecondAttackButton()
    {
        AudioManager.I.PlaySe(AudioNames.ButtonSE);
        SetIntaractivableButton(false);

        switch (GameInfo.MyTurn)
        {
            case Turn.User01:
                m_SelectTurn = Turn.User02;
                break;
            case Turn.User02:
                m_SelectTurn = Turn.User01;
                break;
        }
    }
}
