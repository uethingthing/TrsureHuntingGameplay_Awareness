using System.Collections;
using UnityEngine;

/// <summary>
/// タイトル画面表示クラス
/// </summary>
class StartManager : MonoBehaviour
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
    private GameObject m_startObj;

    private void Awake()
    {
        m_startObj.SetActive(false);
    }

    public IEnumerator VisibleStart()
    {
        // Start表示
        m_startObj.SetActive(true);

        // 指定時間経過後に非表示s
        yield return new WaitForSeconds(m_startVisibleTime);
        m_startObj.SetActive(false);
    }
}
