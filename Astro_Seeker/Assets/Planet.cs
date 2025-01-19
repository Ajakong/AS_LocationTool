using UnityEngine;

public class Planet : MonoBehaviour
{
    [System.Serializable]
    public struct LocationPlanet
    {
        public string name;
        public Vector3 pos;
        public int color;
        public float gravityPower;
        public string modelName;
        public float coefficientOfFriction;
        public float scale; // 1/50
    }

    [SerializeField] public LocationPlanet locationData; // Planet�̃f�[�^��ێ�����ϐ�
    
    public void Init()
    {
        // �Ⴆ�΁A�����f�[�^���Z�b�g
        locationData.name = this.name;
        locationData.pos = transform.position;
        locationData.color = 0x00FF00;  // ��
        //50���f���̊�{�̑傫���A/2�͒��a���甼�a��
        locationData.scale = transform.localScale.x / 50/2;
    }

    void Start()
    {
        
    }

    // �K�v�ȃf�[�^�ɃA�N�Z�X�ł���悤�ɂ��郁�\�b�h�Ȃǂ�ǉ�����
    public LocationPlanet GetLocationData()
    {
        return locationData;
    }
}