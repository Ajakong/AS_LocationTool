using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class TalkObjectLocation : MonoBehaviour
{
    [MenuItem("���j���[/LockedObjects�o��")]
    public static void OutputLocationData()
    {
        string fileName = EditorUtility.SaveFilePanel("�o�̓t�@�C��", "", "TalkObject", "loc");
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

    private static bool WriteLocationLockedObjectData(BinaryWriter bw, GameObject obj,TalkObject to)
    {
        to.Init();
        // �q�I�u�W�F�N�g��ΏۂɃf�[�^����������
        bw.Write(obj.name);
        bw.Write(obj.tag);
        WriteVector(bw, obj.transform.position);
        bw.Write(to.locationData.modelName);
        bw.Write(to.locationData.graphName);

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
                foreach (TalkObject obj in child)
                {
                    Debug.Log("���P�[�V�����o��");
                    if (!WriteLocationLockedObjectData(binaryWriter, child.gameObject, obj))
                    {
                        EditorUtility.DisplayDialog("���s���܂���", "�f�[�^�̏������݂Ɏ��s���܂���", "����");
                        return false;
                    }
                }
            }
        }

        fs.Close();
        return true;
    }
}