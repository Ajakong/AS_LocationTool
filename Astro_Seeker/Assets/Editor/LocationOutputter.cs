using UnityEngine;
//�K�v�ȃ��C�u����
using UnityEditor;//���j���[�����o�����߂ɕK�v�Ȃ���
using System.IO;


public class LocationOutputter : MonoBehaviour
{
    [MenuItem("���j���[/data.loc�o��")]
    public static void OutputLocationData()
    {
        string fileName = EditorUtility.SaveFilePanel("�o�̓t�@�C��", "", "data", "loc");
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
            bw.Write(gameObject.name);//�擪1�o�C�g�������񐔁A���Ƃ͕�����f�[�^

            bw.Write(gameObject.tag);//�擪1�o�C�g�������񐔁A���Ƃ͕�����f�[�^

            WriteVector(bw, offsetPos + gameObject.transform.localPosition);//���W
            WriteVector(bw, offsetRot + gameObject.transform.localRotation.eulerAngles);//��](XYZ���̃I�C���[��])
            WriteVector(bw, Vector3.Scale(mulScale, gameObject.transform.localScale));//�X�P�[�����O
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
        //�q�������Ȃ������[�t(���[)
        if (cnt == 0)
        {
            return 1;//�����������Ȃ����߂P��Ԃ�
        }
        else//�q���������ꍇ
        {
            int sum = 0;
            for (int i = 0; i < cnt; ++i)//�J�E���g�͎q���ɂ����܂�
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
        //���ݑI�𒆂̃I�u�W�F�N�g������Ă���
        GameObject topObject = Selection.activeGameObject;
        if (topObject == null)
        {
            EditorUtility.DisplayDialog("", "�g�b�v���x���I�u�W�F�N�g��I�����Ă�������", "����");
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
            EditorUtility.DisplayDialog("���s���܂���", fileName + "�o�͂Ɏ��s���܂���", "����");
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
