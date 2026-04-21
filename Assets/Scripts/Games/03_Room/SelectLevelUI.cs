using System.Collections;
using UnityEngine.UI;
using UnityEngine;

/// <summary>
/// レベル選択用UI
/// </summary>
public class SelectLevelUI : MonoBehaviour
{
    //--------------------------------------------
    // コンポーネント
    //--------------------------------------------

    [SerializeField, Header("レベル1ボタン")]
    private Button m_level1Button;

    [SerializeField, Header("レベル2ボタン")]
    private Button m_level2Button;

    [SerializeField, Header("レベル3ボタン")]
    private Button m_level3Button;

    //--------------------------------------------
    // データ
    //--------------------------------------------

    /// <summary>
    /// 選択用レベル
    /// </summary>
    private int m_Selectlevel;

    //--------------------------------------------
    // 初期化
    //--------------------------------------------

    void Awake()
    {
        m_level1Button.onClick.AddListener(() => OnClickLevelButton(0));
        m_level2Button.onClick.AddListener(() => OnClickLevelButton(1));
        m_level3Button.onClick.AddListener(() => OnClickLevelButton(2));
    }

    void Start()
    {
        m_Selectlevel = -1;
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
        m_level1Button.interactable = enabled;
        m_level2Button.interactable = enabled;
        m_level3Button.interactable = enabled;
    }

    /// <summary>
    /// レベルを選択する
    /// </summary>
    /// <returns></returns>
    public IEnumerator CoSelect()
    {
        // ボタン登録
        SetIntaractivableButton(true);

        // 先行 or 後攻が選択されるまでループ
        yield return new WaitUntil(() => m_Selectlevel != -1);

        // レベルを設定
        GameInfo.Game.SelectLevel = m_Selectlevel;
    }

    //--------------------------------------------
    // ボタン
    //--------------------------------------------

    /// <summary>
    /// レベルボタン押下時に呼び出されるメソッド
    /// </summary>
    private void OnClickLevelButton(int no)
    {
        AudioManager.I.PlaySe(AudioNames.ButtonSE);
        SetIntaractivableButton(false);
        m_Selectlevel = no;
    }
}
