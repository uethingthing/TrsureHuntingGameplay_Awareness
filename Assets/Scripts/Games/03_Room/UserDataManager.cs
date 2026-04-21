using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using static RoomCanvasManager;

public class UserDataManager : MonoBehaviour
{
    //--------------------------------------------
    // コンポーネント
    //--------------------------------------------

    [SerializeField, Header("キャンバス管理")]
    private RoomCanvasManager m_roomCanvasManager;

    //--------------------------------------------
    // 処理
    //--------------------------------------------

    public UnityAction ErrorAction { get; set; }

    //--------------------------------------------
    // ユーザーデータ取得
    //--------------------------------------------

    /// <summary>
    /// GameKey取得
    /// </summary>
    /// <returns></returns>
    public IEnumerator CoGetGameKey()
    {
        // ゲームが実行されている環境がWebGLの場合なら...
        // GameKey取得
        if (Application.platform == RuntimePlatform.WebGLPlayer)
        {
            if (GetClieParameters.m_GameKey != null)
            {
                KeyData.GameKey = GetClieParameters.m_GameKey;
            }
            else
            {
                // RoomID取得エラーメッセージ
                UtilityManager.I.ErrorMessageBox("Communication error", $"Failed to get progresses ID\nReturn to the title.", ErrorAction);
                while (true) { yield return null; }
            }
        }

        yield break;
    }

    /// <summary>
    /// ユーザーID取得
    /// </summary>
    /// <returns></returns>
    public IEnumerator CoGetUserID()
    {
        // ゲームが実行されている環境がWebGLの場合なら...
        // UserID取得
        if (Application.platform == RuntimePlatform.WebGLPlayer)
        {
            if (GetClieParameters.m_UserId != null)
            {
                GameInfo.MyUserID = GetClieParameters.m_UserId;
            }
            else
            {
                // ユーザーデータ取得失敗UI表示
                m_roomCanvasManager.ChangeView(Room.FailedUserData);

                // UserID取得エラーメッセージ
                UtilityManager.I.ErrorMessageBox("Communication error", $"Failed to get user ID\nReturn to the title.", ErrorAction);
                while (true) { yield return null; }
            }
        }

        yield break;
    }

    /// <summary>
    /// プレイヤータイプ（講師か生徒）取得
    /// </summary>
    /// <returns></returns>
    public IEnumerator CoGetPlayerType()
    {
        // ゲームが実行されている環境がWebGLの場合なら...
        // 講師か生徒か取得
        if (Application.platform == RuntimePlatform.WebGLPlayer)
        {
            int tmp = -1;
            string playerTypeStr = GetClieParameters.m_PlayerType;

            // URLに入力されているPlayerType設定文字列が正常にint型に変換できるもので、
            // 設定可能な値の範囲内であれば、PlayerTypeを設定
            // 念のためここでもチェックしてる（多分いらない）
            if (!string.IsNullOrEmpty(playerTypeStr) &&
                int.TryParse(playerTypeStr, out tmp) &&
                int.Parse(playerTypeStr) >= 1 &&
                int.Parse(playerTypeStr) <= 2)
            {
                // PlayerType取得完了
                GameInfo.MyPlayerType = (PlayerType)Enum.Parse(typeof(PlayerType), playerTypeStr);

                Debug.Log("プレイヤータイプ取得 : OK");
            }
            else
            {
                // PlayerType取得失敗UI表示
                m_roomCanvasManager.ChangeView(Room.FailedUserData);

                Debug.Log("プレイヤータイプ取得 : エラー");

                // PlayerType取得エラーメッセージ
                UtilityManager.I.ErrorMessageBox("Communication error", $"Failed to get Player Type\nReturn to the title.", ErrorAction);
                while (true) { yield return null; }
            }
        }

        yield break;
    }
}
