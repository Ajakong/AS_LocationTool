using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Planet;

public class TalkObject : MonoBehaviour
{
    [System.Serializable]
    public struct LocationTalkObject
    {
        public string name;
        public Vector3 pos;
        public string modelName;
        public string graphName;
    }
    [SerializeField] public LocationTalkObject locationData; // Planet�̃f�[�^��ێ�����ϐ�
    public void Init()
    {
        // �Ⴆ�΁A�����f�[�^���Z�b�g
        locationData.name = this.name;
        locationData.pos = transform.position;
        
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
