using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardMenuManager : MonoBehaviour
{
    //--------------------------------------------
    // コンポーネント
    //--------------------------------------------

    [SerializeField]
    private CardSelectManager[] m_cardSelectManagers;

    [SerializeField]
    private Button m_useButton;

    [SerializeField]
    private Button m_notUseButton;

    [SerializeField]
    private GameObject m_useSkillObject;

    //--------------------------------------------
    // データ
    //--------------------------------------------

    private int m_maxCard;

    /// <summary>
    /// 選択カード番号
    /// </summary>
    private int m_selectNo;

    //--------------------------------------------
    // 初期化
    //--------------------------------------------

    void Awake()
    {
        m_maxCard = m_cardSelectManagers.Length;

        Action<int> action = (no) => m_selectNo = no;
        for (int i = 0; i < m_cardSelectManagers.Length; i++)
        {
            m_cardSelectManagers[i].SelectAction = action;
        }

        m_useButton.onClick.AddListener(() => OnUseButton());
        m_notUseButton.onClick.AddListener(() => OnNotUseButton());

        gameObject.SetActive(true);
    }

    /// <summary>
    /// 画面の表示
    /// </summary>
    public void Visible()
    {
        m_selectNo = -1;
        // 全カード非表示
        for (int i = 0; i < m_cardSelectManagers.Length; i++)
        {
            m_cardSelectManagers[i].Invisible();
        }

        // 自分の持つカードを表示
        UserData myData = GameInfo.MyData;
        // 画像更新と表示
        for (int i = 0; i < myData.Card.Count && i < m_maxCard; i++)
        {
            m_cardSelectManagers[i].Visible((int)myData.Card[i]);
        }

        gameObject.SetActive(true);
    }

    //--------------------------------------------
    // ボタン
    //--------------------------------------------

    /// <summary>
    /// カードを使う押下時
    /// </summary>
    private void OnUseButton()
    {
        // どれか選択されていれば押下できる
        if(m_selectNo != -1)
        {
            AudioManager.I.PlaySe(AudioNames.ButtonSE);

            // カード使用状態にする
            GameInfo.Game.IsUseCard = true;
            GameInfo.Game.SelectCardNo = m_selectNo;
            m_useSkillObject.SetActive(true);

            gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// 使わない押下時
    /// </summary>
    private void OnNotUseButton()
    {
        AudioManager.I.PlaySe(AudioNames.ButtonSE);
        gameObject.SetActive(false);
    }
}
