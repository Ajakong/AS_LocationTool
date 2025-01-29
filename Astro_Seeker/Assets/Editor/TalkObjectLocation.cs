using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class TalkObjectLocation : MonoBehaviour
{
    [MenuItem("メニュー/TalkObjects出力")]
    public static void OutputLocationData()
    {
        string fileName = EditorUtility.SaveFilePanel("出力ファイル", "", "TalkObject", "loc");
        if (fileName == "")
        {
            Debug.Log("キャンセルされました");
            return;
        }
        if (OutputData(fileName))
        {
            EditorUtility.DisplayDialog("出力しました", fileName + "に出力しました", "閉じる");
        }
    }

    private static void WriteVector(BinaryWriter bw, Vector3 vec)
    {
        bw.Write(vec.x);
        bw.Write(vec.y);
        bw.Write(vec.z);
    }

    private static bool WriteLocationLockedObjectData(BinaryWriter bw, GameObject obj,TalkObject to)
    {
        to.Init();
        // 子オブジェクトを対象にデータを書き込み
        bw.Write(obj.name);
        bw.Write(obj.tag);
        WriteVector(bw, obj.transform.position);
        return true;
    }

    private static bool OutputData(string fileName)
    {
        FileStream fs = File.Create(fileName);
        BinaryWriter binaryWriter = new BinaryWriter(fs);

        // Planetオブジェクトをシーンから取得
        TalkObject[] talkObjects = FindObjectsOfType<TalkObject>();
        int talkObjectCount = talkObjects.Length;

        // 選択されているオブジェクトを取得
        GameObject[] selectedObjects = Selection.gameObjects;

        // 子オブジェクトの総数をカウント
        int childObjectCount = 0;
        foreach (GameObject selectedObj in selectedObjects)
        {
            childObjectCount += selectedObj.transform.childCount;
        }
        // 最初にオブジェクトの数を書き込む
        binaryWriter.Write(childObjectCount);
        Debug.Log(childObjectCount);
        // それぞれのPlanetデータを書き込む
        foreach (TalkObject obj in talkObjects)
        {
            Debug.Log("ロケーション出力");
            if (!WriteLocationLockedObjectData(binaryWriter, obj.gameObject, obj))
            {
                EditorUtility.DisplayDialog("失敗しました", "データの書き込みに失敗しました", "閉じる");
                return false;
            }
        }

        fs.Close();
        return true;
    }
}