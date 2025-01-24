using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyLockObject : MonoBehaviour
{
    [System.Serializable]
    public struct LocationKeyLockObject
    {
        public string name;
        public Vector3 pos;
        public int connectObjectNumber;
        
    }

    [SerializeField] public LocationKeyLockObject locationData; // Planet�̃f�[�^��ێ�����ϐ�

    public void Init()
    {
        // �Ⴆ�΁A�����f�[�^���Z�b�g
        locationData.name = this.name;
        locationData.pos = transform.position;
     
    }

    void Start()
    {

    }

    // �K�v�ȃf�[�^�ɃA�N�Z�X�ł���悤�ɂ��郁�\�b�h�Ȃǂ�ǉ�����
    public LocationKeyLockObject GetLocationData()
    {
        return locationData;
    }
}
