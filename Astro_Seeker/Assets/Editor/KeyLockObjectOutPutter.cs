using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class KeyLockObjectOutPutter : MonoBehaviour
{
    [MenuItem("メニュー/KeyLockedObjects出力")]
    public static void OutputLocationData()
    {
        string fileName = EditorUtility.SaveFilePanel("出力ファイル", "", "KeyLockedObjects", "loc");
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

    private static bool WriteLocationLockedObjectData(BinaryWriter bw, KeyLockObject keyLockObject, GameObject obj)
    {

        keyLockObject.Init();
        KeyLockObject.LocationKeyLockObject locationData = keyLockObject.GetLocationData();
        // LocationBoosterの各フィールドを書き込み
        bw.Write(obj.name);
        bw.Write(obj.tag);
        WriteVector(bw, obj.transform.position);
        bw.Write(locationData.connectObjectNumber);
       
        return true;
    }

    private static bool OutputData(string fileName)
    {
        FileStream fs = File.Create(fileName);
        BinaryWriter binaryWriter = new BinaryWriter(fs);

        // Planetオブジェクトをシーンから取得
        KeyLockObject[] keyLockObjects = FindObjectsOfType<KeyLockObject>();
        int planetCount = keyLockObjects.Length;

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
        foreach (KeyLockObject obj in keyLockObjects)
        {
            Debug.Log("ロケーション出力");
            if (!WriteLocationLockedObjectData(binaryWriter, obj, obj.gameObject))
            {
                EditorUtility.DisplayDialog("失敗しました", "データの書き込みに失敗しました", "閉じる");
                return false;
            }
        }

        fs.Close();
        return true;
    }
}
