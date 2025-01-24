using System.IO;
using UnityEditor;
using UnityEngine;

public static class SeekerLineLocation
{
    // ���j���[���ڂ��쐬
    [MenuItem("���j���[/SeekerLine�f�[�^�o��")]
    public static void OutputLocationData()
    {
        // �o�͐�t�@�C����I������_�C�A���O��\��
        string fileName = EditorUtility.SaveFilePanel("�o�̓t�@�C��", "", "SeekerLine", "loc");
        if (string.IsNullOrEmpty(fileName))
        {
            Debug.Log("�L�����Z������܂���");
            return;
        }
        if (OutputData(fileName))
        {
            // �o�͐��������ꍇ
            EditorUtility.DisplayDialog("�o�͂��܂���", $"{fileName}�ɏo�͂��܂���", "����");
        }
        else
        {
            EditorUtility.DisplayDialog("���s���܂���", $"{fileName}�o�͂Ɏ��s���܂���", "����");
        }
    }

    // Vector3 �̃f�[�^���������ނ��߂̃w���p�[���\�b�h
    private static void WriteVector(BinaryWriter bw, Vector3 vec)
    {
        bw.Write(vec.x);
        bw.Write(vec.y);
        bw.Write(vec.z);
    }

    // ���ׂĂ̊K�w�̎q�I�u�W�F�N�g�̈ʒu���ċA�I�ɏo�͂��郁�\�b�h�i���[���h���W�j
    private static bool WriteRecursivePosition(BinaryWriter bw, GameObject gameObject, Vector3 offsetPos, bool skipTopObject = false)
    {
        if (gameObject == null || gameObject.transform == null)
        {
            return false;
        }

        // �g�b�v�I�u�W�F�N�g�̏ꍇ�A�ʒu���X�L�b�v
        if (skipTopObject)
        {
            // �q�I�u�W�F�N�g������ꍇ�A�ċA�I�ɏ���
            for (int i = 0; i < gameObject.transform.childCount; ++i)
            {
                WriteRecursivePosition(bw, gameObject.transform.GetChild(i).gameObject, offsetPos + gameObject.transform.GetChild(i).localPosition);
            }
        }
        else // �q�I�u�W�F�N�g�̏ꍇ
        {
            bw.Write(gameObject.name);

            // offsetPos(�e�I�u�W�F�N�g�̃��[���h���W)�����[�J�����W�ɑ����ă��[���h���W���o��
            WriteVector(bw, gameObject.transform.position);  // �������C��

            // �q�I�u�W�F�N�g������ꍇ�A�ċA�I�ɏ���
            for (int i = 0; i < gameObject.transform.childCount; ++i)
            {
                WriteRecursivePosition(bw, gameObject.transform.GetChild(i).gameObject,gameObject.transform.GetChild(i).localPosition);
            }
        }

        return true;
    }
    // �I�u�W�F�N�g�̐����v�Z���郁�\�b�h�i�g�b�v�I�u�W�F�N�g���܂܂Ȃ��j
    private static int GetRecursiveObjectCount(GameObject gameObject,bool skipTopObject=false)
    {
        int objectCount = 1;

        //�g�b�v�I�u�W�F�N�g�̏ꍇ�̂݃J�E���g���Ȃ�
        if (skipTopObject)
        {
            objectCount= 0;
        }
        
        // �q�I�u�W�F�N�g������ꍇ�A�ċA�I�ɏ���
        for (int i = 0; i < gameObject.transform.childCount; ++i)
        {
            objectCount += GetRecursiveObjectCount(gameObject.transform.GetChild(i).gameObject);
        }

        return objectCount; // �q�I�u�W�F�N�g�̃J�E���g�𑫂��i�g�b�v�I�u�W�F�N�g�����O�j
    }

    // GameObject�̈ʒu�f�[�^���o�C�i���t�@�C���ɏo�͂��郁�C������
    public static bool WriteTransformData(BinaryWriter bw,GameObject obj)
    {
        Debug.Log("WriteTransformData called for: " + obj.name);

        // ���ݑI�𒆂̃I�u�W�F�N�g���擾
        GameObject topObject = obj;
        if (topObject == null)
        {
            EditorUtility.DisplayDialog("", "�g�b�v���x���I�u�W�F�N�g��I�����Ă�������", "����");
            return false;
        }

        // �q�I�u�W�F�N�g�̈ʒu�Ɛ����o��
        WriteRecursivePosition(bw, topObject, Vector3.zero, true); // �g�b�v�I�u�W�F�N�g�̈ʒu�𖳎�

        return true;
    }

    // �f�[�^�̏����o������
    private static bool OutputData(string fileName)
    {
        // �g�b�v�I�u�W�F�N�g���I������Ă��Ȃ��ꍇ�͏������I��
        GameObject obj = Selection.activeGameObject;
        if (obj == null)
        {
            EditorUtility.DisplayDialog("", "�g�b�v���x���I�u�W�F�N�g��I�����Ă�������", "����");
            return false; // �����ŏ����𒆎~
        }

        using (FileStream fs = File.Create(fileName))
        using (BinaryWriter binaryWriter = new BinaryWriter(fs))
        {
            binaryWriter.Write(GetRecursiveObjectCount(obj,true)); // �I�u�W�F�N�g���i�g�b�v�I�u�W�F�N�g���܂߂Ȃ��j
            //binaryWriter.Write(obj.name); // �I�u�W�F�N�g��

            // �g�����X�t�H�[���f�[�^�̏�������
            if (!WriteTransformData(binaryWriter,obj))
            {
                EditorUtility.DisplayDialog("���s���܂���", $"{fileName}�o�͂Ɏ��s���܂���", "����");
                return false;
            }
        }
        return true;
    }
}