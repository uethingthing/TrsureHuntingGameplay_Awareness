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

    private GameDataSync m_gameDataSync;


    private void Awake()
    {
        m_gameDataSync = FindObjectsOfType<GameDataSync>()[0];

        if (m_rootObj != null)
        {
            m_rootObj.SetActive(false);
        }
    }

    public void Visible(CardType cardType)
    {
        if (m_cardImage != null)
        {
            // 自分の持つカードを表示
            int spriteIdx = (int)cardType;
            m_cardImage.sprite = m_cardSetObject.Cards[spriteIdx].bigSprite;
        }

        StartCoroutine(CoShow(cardType));
    }

    private IEnumerator CoShow(CardType cardType)
    {
        // 相手にもカードを表示させるためデータを送信
        GameInfo.Game.UseCard = cardType;
        yield return m_gameDataSync.CoUpdateGameData(GameInfo.Game);

        m_rootObj.SetActive(true);
        yield return new WaitForSeconds(5f);

        m_rootObj.SetActive(false);

        if (m_cardImage != null && m_cardSetObject != null)
        {
            m_cardImage.sprite = m_cardSetObject.Cards[(int)CardType.None].bigSprite;
        }
    }
}
