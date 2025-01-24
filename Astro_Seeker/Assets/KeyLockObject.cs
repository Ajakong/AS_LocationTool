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

    [SerializeField] public LocationKeyLockObject locationData; // Planetのデータを保持する変数

    public void Init()
    {
        // 例えば、初期データをセット
        locationData.name = this.name;
        locationData.pos = transform.position;
     
    }

    void Start()
    {

    }

    // 必要なデータにアクセスできるようにするメソッドなどを追加する
    public LocationKeyLockObject GetLocationData()
    {
        return locationData;
    }
}
