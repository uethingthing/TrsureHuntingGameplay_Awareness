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
    private ToggleGroup m_toggleGroup;

    [SerializeField]
    private RectTransform m_fingerIcon;

    [SerializeField]
    private CardSelectManager[] m_cardSelectManagers;

    [SerializeField]
    private Button m_useButton;

    [SerializeField]
    private Button m_notUseButton;

    public System.Action<int> OnUseEvent;

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
        Action<int> action = (no) => m_selectNo = no;
        for (int i = 0; i < m_cardSelectManagers.Length; i++)
        {
            m_cardSelectManagers[i].SelectAction = action;
        }

        m_useButton.onClick.AddListener(() => OnUseButton());
        m_notUseButton.onClick.AddListener(() => OnNotUseButton());

        gameObject.SetActive(true);

        m_maxCard = m_cardSelectManagers.Length;
    }

    private void Start()
    {
        var toggles = m_toggleGroup.GetComponentsInChildren<Toggle>();
        foreach(var toggle in toggles)
        {
            toggle.onValueChanged.AddListener((isOn) =>
            {
                if(isOn)
                {
                    OnCardSelected(toggle);
                }
            });
        }

        m_fingerIcon.gameObject.SetActive(false);
    }

    private void OnDisable()
    {
        m_fingerIcon.gameObject.SetActive(false);
    }

    private void OnCardSelected(Toggle selectedToggle)
    {
        // 初めて選択されたから表示する
        if(!m_fingerIcon.gameObject.activeSelf)
        {
            m_fingerIcon.gameObject.SetActive(true);
        }

        // 選択されたToggleのRectTransformを取得
        RectTransform cardRect = selectedToggle.GetComponent<RectTransform>();

        // 指アイコンをカードの位置へ移動
        m_fingerIcon.position = cardRect.position;

        // 指アイコンがカードの上に来るようにy位置を調整
        Vector3 newPos = m_fingerIcon.localPosition;
        newPos.y += 170f;
        m_fingerIcon.localPosition = newPos;
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

            gameObject.SetActive(false);

            // カード使用時のイベント通知
            OnUseEvent?.Invoke(m_selectNo);
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
