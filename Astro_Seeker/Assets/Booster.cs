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

    [SerializeField] public LocationBooster locationData; // Planetのデータを保持する変数

    public void Init()
    {
        locationData.name = this.name;
        locationData.pos = transform.position;

    }

    void Start()
    {

    }

    // 必要なデータにアクセスできるようにするメソッドなどを追加する
    public LocationBooster GetLocationData()
    {
        return locationData;
    }
}
