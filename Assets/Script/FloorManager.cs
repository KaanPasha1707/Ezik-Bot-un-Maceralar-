using UnityEngine;

public class FloorManager : MonoBehaviour {
     [Header("Ayarlar")]
    [SerializeField] private Transform player; 
    [SerializeField] private Transform[] floorSpawnPoints; 
    [SerializeField] private CameraFollow camera; 

    [Header("Eşyalar")]
    [SerializeField] private GameObject handWrench; // Karakterin elindeki gizli obje
    [SerializeField] private GameObject groundWrench; // Yerdeki obje

    private const string FLOOR_KEY = "SavedFloorIndex"; 
    private const string WRENCH_KEY = "HasWrench";

    private void Start() {
        LoadSavedFloor();
    }

   private void LoadSavedFloor() {
        if(floorSpawnPoints == null || floorSpawnPoints.Length == 0) {
            Debug.LogWarning("Floor spawn noktaları atanmamış");
            return;
        }

        int savedFloorIndex = PlayerPrefs.GetInt(FLOOR_KEY, 0);
        savedFloorIndex = Mathf.Clamp(savedFloorIndex, 0, floorSpawnPoints.Length - 1);

        Transform targetSpawn = floorSpawnPoints[savedFloorIndex];

        // Pozisyonu ve rotasyonu ayarla
        player.position = targetSpawn.position;
        player.rotation = targetSpawn.rotation; 
        
        if(player.TryGetComponent(out Rigidbody rb)) {
            rb.linearVelocity = Vector3.zero; 
            rb.angularVelocity = Vector3.zero; // Dönme hızını sıfırla
        }
        
        // Kamerayı ışınla 
        if(camera != null) {
            camera.SnapToTarget();
        }

        // Eğer eşya alınmışsa
        if(PlayerPrefs.GetInt(WRENCH_KEY, 0) == 1) {
        if(handWrench != null) handWrench.SetActive(true);    // Eldekini görünür yap
        if(groundWrench != null) groundWrench.SetActive(false); // Yerdekini sil
        }
        // Eğer eşya alınmamışsa
        else {
        if(handWrench != null) handWrench.SetActive(false);   // Eldekini gizle
        if(groundWrench != null) groundWrench.SetActive(true);  // Yerdekini göster
        }

        Debug.Log("Oyun " + savedFloorIndex + ". kattan başlatıldı");
    }
    
    public void CompleteFloor(int nextFloorIndex) {
        if(floorSpawnPoints == null || floorSpawnPoints.Length == 0) {
            Debug.LogWarning("Save işlemi başarısız: floorSpawnPoints yok");
            return;
        }

        int validatedFloorIndex = Mathf.Clamp(nextFloorIndex, 0, floorSpawnPoints.Length - 1);
        int currentSavedFloor = PlayerPrefs.GetInt(FLOOR_KEY, 0);

        // Default 0 veya geçersiz değer save'i bozmasın, sadece ilerleme olduğunda kaydet
        if(validatedFloorIndex > currentSavedFloor || !PlayerPrefs.HasKey(FLOOR_KEY)) {
            PlayerPrefs.SetInt(FLOOR_KEY, validatedFloorIndex);
            PlayerPrefs.Save();
            Debug.Log("İlerleme Kaydedildi! Yeni Kat: " + validatedFloorIndex);
        }
        else {
            Debug.Log("İlerleme yok: önceki kat = " + currentSavedFloor + ", yeni kat = " + validatedFloorIndex);
        }
    }
    
    // Kaydı sıfırla 
    public void ResetSave() {
        PlayerPrefs.DeleteKey(FLOOR_KEY);
        PlayerPrefs.DeleteKey(WRENCH_KEY);
        PlayerPrefs.Save();
    }
}