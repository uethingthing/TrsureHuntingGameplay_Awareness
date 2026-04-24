using UnityEngine;
using UnityEngine.UI;

class CardButtonManager : MonoBehaviour
{
    [SerializeField]
    private Button m_button;

    [SerializeField]
    private Image m_image;

    [SerializeField]
    private Sprite[] m_sprites;

    public Button Button => m_button;

    private Turn m_oldTurn;

    private bool m_DisableUpdate = false;

    private void Awake()
    {
        m_oldTurn = Turn.None;
    }

    private void Update()
    {
        if (m_DisableUpdate)
            return;

        if(HasCards())
        {
            Turn turn = GameInfo.Game.Turn;
            if (m_oldTurn != turn)
            {
                if (turn == GameInfo.MyTurn)
                {
                    Visible();
                }
                else
                {
                    Invisible();
                }
                m_oldTurn = turn;
            }
        }
        else
        {
            Invisible();
        }
    }

    public void EnableVisibleUpdate()
    {
        m_DisableUpdate = false;
    }

    public void ForceInvisible()
    {
        m_DisableUpdate = true;
        Invisible();
    }

    /// <summary>
    /// ボタン画像設定
    /// </summary>
    /// <param name="cardCount"></param>
    public void SetImage(int cardCount)
    {
        int spriteIdx = (Mathf.Clamp(cardCount, 0, 3) - 1);
        if(spriteIdx < 0)
        {
            Invisible();
        }
        else
        {
            m_image.sprite = m_sprites[spriteIdx];
        }
    }

    private bool HasCards()
    {
        return GameInfo.MyData.Card.Count > 0;
    }

    private void Visible()
    {
        if (!m_image.gameObject.activeSelf)
        {
            m_image.gameObject.SetActive(true);
        }
    }

    private void Invisible()
    {
        if (m_image.gameObject.activeSelf)
        {
            m_image.gameObject.SetActive(false);
        }
    }
}
