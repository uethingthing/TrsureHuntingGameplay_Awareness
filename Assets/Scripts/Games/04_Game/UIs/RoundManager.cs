using UnityEngine;
using UnityEngine.UI;

public class RoundManager : MonoBehaviour
{
    //--------------------------------------------
    // コンポーネント
    //--------------------------------------------

    [SerializeField]
    private Image m_roundNumImage;

    [SerializeField]
    private Sprite[] m_sprites;

    //--------------------------------------------
    // 初期化
    //--------------------------------------------

    // Start is called before the first frame update
    void Start()
    {
        int spriteIdx = Mathf.Clamp(GameInfo.Game.WinOrLose.Count, 0, 2);
        m_roundNumImage.sprite = m_sprites[spriteIdx];
    }
}
