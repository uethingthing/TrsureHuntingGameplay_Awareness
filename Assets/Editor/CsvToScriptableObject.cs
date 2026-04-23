#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.IO;
using System.Collections;
using System.Collections.Generic;

public class CsvToWordSetObject : EditorWindow
{
    [MenuItem("Tools/Convert CSV to WordSetObject")]
    public static void ShowWindow()
    {
        GetWindow<CsvToWordSetObject>("CSV to WordSetObject");
    }

    private void OnGUI()
    {
        GUILayout.Label("CSVファイルを選択して変換してください", EditorStyles.boldLabel);
        if(GUILayout.Button("Convert"))
        {
            ConvertCsvToScriptableObject();
        }
    }

    private void ConvertCsvToScriptableObject()
    {
        // CSVファイルを選択
        string path = EditorUtility.OpenFilePanel("Select CSV", Application.dataPath, "csv");
        if (string.IsNullOrEmpty(path))
            return;

        // CSV読み込み
        string[] lines = File.ReadAllLines(path);

        // ScriptableObjectの作成
        WordSetObject wordSetObj = ScriptableObject.CreateInstance<WordSetObject>();
        wordSetObj.Words = new List<string>();

        // パース(１行目はヘッダーとしてスキップ)
        for(int i = 1; i < lines.Length; i++)
        {
            string[] data = lines[i].Split(',');
            if (data.Length < 1) continue;

            wordSetObj.Words.Add(data[0]);
        }

        // 保存場所とファイル名の指定
        string savePath = EditorUtility.SaveFilePanelInProject(
            "Save WordSetObject",
            "NewWordSetObject",
            "asset",
            "保存するファイル名を入力してください");

        // パスが空の場合
        if (string.IsNullOrEmpty(savePath))
            return;

        // 指定パスにアセットを作成
        AssetDatabase.CreateAsset(wordSetObj, savePath);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        Debug.Log($"Asset created at: {savePath}");
    }
}
#endif
