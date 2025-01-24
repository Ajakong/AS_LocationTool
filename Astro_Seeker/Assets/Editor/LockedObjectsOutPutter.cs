using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class LockedObjectsOutPutter : MonoBehaviour
{
    [MenuItem("メニュー/LockedObjects出力")]
    public static void OutputLocationData()
    {

        string fileName = EditorUtility.SaveFilePanel("出力ファイル", "", "LockedObject", "loc");
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

    private static bool WriteLocationLockedObjectData(BinaryWriter bw, GameObject obj)
    {
        Debug.Log("WriteLocationLockedObjectData called for: " + obj.name);

        // 子オブジェクトを対象にデータを書き込み
        bw.Write(obj.name);
        bw.Write(obj.tag);
        WriteVector(bw, obj.transform.position);

        if (obj.tag == "Booster")
        {
            // 子オブジェクトの Booster コンポーネントがあれば、そのデータも書き込む
            Booster booster = obj.GetComponent<Booster>();
            booster.Init();
            Booster.LocationBooster locationData = booster.GetLocationData();
            WriteVector(bw, locationData.dir);
            bw.Write(locationData.modelName);
            bw.Write(locationData.power);
        }
        if (obj.tag == "SeekerLine")
        {
            SeekerLineLocation.WriteTransformData(bw, obj);
        }

        return true;
    }

    private static bool OutputData(string fileName)
    {
        FileStream fs = File.Create(fileName);
        BinaryWriter binaryWriter = new BinaryWriter(fs);

        // 選択されているオブジェクトを取得
        GameObject[] selectedObjects = Selection.gameObjects;
        int selectedObjectCount = selectedObjects.Length;

        // 最初に選択されたオブジェクトの数を書き込む
        binaryWriter.Write(selectedObjectCount);

        // 各選択されたオブジェクトの子オブジェクトのデータを書き込む
        foreach (GameObject selectedObj in selectedObjects)
        {
            // 直接の子オブジェクトを全て取得
            foreach (Transform child in selectedObj.transform)
            {
                if (!WriteLocationLockedObjectData(binaryWriter, child.gameObject))
                {
                    EditorUtility.DisplayDialog("失敗しました", "データの書き込みに失敗しました", "閉じる");
                    fs.Close();
                    return false;
                }
            }
        }

        fs.Close();
        return true;
    }
}