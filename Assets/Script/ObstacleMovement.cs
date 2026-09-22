using UnityEngine;

public class ObstacleMovement : MonoBehaviour {
    [Header("Engel Ayarları")]
    [SerializeField] private float hiz = 2f;      
    [SerializeField] private float mesafe = 3f;  
    
    private Vector3 baslangicKonumu;

    private void Start() {
        // Oyun başladığında pistonun durduğu yeri kaydediyoruz
        baslangicKonumu = transform.position;
    }

    private void Update() {
        // sin -1 ile 1 arasında gidip gelir. Bunu mesafe ile çarparak sağa sola hareketi sağlarız
        float yeniX = baslangicKonumu.x + Mathf.Sin(Time.time * hiz) * mesafe;

        // Pistonun pozisyonunu güncelliyoruz 
        transform.position = new Vector3(yeniX, baslangicKonumu.y, baslangicKonumu.z);
    }
}
