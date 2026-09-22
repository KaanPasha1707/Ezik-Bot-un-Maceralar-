using UnityEngine;

public class CameraFollow : MonoBehaviour {
    [SerializeField] private Transform target;
    [SerializeField] private float height = 1.5f;
    [SerializeField] private float speed = 5f; 
    [SerializeField] private float rotationSpeed = 10f;

    private Vector3 offset;

    private void Awake() {
        if(target != null) {
            offset = target.InverseTransformPoint(transform.position);
        }
    }

    private void LateUpdate() {
        if(target != null) {
            Vector3 targetPosition = target.TransformPoint(offset);
            transform.position = Vector3.Lerp(transform.position, targetPosition, speed * Time.deltaTime);

            Vector3 LookRotation = target.position + Vector3.up * height;
            Quaternion targetRotation = Quaternion.LookRotation(LookRotation - transform.position);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }

    // FloorManager bunu çağırıyor
    public void SnapToTarget() {
        if(target == null) return;

        // Direkt istenilen yere yolla
        Vector3 targetPosition = target.TransformPoint(offset);
        transform.position = targetPosition;

        // Rotasyonu ayarla
        Vector3 targetRotation = target.position + Vector3.up * height;
        transform.LookAt(targetRotation);
    }
}