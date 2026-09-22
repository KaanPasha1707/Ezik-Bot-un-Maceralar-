using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class PlayerAttack : MonoBehaviour {
    [Header("Ayarlar")]
    [SerializeField] private GameInput gameInput;
    [SerializeField] private Transform attackPoint; // Saldırının merkezi 
    [SerializeField] private float attackRange = 1.5f; // Ne kadar uzağa vurabilir?
    [SerializeField] private LayerMask breakableLayers; // Neleri kırabilir?
    [SerializeField] private float hitDelay = 0.4f; // saldırının objeye değdiğinden emin olmak için gecikme süresi

    private void Start() {
        if(gameInput == null) return;

        // GameInput'tan gelen tıklama haberini dinle
        gameInput.attackEvent += GameInput_OnAttackAction;
    }

    private void GameInput_OnAttackAction(object sender, System.EventArgs e) {
        if(EventSystem.current != null && EventSystem.current.IsPointerOverGameObject()) return;
        StartCoroutine(AttackRoutine());
    }

   private IEnumerator AttackRoutine() {
        yield return new WaitForSeconds(hitDelay);

        if(attackPoint == null) {
            yield break;
        }

        // Sadece breakableLayers nesneleri kır
        Collider[] hitObjects = Physics.OverlapSphere(attackPoint.position, attackRange, breakableLayers);

        foreach (Collider obj in hitObjects) {
            if(obj == null) continue;

            if (obj.TryGetComponent(out BreakableObject breakable)) {
                breakable.Break();
            }
        }
    }
}