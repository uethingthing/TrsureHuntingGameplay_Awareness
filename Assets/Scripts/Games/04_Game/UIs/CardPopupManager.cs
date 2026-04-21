using System.Collections;
using UnityEngine;
using UnityEngine.UI;

class CardPopupManager : MonoBehaviour
{
    [SerializeField]
    private GameObject m_rootObj;

    [SerializeField]
    private Image m_cardImage;

    [SerializeField]
    private CardSetObject m_cardSetObject;


    private void Awake()
    {
        if (m_rootObj != null)
        {
            m_rootObj.SetActive(false);
        }
    }

    public void Visible(int selectedNo)
    {
        if(m_cardImage != null)
        {
            // 自分の持つカードを表示
            UserData myData = GameInfo.MyData;
            int spriteIdx = (int)myData.Card[selectedNo];
            m_cardImage.sprite = m_cardSetObject.Cards[spriteIdx].bigSprite;
        }

        StartCoroutine(CoShow());
    }

    private IEnumerator CoShow()
    {
        m_rootObj.SetActive(true);
        yield return new WaitForSeconds(5f);

        m_rootObj.SetActive(false);

        if (m_cardImage != null && m_cardSetObject != null)
        {
            m_cardImage.sprite = m_cardSetObject.Cards[(int)CardType.None].bigSprite;
        }
    }
}
