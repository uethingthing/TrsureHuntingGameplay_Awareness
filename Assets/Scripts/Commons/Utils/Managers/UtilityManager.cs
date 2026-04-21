using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// ゲーム状態
/// </summary>
public enum GameState
{
    Init,
    Opening,
    Game,
    Result,
}

public class UtilityManager : SingletonMonoBehaviour<UtilityManager>
{
    public void ErrorMessageBox(string title, string message, UnityAction action)
    {
        var msgBox = (GameObject)Instantiate((GameObject)Resources.Load("Prefabs/MessageBox"));
        msgBox.GetComponent<MessageBox>().Initialize_Ok(title, message, action);
    }
}
