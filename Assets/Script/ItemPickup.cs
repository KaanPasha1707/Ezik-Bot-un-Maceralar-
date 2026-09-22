using UnityEngine;

public class ItemPickup : MonoBehaviour {
    [Header("Ayarlar")]
    [SerializeField] private GameObject objectInHand; 
    [SerializeField] private GameObject promptText;   

    private bool isPlayerClose = false;

    private void Start() {
        // Başlangıçta yazı kapalı
        if(promptText != null) promptText.SetActive(false);
    }

    private void Update() {
        // Eğer oyuncu yakınsa VE 'E' tuşuna basarsa
        if(isPlayerClose && Input.GetKeyDown(KeyCode.E)) {
            PickUpItem();
        }
    }

    // Alanın içine girince
    private void OnTriggerEnter(Collider other) {
        if(other.CompareTag("Player")) {
            isPlayerClose = true;
            if(promptText != null) promptText.SetActive(true); // Yazıyı göster
        }
    }

    // Alandan çıkınca
    private void OnTriggerExit(Collider other) {
        if(other.CompareTag("Player")) {
            isPlayerClose = false;
            if(promptText != null) promptText.SetActive(false); // Yazıyı gizle
        }
    }

    private void PickUpItem() {
        // Eldeki objeyi görünür yap
        objectInHand.SetActive(true);
        gameObject.SetActive(false);

        PlayerPrefs.SetInt("HasWrench", 1);
        PlayerPrefs.Save();
        
        Debug.Log("Obje alındı!");
    }
}