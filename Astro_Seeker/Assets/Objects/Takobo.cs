using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Takobo : MonoBehaviour
{

    [System.Serializable]
    public struct TakoboProperty
    {
        public int hp;
    }
    [SerializeField] public TakoboProperty property; // データを保持する変数
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // 必要なデータにアクセスできるようにするメソッドなどを追加する
    public TakoboProperty GetLocationData()
    {
        return property;
    }
}
