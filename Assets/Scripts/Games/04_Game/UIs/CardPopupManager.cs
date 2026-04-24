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

    [SerializeField, Header("表示時間")]
    private float m_showWaitTime = 5f;

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
        m_rootObj.SetActive(true);

        Coroutine waitPopupRoutine = StartCoroutine(CoWaitPopupTime());
        Coroutine updateGameDataRoutine = StartCoroutine(CoUpdateGameData(cardType));

        yield return waitPopupRoutine;
        yield return updateGameDataRoutine;

        m_rootObj.SetActive(false);

        if (m_cardImage != null && m_cardSetObject != null)
        {
            m_cardImage.sprite = m_cardSetObject.Cards[(int)CardType.None].bigSprite;
        }
    }

    private IEnumerator CoWaitPopupTime()
    {
        yield return new WaitForSeconds(m_showWaitTime);
    }

    private IEnumerator CoUpdateGameData(CardType cardType)
    {
        // 相手にもカードを表示させるためデータを送信
        GameInfo.Game.UseCard = cardType;
        yield return m_gameDataSync.CoUpdateGameData(GameInfo.Game);
    }
}
