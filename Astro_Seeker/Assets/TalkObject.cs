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
    [SerializeField] public LocationTalkObject locationData; // Planetのデータを保持する変数
    public void Init()
    {
        // 例えば、初期データをセット
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
