using UnityEngine;
using UnityEngine.UI;

class CardButton : MonoBehaviour
{
    [SerializeField]
    private Button m_button;

    [SerializeField]
    private Image m_image;

    [SerializeField]
    private Sprite[] m_sprites;

    public Button Button => m_button;


    /// <summary>
    /// ボタン画像設定
    /// </summary>
    /// <param name="cardCount"></param>
    public void SetImage(int cardCount)
    {
        int spriteIdx = (Mathf.Clamp(cardCount, 0, 3) - 1);
        if(spriteIdx < 0)
        {
            this.gameObject.SetActive(false);
        }
        else
        {
            m_image.sprite = m_sprites[spriteIdx];
            if(!this.gameObject.activeSelf)
            {
                this.gameObject.SetActive(true);
            }
        }
    }
}
