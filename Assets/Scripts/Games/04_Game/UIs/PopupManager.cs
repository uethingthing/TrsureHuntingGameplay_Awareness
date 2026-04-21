using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopupManager : MonoBehaviour
{
    //--------------------------------------------
    // コンポーネント
    //--------------------------------------------

    [SerializeField]
    private Text m_wordText;

    [SerializeField]
    private Text m_JPwordText;

    //--------------------------------------------
    // データ
    //--------------------------------------------

    [SerializeField]
    private WordSetObject[] m_wordSetObject;

    [SerializeField]
    private WordSetObject[] m_JPwordSetObject;

    //--------------------------------------------
    // 表示
    //--------------------------------------------

    /// <summary>
    /// 単語の表示
    /// </summary>
    /// <param name="no"></param>
    public void Visible(int no = 0)
    {
        m_wordText.text = m_wordSetObject[GameInfo.Game.SelectLevel].Words[no];
        m_JPwordText.text = m_JPwordSetObject[GameInfo.Game.SelectLevel].Words[no];
        gameObject.SetActive(true);
    }

    /// <summary>
    /// 単語非表示
    /// </summary>
    public void Invisible()
    {
        gameObject.SetActive(false);
    }
}
