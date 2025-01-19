using UnityEngine;
using UnityEditor;
using System.IO;

public class PlanetLocation : MonoBehaviour
{
    [MenuItem("���j���[/Planet�o��")]
    public static void OutputLocationData()
    {
        string fileName = EditorUtility.SaveFilePanel("�o�̓t�@�C��", "", "Planet", "loc");
        if (fileName == "")
        {
            Debug.Log("�L�����Z������܂���");
            return;
        }
        if (OutputData(fileName))
        {
            EditorUtility.DisplayDialog("�o�͂��܂���", fileName + "�ɏo�͂��܂���", "����");
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
        // LocationPlanet�̊e�t�B�[���h����������
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

        // Planet�I�u�W�F�N�g���V�[������擾
        Planet[] planets = FindObjectsOfType<Planet>();
        int planetCount = planets.Length;

        // �ŏ��ɃI�u�W�F�N�g�̐�����������
        binaryWriter.Write(planetCount);

        // ���ꂼ���Planet�f�[�^����������
        foreach (Planet planet in planets)
        {
            
            if (!WriteLocationPlanetData(binaryWriter, planet,planet.gameObject))
            {
                EditorUtility.DisplayDialog("���s���܂���", "�f�[�^�̏������݂Ɏ��s���܂���", "����");
                return false;
            }
        }

        fs.Close();
        return true;
    }
}