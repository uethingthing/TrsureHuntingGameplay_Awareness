using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardManager : MonoBehaviour
{
    //--------------------------------------------
    // コンポーネント
    //--------------------------------------------

    [SerializeField]
    protected Text m_cardText;

    //--------------------------------------------
    // データ
    //--------------------------------------------

    [SerializeField]
    protected CardSetObject m_cardSet;

    /// <summary>
    /// カードの表示
    /// </summary>
    /// <param name="no"></param>
    public virtual void Visible(int no = 0)
    {
        m_cardText.text = m_cardSet.Cards[no].text;
        gameObject.SetActive(true);
    }

    /// <summary>
    /// 単語非表示
    /// </summary>
    public virtual void Invisible()
    {
        gameObject.SetActive(false);
    }
}
