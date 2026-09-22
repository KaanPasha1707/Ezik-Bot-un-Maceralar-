using UnityEngine;

public class PistonHareket : MonoBehaviour {
    [Header("Piston Ayarları")]
    [SerializeField] private float hiz = 2f;      
    [SerializeField] private float mesafe = 3f;  
    
    private Vector3 baslangicKonumu;

    private void Start() {
        // Oyun başladığında pistonun durduğu yeri kaydediyoruz
        baslangicKonumu = transform.position;
    }

    private void Update() {
        // sin -1 ile 1 arasında gidip gelir. Bunu mesafe ile çarparak yukarı aşağı hareketi sağlarız
        float yeniY = baslangicKonumu.y + Mathf.Sin(Time.time * hiz) * mesafe;

        // Pistonun pozisyonunu güncelliyoruz 
        transform.position = new Vector3(baslangicKonumu.x, yeniY, baslangicKonumu.z);
    }
}