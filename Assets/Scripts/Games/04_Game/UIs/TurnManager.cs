using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TurnManager : MonoBehaviour
{
    //--------------------------------------------
    // コンポーネント
    //--------------------------------------------

    [SerializeField]
    private Text m_turnText;

    //--------------------------------------------
    // データ
    //--------------------------------------------

    private Turn oldTurn;

    //--------------------------------------------
    // 更新
    //--------------------------------------------

    void Awake()
    {
        m_turnText.text = "";
        oldTurn = Turn.None;
    }

    // Update is called once per frame
    void Update()
    {
        Turn turn = GameInfo.Game.Turn;
        if (oldTurn != turn)
        {
            string text = "";
            if(turn == GameInfo.MyTurn)
            {
                text = "Your Turn";
            }
            else if(turn == GameInfo.OpponentTurn)
            {
                text = "Opponent Turn";
            }
            m_turnText.text = text;
            oldTurn = turn;
        }
    }
}
