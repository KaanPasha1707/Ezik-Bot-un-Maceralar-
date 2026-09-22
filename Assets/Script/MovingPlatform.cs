using UnityEngine;

public class MovingPlatform : MonoBehaviour {
    [Header("Ayarlar")]
    [SerializeField] private Vector3 moveOffset = new Vector3(0, 5, 5); 
    [SerializeField] private float speed = 3f;

    private Vector3 startPosition;
    private Vector3 targetPosition;
    private bool move = false;

    private void Start() {
        // Başlangıç pozisyonunu kaydet
        startPosition = transform.position;
        // Hedef pozisyonu hesapla
        targetPosition = startPosition + moveOffset;
    }

    private void Update() {
        // Eğer hareket izni verildiyse hedefe doğru git
        if (move) {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
        }
    }

    // Karakter platforma basınca
    private void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.CompareTag("Player")) {
            // Hareketi başlat
            move = true;

            // Platformu karakterin ebeveyni yapıyoruz böylece platformla beraber hareket eder
            collision.transform.SetParent(this.transform);
        }
    }

    private void OnCollisionExit(Collision collision) {
        if(collision.gameObject.CompareTag("Player")) {
            // Karakterin parentı olmaktan çık
            collision.transform.SetParent(null);
        }
    }
}