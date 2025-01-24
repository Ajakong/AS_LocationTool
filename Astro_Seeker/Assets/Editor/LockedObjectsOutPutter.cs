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
        Debug.Log("WriteLocationLockedObjectData called for: " + obj.name);

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
            SeekerLineLocation.WriteTransformData(bw, obj);
        }

        return true;
    }

    private static bool OutputData(string fileName)
    {
        FileStream fs = File.Create(fileName);
        BinaryWriter binaryWriter = new BinaryWriter(fs);

        // �I������Ă���I�u�W�F�N�g���擾
        GameObject[] selectedObjects = Selection.gameObjects;
        int selectedObjectCount = selectedObjects.Length;

        // �ŏ��ɑI�����ꂽ�I�u�W�F�N�g�̐�����������
        binaryWriter.Write(selectedObjectCount);

        // �e�I�����ꂽ�I�u�W�F�N�g�̎q�I�u�W�F�N�g�̃f�[�^����������
        foreach (GameObject selectedObj in selectedObjects)
        {
            // ���ڂ̎q�I�u�W�F�N�g��S�Ď擾
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