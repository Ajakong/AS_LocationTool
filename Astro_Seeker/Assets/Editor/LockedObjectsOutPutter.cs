using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class LockedObjectsOutPutter : MonoBehaviour
{
    [MenuItem("���j���[/LockedObjects�o��")]
    public static void OutputLocationData()
    {
        string fileName = EditorUtility.SaveFilePanel("�o�̓t�@�C��", "", "LockedObject", "loc");
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

    private static bool WriteLocationLockedObjectData(BinaryWriter bw, GameObject obj)
    {
        
        // �q�I�u�W�F�N�g��ΏۂɃf�[�^����������
        bw.Write(obj.name);
        bw.Write(obj.tag);
        WriteVector(bw, obj.transform.position);

        if (obj.tag == "Booster")
        {
            // �q�I�u�W�F�N�g�� Booster �R���|�[�l���g������΁A���̃f�[�^����������
            Booster booster = obj.GetComponent<Booster>();
            booster.Init();
            Booster.LocationBooster locationData = booster.GetLocationData();
            WriteVector(bw, locationData.dir);
            bw.Write(locationData.modelName);
            bw.Write(locationData.power);
        }
        if (obj.tag == "SeekerLine")
        {
            int num=SeekerLineLocation.GetRecursiveObjectCount(obj,true);
            Debug.Log(num);
            bw.Write(num);
            SeekerLineLocation.WriteRecursivePosition(bw, obj, Vector3.zero, true);
        }

        return true;
    }

    private static bool OutputData(string fileName)
    {
        FileStream fs = File.Create(fileName);
        BinaryWriter binaryWriter = new BinaryWriter(fs);

        // �I������Ă���I�u�W�F�N�g���擾
        GameObject[] selectedObjects = Selection.gameObjects;

        // �q�I�u�W�F�N�g�̑������J�E���g
        int childObjectCount = 0;
        foreach (GameObject selectedObj in selectedObjects)
        {
            childObjectCount += selectedObj.transform.childCount;
        }

        // �ŏ��Ɏq�I�u�W�F�N�g�̑�������������
        binaryWriter.Write(childObjectCount);

        

        // �e�I�����ꂽ�I�u�W�F�N�g�̎q�I�u�W�F�N�g�̃f�[�^����������
        foreach (GameObject selectedObj in selectedObjects)
        {
            foreach (Transform child in selectedObj.transform)
            {
                if (!WriteLocationLockedObjectData(binaryWriter, child.gameObject))
                {
                    EditorUtility.DisplayDialog("���s���܂���", "�f�[�^�̏������݂Ɏ��s���܂���", "����");
                    fs.Close();
                    return false;
                }
            }
        }

        fs.Close();
        return true;
    }
}