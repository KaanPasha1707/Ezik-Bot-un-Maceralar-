using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerAnimator : MonoBehaviour {

  [SerializeField] private Animator animator;
  [SerializeField] private GameInput gameInput;

  private const string IS_MOVING = "IsMoving"; 
  private const string ATTACK_TRIGGER = "Attack";
  [SerializeField] private Player player;

  private void Awake(){
  animator = GetComponent<Animator>();
  gameInput.attackEvent += GameInput_OnAttackAction;
  }

  private void Update(){
  animator.SetBool(IS_MOVING, player.IsMoving());
  }

  private void GameInput_OnAttackAction(object sender, System.EventArgs e) {
        // Eğer mouse bir butonun veya UI elemanının üzerindeyse kodu durdur
        if (EventSystem.current.IsPointerOverGameObject()) return;

        // Saldırı tuşuna basılınca animasyonu oynat
        animator.SetTrigger(ATTACK_TRIGGER);
  }
}
