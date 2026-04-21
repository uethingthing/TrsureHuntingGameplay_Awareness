using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// タイトル管理用クラス
/// </summary>
public class TitleManager : UWRHelper
{
    //--------------------------------------------
    // 設定
    //--------------------------------------------

    [SerializeField]
    private URLType m_urlType = URLType.Quadra;

    [SerializeField]
    private bool m_checkPlayerType = true;

    [SerializeField]
    private bool m_isSingleMode = false;

    //--------------------------------------------
    // コンポーネント
    //--------------------------------------------

    [SerializeField]
    private GameObject m_debugSetObject;

    private DebugSetID m_debugSetId;

    //--------------------------------------------
    // 初期化
    //--------------------------------------------

    private void Awake()
    {
        GameInfo.Init();
        GameInfo.URLType = m_urlType;
        GameInfo.CheckPlayerType = m_checkPlayerType;
#if UNITY_EDITOR
        GameInfo.IsSingleMode = m_isSingleMode;
#endif

        m_debugSetId = m_debugSetObject.GetComponent<DebugSetID>();
    }

    /// <summary>
    /// Start
    /// </summary>
    protected virtual IEnumerator Start()
    {
        m_debugSetObject.SetActive(false);

        // 指定秒数待機...
        yield return new WaitForSeconds(0.1f);

        Debug.Log("接続先 : " + GameInfo.URLType);
        Debug.Log("プレイヤータイプチェック（講師か生徒か見る） : " + (GameInfo.CheckPlayerType == true ? "する" : "しない"));

        // WebGLでの起動でない場合
        if (Application.platform != RuntimePlatform.WebGLPlayer)
        {
            m_debugSetObject.SetActive(true);
        }

        // タイトルBGM再生
        AudioManager.I.PlayBgm(AudioNames.MainBgm, 0.5f);

        // クリックされるまで待機...
        yield return new WaitUntil(() => OnClick());

        if (Application.platform != RuntimePlatform.WebGLPlayer)
        {
            m_debugSetId.SettingData();
        }

        // ボタンSE再生
        AudioManager.I.PlaySe(AudioNames.ButtonSE);

        // ルームシーンへ遷移
        SceneFadeManager.I.Load(SceneName.Room);
    }

    //--------------------------------------------
    // ボタン
    //--------------------------------------------

    /// <summary>
    /// クリックされているか？
    /// </summary>
    /// <returns>TRUE: クリックされた FALSE: クリックされていない</returns>
    private bool OnClick()
    {
        if (Input.GetMouseButtonDown(0))
        {
            List<RaycastResult> results = new List<RaycastResult>();

            // マウスポインタの位置にレイ飛ばし、ヒットしたものを保存
            // ポインタ（マウス/タッチ）イベントに関連するイベントの情報
            var pointer = new PointerEventData(EventSystem.current);
            pointer.position = Input.mousePosition;
            EventSystem.current.RaycastAll(pointer, results);

            // UIがヒットしていればfalseを返す
            foreach (RaycastResult target in results)
            {
                return false;
            }

            // UIがヒットしていなればtrueを返す
            return true;
        }
        return false;
    }
}
