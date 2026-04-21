using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardGroupManager : MonoBehaviour
{
    //--------------------------------------------
    // コンポーネント
    //--------------------------------------------

    [SerializeField]
    private CardManager[] m_cardManagers;

    [SerializeField]
    private Button m_skillCardButton;

    [SerializeField]
    private CardMenuManager m_cardMenu;

    [SerializeField]
    private CardPopupManager m_cardPopup;

    [SerializeField]
    private Text m_cardCount;

    //--------------------------------------------
    // データ
    //--------------------------------------------

    private int m_maxCard;

    //--------------------------------------------
    // 初期化
    //--------------------------------------------

    void Awake()
    {
        m_maxCard = m_cardManagers.Length;

    }

    private void Start()
    {
        m_skillCardButton.onClick.AddListener(() => OnSkillCardButton());
        m_cardMenu.OnUseEvent = (selectNo) => { m_cardPopup.Visible(selectNo); };
    }

    /// <summary>
    /// カード表示s
    /// </summary>
    public void Visible()
    {
        // 全カード非表示
        for (int i = 0; i < m_cardManagers.Length; i++)
        {
            m_cardManagers[i].Invisible();
        }

        // 自分の持つカードを表示
        UserData myData = GameInfo.MyData;
        // 画像更新と表示
        for (int i = 0; i < myData.Card.Count && i < m_maxCard; i++)
        {
            m_cardManagers[i].Visible((int)myData.Card[i]);
        }

        m_cardCount.text = myData.Card.Count.ToString();
    }

    //--------------------------------------------
    // ボタン
    //--------------------------------------------

    /// <summary>
    /// スキルカードボタン押下時
    /// </summary>
    private void OnSkillCardButton()
    {
        // 自分のターンかつ、まだ単語が選択されていないかつ、カードが選択されていないとき選択画面表示
        if(GameManager.I.SyncCompletedFlg && !GameManager.I.IsVisiblePopup
            && GameInfo.Game.SelectCardNo == -1)
        {
            AudioManager.I.PlaySe(AudioNames.ButtonSE);
            m_cardMenu.Visible();
        }
    }
}
