using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class KeyLockObjectOutPutter : MonoBehaviour
{
    [MenuItem("���j���[/KeyLockedObjects�o��")]
    public static void OutputLocationData()
    {
        string fileName = EditorUtility.SaveFilePanel("�o�̓t�@�C��", "", "KeyLockedObjects", "loc");
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

    private static bool WriteLocationLockedObjectData(BinaryWriter bw, KeyLockObject keyLockObject, GameObject obj)
    {

        keyLockObject.Init();
        KeyLockObject.LocationKeyLockObject locationData = keyLockObject.GetLocationData();
        // LocationBooster�̊e�t�B�[���h����������
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

        // Planet�I�u�W�F�N�g���V�[������擾
        KeyLockObject[] keyLockObjects = FindObjectsOfType<KeyLockObject>();
        int planetCount = keyLockObjects.Length;

        // �I������Ă���I�u�W�F�N�g���擾
        GameObject[] selectedObjects = Selection.gameObjects;

        // �q�I�u�W�F�N�g�̑������J�E���g
        int childObjectCount = 0;
        foreach (GameObject selectedObj in selectedObjects)
        {
            childObjectCount += selectedObj.transform.childCount;
        }
        // �ŏ��ɃI�u�W�F�N�g�̐�����������
        binaryWriter.Write(childObjectCount);
        Debug.Log(childObjectCount);
        // ���ꂼ���Planet�f�[�^����������
        foreach (KeyLockObject obj in keyLockObjects)
        {
            Debug.Log("���P�[�V�����o��");
            if (!WriteLocationLockedObjectData(binaryWriter, obj, obj.gameObject))
            {
                EditorUtility.DisplayDialog("���s���܂���", "�f�[�^�̏������݂Ɏ��s���܂���", "����");
                return false;
            }
        }

        fs.Close();
        return true;
    }
}
