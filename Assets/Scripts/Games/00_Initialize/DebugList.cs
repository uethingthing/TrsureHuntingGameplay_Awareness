using System.Collections;
using UnityEngine.UI;
using UnityEngine;

/// <summary>
/// DebugListクラス
/// </summary>
public class DebugList : UWRHelper
{
    [SerializeField, Header("FPS/スクリーンサイズ表示用UI")]
    private GameObject m_InfoView = default;

    [SerializeField, Header("デバッグコンソール")]
    private GameObject m_IngameDebugConsole = null;

    [SerializeField, Header("ゲームデータ表示用")]
    private GameObject m_GameDataView = null;

    [SerializeField, Header("デバッグボタン（m_DebugListButton表示用）")]
    private Button m_DebugButton = default;

    [SerializeField, Header("デバッグ用ボタンが列挙されているUI")]
    private GameObject m_DebugListButton = default;

    [SerializeField, Header("FPS/スクリーンサイズ表示用ボタン")]
    private Button m_FpsAndScreenSizeButton = default;

    [SerializeField, Header("ゲームデータ削除用ボタン")]
    private Button m_DeleteGameDataButton = default;

    [SerializeField, Header("コンソール表示用ボタン")]
    private Button m_DisplayConsoleButton = null;

    [SerializeField, Header("ゲームデータ表示用ボタン")]
    private Button m_DisplayGameDataButton = null;

    [SerializeField, Header("URLドロップダウンリスト")]
    private Dropdown m_URLDropDown = null;

    /// <summary>
    /// Start
    /// </summary>
    private void Start()
    {
#if UNITY_EDITOR
        gameObject.SetActive(true);
        m_DebugListButton.SetActive(false);
#else
        gameObject.SetActive(false);
#endif

        // 各ボタン登録
        m_DebugButton.onClick.AddListener(() => OnClick_DebugButton());
        m_FpsAndScreenSizeButton.onClick.AddListener(() => OnClick_FpsAndScreenSizeButton());
        m_DeleteGameDataButton.onClick.AddListener(() => StartCoroutine(CoAllDeleteData()));
        m_DisplayConsoleButton.onClick.AddListener(() => OnClick_DisplayConsoleButton());
        m_DisplayGameDataButton.onClick.AddListener(() => OnClick_DisplayGameDataButton());
        m_URLDropDown.onValueChanged.AddListener((int value) => OnValueChanged_URLDropDown(value));
        m_URLDropDown.value = (int)GameInfo.URLType;
    }

    /// <summary>
    /// デバッグボタン押下時、デバッグボタンリスト表示
    /// </summary>
    private void OnClick_DebugButton()
    {
        m_DebugListButton.SetActive(!m_DebugListButton.activeSelf);
    }

    /// <summary>
    /// 全てのデータを削除
    /// </summary>
    /// <returns></returns>
    private IEnumerator CoAllDeleteData()
    {
        SetIntaractableButton(false);

        var msgBox = (GameObject)Instantiate((GameObject)Resources.Load("Prefabs/MessageBox"));
        msgBox.GetComponent<MessageBox>().Initialize_MessageOnly("Delete", "Delete GameData");

        // ゲームデータ削除
        var uwr = CreateDeletUrl(KeyData.GameKey);
        yield return WaitForRequest(uwr);

        //Debug.Log("全てのデータ削除");

        Destroy(msgBox);
        SetIntaractableButton(true);
    }

    /// <summary>
    /// FPS/スクリーンサイズ 表示/非表示
    /// </summary>
    private void OnClick_FpsAndScreenSizeButton()
    {
        SetIntaractableButton(false);

        m_InfoView.SetActive(!m_InfoView.gameObject.activeSelf);

        if(m_InfoView.gameObject.activeSelf)
        {
            m_FpsAndScreenSizeButton.GetComponent<Image>().color = Color.red;
        }
        else
        {
            m_FpsAndScreenSizeButton.GetComponent<Image>().color = Color.white;
        }

        SetIntaractableButton(true);
    }


    /// <summary>
    /// デバッグコンソール　表示/非表示
    /// </summary>
    private void OnClick_DisplayConsoleButton()
    {
        SetIntaractableButton(false);

        m_IngameDebugConsole.SetActive(!m_IngameDebugConsole.activeSelf);

        if (m_IngameDebugConsole.activeSelf)
        {
            m_DisplayConsoleButton.GetComponent<Image>().color = Color.red;
        }
        else
        {
            m_DisplayConsoleButton.GetComponent<Image>().color = Color.white;
        }

        SetIntaractableButton(true);
    }

    /// <summary>
    /// ゲームデータ　表示/非表示
    /// </summary>
    private void OnClick_DisplayGameDataButton()
    {
        SetIntaractableButton(false);

        m_GameDataView.SetActive(!m_GameDataView.activeSelf);

        if (m_GameDataView.activeSelf)
        {
            m_DisplayGameDataButton.GetComponent<Image>().color = Color.red;
        }
        else
        {
            m_DisplayGameDataButton.GetComponent<Image>().color = Color.white;
        }

        SetIntaractableButton(true);
    }

    /// <summary>
    /// 環境変更（URL変更）
    /// </summary>
    /// <param name="value"></param>
    private void OnValueChanged_URLDropDown(int value)
    {
        var type = (URLType)value;
        GameInfo.URLType = type;
        Debug.Log("環境" + GameInfo.URLType);
    }

    /// <summary>
    /// ボタン反応　有り/無し
    /// </summary>
    /// <param name="enabled"></param>
    private void SetIntaractableButton(bool enabled)
    {
        m_DeleteGameDataButton.interactable   = enabled;
        m_FpsAndScreenSizeButton.interactable = enabled;
        m_DisplayConsoleButton.interactable   = enabled;
        m_DisplayGameDataButton.interactable  = enabled;
    }
}
