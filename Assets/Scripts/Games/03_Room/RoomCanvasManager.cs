using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoomCanvasManager : MonoBehaviour
{
    //--------------------------------------------
    // enum
    //--------------------------------------------

    public enum Room
    {
        None,
        GetUserData,
        FailedUserData,
        WaitingConnect,
        WaitingSelectTurn,
        SelectTurn,
        SelectLevel
    }

    //--------------------------------------------
    // コンポーネント
    //--------------------------------------------

    [SerializeField, Header("ユーザーデータ取得UI")]
    private GameObject m_getUserDataUI;

    [SerializeField, Header("ユーザーデータ取得失敗UI")]
    private GameObject m_failedUserDataUI;

    [SerializeField, Header("接続待ちUI")]
    private GameObject m_waitingConnectUI;

    [SerializeField, Header("ターン選択待ちUI")]
    private GameObject m_waitingSelectTurnUI;

    [SerializeField, Header("先行/後攻を設定するUI")]
    private GameObject m_selectTurnUI;

    [SerializeField, Header("レベルを設定するUI")]
    private GameObject m_selectLevelUI;

    [SerializeField, Header("ユーザーの名前を表示するテキスト")]
    private Text m_userNameText;

    [SerializeField, Header("プレイヤータイプを表示するテキスト")]
    private Text m_playerTypeText;

    //--------------------------------------------
    // 画面更新
    //--------------------------------------------

    /// <summary>
    /// 表示画面変更
    /// </summary>
    /// <param name="room"></param>
    public void ChangeView(Room room)
    {
        switch (room)
        {
            case Room.None:
                m_getUserDataUI.SetActive(false);
                m_failedUserDataUI.SetActive(false);
                m_waitingConnectUI.SetActive(false);
                m_waitingSelectTurnUI.SetActive(false);
                m_selectTurnUI.SetActive(false);
                m_selectLevelUI.SetActive(false);
                break;

            case Room.GetUserData:
                m_getUserDataUI.SetActive(true);
                m_failedUserDataUI.SetActive(false);
                m_waitingConnectUI.SetActive(false);
                m_waitingSelectTurnUI.SetActive(false);
                m_selectTurnUI.SetActive(false);
                m_selectLevelUI.SetActive(false);
                break;

            case Room.FailedUserData:
                m_getUserDataUI.SetActive(false);
                m_failedUserDataUI.SetActive(true);
                m_waitingConnectUI.SetActive(false);
                m_waitingSelectTurnUI.SetActive(false);
                m_selectTurnUI.SetActive(false);
                m_selectLevelUI.SetActive(false);
                break;

            case Room.WaitingConnect:
                m_getUserDataUI.SetActive(false);
                m_failedUserDataUI.SetActive(false);
                m_waitingConnectUI.SetActive(true);
                m_waitingSelectTurnUI.SetActive(false);
                m_selectTurnUI.SetActive(false);
                m_selectLevelUI.SetActive(false);
                break;

            case Room.WaitingSelectTurn:
                m_getUserDataUI.SetActive(false);
                m_failedUserDataUI.SetActive(false);
                m_waitingConnectUI.SetActive(false);
                m_waitingSelectTurnUI.SetActive(true);
                m_selectTurnUI.SetActive(false);
                m_selectLevelUI.SetActive(false);
                break;

            case Room.SelectTurn:
                m_getUserDataUI.SetActive(false);
                m_failedUserDataUI.SetActive(false);
                m_waitingConnectUI.SetActive(false);
                m_waitingSelectTurnUI.SetActive(false);
                m_selectTurnUI.SetActive(true);
                m_selectLevelUI.SetActive(false);
                break;

            case Room.SelectLevel:
                m_getUserDataUI.SetActive(false);
                m_failedUserDataUI.SetActive(false);
                m_waitingConnectUI.SetActive(false);
                m_waitingSelectTurnUI.SetActive(false);
                m_selectTurnUI.SetActive(false);
                m_selectLevelUI.SetActive(true);
                break;
        }
    }

    //--------------------------------------------
    // テキスト更新
    //--------------------------------------------

    /// <summary>
    /// ユーザー名変更
    /// </summary>
    /// <param name="name"></param>
    public void ChangeUserNameText(string name)
    {
        m_userNameText.text = name;
    }

    /// <summary>
    /// プレイヤータイプ変更
    /// </summary>
    /// <param name="type"></param>
    public void ChangePlayerTypeText(string type)
    {
        m_playerTypeText.text = type;
    }
}
