using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardSelectManager : CardManager
{
    //--------------------------------------------
    // コンポーネント
    //--------------------------------------------

    private Toggle m_cardToggle;

    //--------------------------------------------
    // 処理
    //--------------------------------------------

    public Action<int> SelectAction { get; set; }

    //--------------------------------------------
    // 設定
    //--------------------------------------------

    [SerializeField]
    private int m_no;

    //--------------------------------------------
    // 初期化
    //--------------------------------------------

    void Awake()
    {
        m_cardToggle = GetComponent<Toggle>();
        m_cardToggle.onValueChanged.AddListener((isOn) => OnChangeToggle(isOn));
    }

    //--------------------------------------------
    // 表示
    //--------------------------------------------

    public override void Visible(int no = 0)
    {
        base.Visible(no);
    }

    public override void Invisible()
    {
        m_cardToggle.isOn = false;
        base.Invisible();
    }

    //--------------------------------------------
    // ボタン
    //--------------------------------------------

    /// <summary>
    /// カード選択時
    /// </summary>
    /// <param name="isOn"></param>
    /// <param name="no"></param>
    private void OnChangeToggle(bool isOn)
    {
        if (isOn)
        {
            AudioManager.I.PlaySe(AudioNames.ButtonSE);
            SelectAction.Invoke(m_no);
        }
    }
}
