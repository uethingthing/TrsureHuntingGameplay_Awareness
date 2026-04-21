using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// ID設定用(Debug)
/// </summary>
public class DebugSetID : MonoBehaviour
{
    //--------------------------------------------
    // コンポーネント
    //--------------------------------------------

    /// <summary>
    /// RoomID入力用
    /// </summary>
    [SerializeField]
    private InputField m_RoomIDField = default;

    /// <summary>
    /// UserID入力用
    /// </summary>
    [SerializeField]
    private InputField m_UserIDField = default;

    /// <summary>
    /// PlayerType選択用
    /// </summary>
    [SerializeField]
    private Dropdown m_PlayerTypeDropdown;

    //--------------------------------------------
    // 初期化
    //--------------------------------------------

    /// <summary>
    /// Start
    /// </summary>
    private void Start()
    {
        m_RoomIDField.text = KeyData.GameKey;
        m_UserIDField.text = GameInfo.MyUserID;
        m_PlayerTypeDropdown.onValueChanged.AddListener((int value) => OnClick_PlayerType(value));
        m_PlayerTypeDropdown.value = (int)GameInfo.MyPlayerType;
    }

    //--------------------------------------------
    // 設定
    //--------------------------------------------

    /// <summary>
    /// データの設定
    /// </summary>
    public void SettingData()
    {
        KeyData.GameKey   = m_RoomIDField.text;
        GameInfo.MyUserID = m_UserIDField.text;
    }

    //--------------------------------------------
    // ドロップダウン
    //--------------------------------------------

    private void OnClick_PlayerType(int value)
    {
        GameInfo.MyPlayerType = (PlayerType)value;

        Debug.Log("PlayerType : " + (PlayerType)value);
    }
}