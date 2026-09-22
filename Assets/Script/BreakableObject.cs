using UnityEngine;

public class BreakableObject : MonoBehaviour {
    // Çağıran obje kendini yok eder
    public void Break() {
        Destroy(gameObject);
    }
}