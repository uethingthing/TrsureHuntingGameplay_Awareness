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
    private CardButtonManager m_cardButton;

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
        m_cardButton.Button.onClick.AddListener(() => OnSkillCardButton());
        m_cardMenu.OnUseEvent = (selectNo) => {
            // 自分の持つカードを表示
            var card = GameInfo.MyData.Card[selectNo];
            m_cardPopup.Visible(card);
        };
    }

    /// <summary>
    /// カード表示
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
        m_cardButton.SetImage(myData.Card.Count);
        //// 画像更新と表示
        //for (int i = 0; i < myData.Card.Count && i < m_maxCard; i++)
        //{
        //    m_cardManagers[i].Visible((int)myData.Card[i]);
        //}

        //m_cardCount.text = myData.Card.Count.ToString();
    }

    /// <summary>
    /// 選択カード表示
    /// </summary>
    /// <param name="cardType"></param>
    public void VisibleCardPopup(CardType cardType)
    {
        m_cardPopup.Visible(cardType);
    }

    /// <summary>
    /// カードボタンの表示更新を止め、強制的に非表示
    /// </summary>
    public void ForceInvisibleCardButton()
    {
        m_cardButton?.ForceInvisible();
    }

    /// <summary>
    /// カードボタンの表示更新を有効化
    /// </summary>
    public void EnableVisibleUpdate()
    {
        m_cardButton?.EnableVisibleUpdate();
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
