using UnityEngine;
//必要なライブラリ
using UnityEditor;//メニュー等を出すために必要なもの
using System.IO;


public class LocationOutputter : MonoBehaviour
{
    [MenuItem("メニュー/data.loc出力")]
    public static void OutputLocationData()
    {
        string fileName = EditorUtility.SaveFilePanel("出力ファイル", "", "data", "loc");
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

    private static bool WriteRecursiveData(BinaryWriter bw, GameObject gameObject, Vector3 offsetPos, Vector3 offsetRot,
        Vector3 mulScale)
    {
        if (gameObject == null)
        {
            return false;
        }
        if (gameObject.transform == null)
        {
            return false;
        }
        if (gameObject.transform.childCount == 0)
        {
            bw.Write(gameObject.name);//先頭1バイトが文字列数、あとは文字列データ

            bw.Write(gameObject.tag);//先頭1バイトが文字列数、あとは文字列データ

            WriteVector(bw, offsetPos + gameObject.transform.localPosition);//座標
            WriteVector(bw, offsetRot + gameObject.transform.localRotation.eulerAngles);//回転(XYZ軸のオイラー回転)
            WriteVector(bw, Vector3.Scale(mulScale, gameObject.transform.localScale));//スケーリング
        }
        else
        {
            for (int i = 0; i < gameObject.transform.childCount; ++i)
            {
                WriteRecursiveData(bw, gameObject.transform.GetChild(i).gameObject,
                    offsetPos + gameObject.transform.localPosition,
                    offsetRot + gameObject.transform.localEulerAngles,
                    Vector3.Scale(mulScale, gameObject.transform.localScale));
            }
        }
        return true;
    }

    private static int GetRecursiveLeafCount(GameObject gameObject)
    {
        int cnt = gameObject.transform.childCount;
        //子供がいない＝リーフ(末端)
        if (cnt == 0)
        {
            return 1;//自分しかいないため１を返す
        }
        else//子供がいた場合
        {
            int sum = 0;
            for (int i = 0; i < cnt; ++i)//カウントは子供にさせます
            {
                sum += GetRecursiveLeafCount(gameObject.transform.GetChild(i).gameObject);
            }
            return sum;
        }

    }

    private static int CalculateLeafCount(GameObject gameObject)
    {
        return GetRecursiveLeafCount(gameObject);
    }

    private static bool WriteTransformData(BinaryWriter bw)
    {
        //現在選択中のオブジェクトを取ってくる
        GameObject topObject = Selection.activeGameObject;
        if (topObject == null)
        {
            EditorUtility.DisplayDialog("", "トップレベルオブジェクトを選択してください", "閉じる");
            return false;
        }
        bw.Write(CalculateLeafCount(topObject));
        WriteRecursiveData(bw, topObject, new Vector3(0, 0, 0), new Vector3(0, 0, 0), new Vector3(1.0f, 1.0f, 1.0f));

        return true;
    }

    private static bool OutputData(string fileName)
    {
        FileStream fs = File.Create(fileName);
        BinaryWriter binaryWriter = new BinaryWriter(fs);
        if (!WriteTransformData(binaryWriter))
        {
            EditorUtility.DisplayDialog("失敗しました", fileName + "出力に失敗しました", "閉じる");
            return false;
        }
        fs.Close();
        return true;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
