using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kuribo : MonoBehaviour
{
    [System.Serializable]
    public struct KuriboProperty
    {
        public int hp;
        public float speed;
        
    }
    [SerializeField] public KuriboProperty property; // データを保持する変数
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // 必要なデータにアクセスできるようにするメソッドなどを追加する
    public KuriboProperty GetLocationData()
    {
        return property;
    }
}
