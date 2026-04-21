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

    [SerializeField]
    protected Image m_cardImage;

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
        if (m_cardText != null)
        {
            m_cardText.text = m_cardSet.Cards[no].text;
        }

        if (m_cardImage != null)
        {
            m_cardImage.sprite = m_cardSet.Cards[no].sprite;
        }

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
