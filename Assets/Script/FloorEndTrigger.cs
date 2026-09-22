using UnityEngine;

public class FloorEndTrigger : MonoBehaviour {
    [SerializeField] private int nextFloorIndex;     
    [SerializeField] private Transform teleportTarget; 

    private void OnTriggerEnter(Collider other) {
        if(other.CompareTag("Player")) {
            // FloorManager'a yeni bilgileri kaydet
            FindFirstObjectByType<FloorManager>().CompleteFloor(nextFloorIndex);

            if(teleportTarget != null) {
                other.transform.position = teleportTarget.position;
                
                 // Hızı sıfırla
                if(other.TryGetComponent(out Rigidbody rb)) {
                    rb.linearVelocity = Vector3.zero; 
                }
            }
            
            // Bir kere oyun kayıt aldıktan sonra trigger'ı kapat
            gameObject.SetActive(false);
        }
    }
}