using UnityEngine;
using UnityEngine.UI;

public class TurnManager : MonoBehaviour
{
    //--------------------------------------------
    // コンポーネント
    //--------------------------------------------

    [SerializeField]
    GameObject m_yourTurnObj;

    [SerializeField]
    GameObject m_opponentTurnObj;

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
                m_yourTurnObj.SetActive(true);
                m_opponentTurnObj.SetActive(false);
            }
            else if(turn == GameInfo.OpponentTurn)
            {
                m_opponentTurnObj.SetActive(true);
                m_yourTurnObj.SetActive(false);
            }
            m_oldTurn = turn;
        }
    }
}
