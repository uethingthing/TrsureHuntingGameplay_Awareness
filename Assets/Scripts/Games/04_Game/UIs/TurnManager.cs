using UnityEngine;
using UnityEngine.UI;

public class TurnManager : MonoBehaviour
{
    //--------------------------------------------
    // コンポーネント
    //--------------------------------------------

    [SerializeField]
    private Image m_turnImage;

    [SerializeField]
    private Sprite[] m_sprites;

    private enum TurnSpriteIdx
    {
        MyTurn = 0,
        OpponentTurn,
    }

    //--------------------------------------------
    // データ
    //--------------------------------------------

    private Turn m_oldTurn;

    //--------------------------------------------
    // 更新
    //--------------------------------------------

    void Awake()
    {
        m_oldTurn = Turn.None;
    }

    // Update is called once per frame
    void Update()
    {
        Turn turn = GameInfo.Game.Turn;
        if (m_oldTurn != turn)
        {
            if(turn == GameInfo.MyTurn)
            {
                m_turnImage.sprite = m_sprites[(int)TurnSpriteIdx.MyTurn];
            }
            else if(turn == GameInfo.OpponentTurn)
            {
                m_turnImage.sprite = m_sprites[(int)TurnSpriteIdx.OpponentTurn];
            }
            m_oldTurn = turn;
        }
    }
}
