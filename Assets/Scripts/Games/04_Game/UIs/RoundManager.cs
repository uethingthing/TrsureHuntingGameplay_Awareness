using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoundManager : MonoBehaviour
{
    //--------------------------------------------
    // コンポーネント
    //--------------------------------------------

    [SerializeField]
    private Text m_roundText;

    //--------------------------------------------
    // 初期化
    //--------------------------------------------

    // Start is called before the first frame update
    void Start()
    {
        m_roundText.text = (GameInfo.Game.WinOrLose.Count + 1).ToString();
    }
}
