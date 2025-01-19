using UnityEngine;
using UnityEditor;
using System.IO;

public class PlanetLocation : MonoBehaviour
{
    [MenuItem("メニュー/Planet出力")]
    public static void OutputLocationData()
    {
        string fileName = EditorUtility.SaveFilePanel("出力ファイル", "", "Planet", "loc");
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

    private static bool WriteLocationPlanetData(BinaryWriter bw, Planet planet,GameObject obj)
    {
       
        planet.Init();
        Planet.LocationPlanet locationData = planet.GetLocationData();
        // LocationPlanetの各フィールドを書き込み
        bw.Write(obj.name);
        WriteVector(bw,obj.transform.position);
        bw.Write(locationData.color);
        bw.Write(locationData.gravityPower);
        bw.Write(locationData.modelName);
        bw.Write(locationData.coefficientOfFriction);
        bw.Write(locationData.scale);

        return true;
    }

    private static bool OutputData(string fileName)
    {
        FileStream fs = File.Create(fileName);
        BinaryWriter binaryWriter = new BinaryWriter(fs);

        // Planetオブジェクトをシーンから取得
        Planet[] planets = FindObjectsOfType<Planet>();
        int planetCount = planets.Length;

        // 最初にオブジェクトの数を書き込む
        binaryWriter.Write(planetCount);

        // それぞれのPlanetデータを書き込む
        foreach (Planet planet in planets)
        {
            
            if (!WriteLocationPlanetData(binaryWriter, planet,planet.gameObject))
            {
                EditorUtility.DisplayDialog("失敗しました", "データの書き込みに失敗しました", "閉じる");
                return false;
            }
        }

        fs.Close();
        return true;
    }
}