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

    [SerializeField] public LocationPlanet locationData; // Planetのデータを保持する変数
    
    public void Init()
    {
        // 例えば、初期データをセット
        locationData.name = this.name;
        locationData.pos = transform.position;
        locationData.color = 0x00FF00;  // 緑
        //50が惑星の基本の大きさ、/2は直径から半径に
        locationData.scale = transform.localScale.x / 50/2;
    }

    void Start()
    {
        
    }

    // 必要なデータにアクセスできるようにするメソッドなどを追加する
    public LocationPlanet GetLocationData()
    {
        return locationData;
    }
}