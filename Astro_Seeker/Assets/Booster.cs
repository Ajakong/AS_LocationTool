using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Booster : MonoBehaviour
{
    [System.Serializable]
    public struct LocationBooster
    {
        public string name;
        public Vector3 pos;
        public Vector3 dir;
        public string modelName;
        public float power;
    }

    [SerializeField] public LocationBooster locationData; // Planet�̃f�[�^��ێ�����ϐ�

    public void Init()
    {
        locationData.name = this.name;
        locationData.pos = transform.position;

    }

    void Start()
    {

    }

    // �K�v�ȃf�[�^�ɃA�N�Z�X�ł���悤�ɂ��郁�\�b�h�Ȃǂ�ǉ�����
    public LocationBooster GetLocationData()
    {
        return locationData;
    }
}
