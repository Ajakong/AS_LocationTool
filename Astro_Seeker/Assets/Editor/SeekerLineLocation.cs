using System.IO;
using UnityEditor;
using UnityEngine;

public static class SeekerLineLocation
{
    // メニュー項目を作成
    [MenuItem("メニュー/SeekerLineデータ出力")]
    public static void OutputLocationData()
    {
        // 出力先ファイルを選択するダイアログを表示
        string fileName = EditorUtility.SaveFilePanel("出力ファイル", "", "SeekerLine", "loc");
        if (string.IsNullOrEmpty(fileName))
        {
            Debug.Log("キャンセルされました");
            return;
        }
        if (OutputData(fileName))
        {
            // 出力成功した場合
            EditorUtility.DisplayDialog("出力しました", $"{fileName}に出力しました", "閉じる");
        }
        else
        {
            EditorUtility.DisplayDialog("失敗しました", $"{fileName}出力に失敗しました", "閉じる");
        }
    }

    // Vector3 のデータを書き込むためのヘルパーメソッド
    private static void WriteVector(BinaryWriter bw, Vector3 vec)
    {
        bw.Write(vec.x);
        bw.Write(vec.y);
        bw.Write(vec.z);
    }

    // すべての階層の子オブジェクトの位置を再帰的に出力するメソッド（ワールド座標）
    private static bool WriteRecursivePosition(BinaryWriter bw, GameObject gameObject, Vector3 offsetPos, bool skipTopObject = false)
    {
        if (gameObject == null || gameObject.transform == null)
        {
            return false;
        }

        // トップオブジェクトの場合、位置をスキップ
        if (skipTopObject)
        {
            // 子オブジェクトがある場合、再帰的に処理
            for (int i = 0; i < gameObject.transform.childCount; ++i)
            {
                WriteRecursivePosition(bw, gameObject.transform.GetChild(i).gameObject, offsetPos + gameObject.transform.GetChild(i).localPosition);
            }
        }
        else // 子オブジェクトの場合
        {
            bw.Write(gameObject.name);

            // offsetPos(親オブジェクトのワールド座標)をローカル座標に足してワールド座標を出力
            WriteVector(bw, gameObject.transform.position);  // ここを修正

            // 子オブジェクトがある場合、再帰的に処理
            for (int i = 0; i < gameObject.transform.childCount; ++i)
            {
                WriteRecursivePosition(bw, gameObject.transform.GetChild(i).gameObject,gameObject.transform.GetChild(i).localPosition);
            }
        }

        return true;
    }
    // オブジェクトの数を計算するメソッド（トップオブジェクトを含まない）
    private static int GetRecursiveObjectCount(GameObject gameObject,bool skipTopObject=false)
    {
        int objectCount = 1;

        //トップオブジェクトの場合のみカウントしない
        if (skipTopObject)
        {
            objectCount= 0;
        }
        
        // 子オブジェクトがいる場合、再帰的に処理
        for (int i = 0; i < gameObject.transform.childCount; ++i)
        {
            objectCount += GetRecursiveObjectCount(gameObject.transform.GetChild(i).gameObject);
        }

        return objectCount; // 子オブジェクトのカウントを足す（トップオブジェクトを除外）
    }

    // GameObjectの位置データをバイナリファイルに出力するメイン処理
    public static bool WriteTransformData(BinaryWriter bw,GameObject obj)
    {
        Debug.Log("WriteTransformData called for: " + obj.name);

        // 現在選択中のオブジェクトを取得
        GameObject topObject = obj;
        if (topObject == null)
        {
            EditorUtility.DisplayDialog("", "トップレベルオブジェクトを選択してください", "閉じる");
            return false;
        }

        // 子オブジェクトの位置と数を出力
        WriteRecursivePosition(bw, topObject, Vector3.zero, true); // トップオブジェクトの位置を無視

        return true;
    }

    // データの書き出し処理
    private static bool OutputData(string fileName)
    {
        // トップオブジェクトが選択されていない場合は処理を終了
        GameObject obj = Selection.activeGameObject;
        if (obj == null)
        {
            EditorUtility.DisplayDialog("", "トップレベルオブジェクトを選択してください", "閉じる");
            return false; // ここで処理を中止
        }

        using (FileStream fs = File.Create(fileName))
        using (BinaryWriter binaryWriter = new BinaryWriter(fs))
        {
            binaryWriter.Write(GetRecursiveObjectCount(obj,true)); // オブジェクト数（トップオブジェクトを含めない）
            //binaryWriter.Write(obj.name); // オブジェクト名

            // トランスフォームデータの書き込み
            if (!WriteTransformData(binaryWriter,obj))
            {
                EditorUtility.DisplayDialog("失敗しました", $"{fileName}出力に失敗しました", "閉じる");
                return false;
            }
        }
        return true;
    }
}