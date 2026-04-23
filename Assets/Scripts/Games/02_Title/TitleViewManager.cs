using System.Collections;
using UnityEngine;

/// <summary>
/// タイトル画面表示クラス
/// </summary>
class TitleViewManager : MonoBehaviour
{
    //--------------------------------------------
    // 設定
    //--------------------------------------------

    [SerializeField, Header("スタート表示時間")]
    private float m_startVisibleTime = 4f;

    //--------------------------------------------
    // コンポーネント
    //--------------------------------------------

    [SerializeField]
    private GameObject m_stageObj;

    [SerializeField]
    private GameObject m_startButtonObj;

    [SerializeField]
    private GameObject m_debugObj;

    [SerializeField]
    private GameObject m_startObj;

    private void Awake()
    {
        m_startObj.SetActive(false);
    }

    public IEnumerator VisibleStart()
    {
        // Start表示以外を非表示にする
        m_stageObj.SetActive(false);
        m_startButtonObj.SetActive(false);
        m_debugObj.SetActive(false);
        m_startObj.SetActive(true);

        yield return new WaitForSeconds(m_startVisibleTime);
    }
}
