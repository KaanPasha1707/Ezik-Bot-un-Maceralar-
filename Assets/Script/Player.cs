using UnityEngine;

public class Player : MonoBehaviour {
    [Header("Second Jump Settings")]
    [SerializeField] private float secondJumpTime = 0.2f; // havadayken ikinci zıplama için süre

    // State'lerin erişeceği değişkenler
    public float SecondJumpTimeCounter { get; set; }
    public float SecondJumpDuration => secondJumpTime; // Getter
    public bool IsJumping { get; set; } 

    [Header("Movement Settings")]
    [SerializeField] private float jumpForce = 5f; 
    [SerializeField] private LayerMask groundLayer; 
    [SerializeField] private float moveSpeed = 7f;
    [SerializeField] private float turnSpeed = 10f;
    [SerializeField] private GameInput gameInput;

    // State'lerin erişmesi gereken değişkenler 
    public Rigidbody rb { get; private set; }
    public GameInput GameInput => gameInput; // Getter

    // State Machine Tanımları
    public PlayerStateMachine StateMachine { get; private set; }
    public PlayerIdleState IdleState { get; private set; }
    public PlayerMoveState MoveState { get; private set; }
    public PlayerJumpState JumpState { get; private set; }
    public PlayerAirState AirState { get; private set; }

    private Transform cameraTransform;
    private bool isMoving; 

    private void Awake() {
        rb = GetComponent<Rigidbody>();
        
        // Kamera referansı
        if(Camera.main != null) 
            cameraTransform = Camera.main.transform;
        else 
            Debug.LogError("Sahne'de kamera bulunamadı!");

        StateMachine = new PlayerStateMachine();
        IdleState = new PlayerIdleState(this, StateMachine);
        MoveState = new PlayerMoveState(this, StateMachine);
        JumpState = new PlayerJumpState(this, StateMachine);
        AirState = new PlayerAirState(this, StateMachine);
    }

    private void Start() {
        gameInput.jumpEvent += GameInput_OnJumpAction;
        
        // Oyuna Idle (Durarak) başla
        StateMachine.Initialize(IdleState);
    }

    private void GameInput_OnJumpAction(object sender, System.EventArgs e) {
        if (!GameManager.Instance.IsGamePlaying()) return;

        // İsteği State Machine'e iletiyoruz
        StateMachine.currentState.HandleJumpInput();
    }

    private void Update() {
        if (gameInput == null || GameManager.Instance == null) return;
        if (!GameManager.Instance.IsGamePlaying()) return;

        // Ölüm kontrolü 
        if (transform.position.y < -5f) {
            GameManager.Instance.EndGame();
            return;
        }

        StateMachine.currentState.UpdateState();
    }

    // MoveState ve AirState bu fonksiyonu çağırarak karakteri hareket ettirecek
    public void HandleMovementLogic() {
        Vector2 inputVector = gameInput.GetMovementVectorNormalized();
        Vector3 moveDir = Vector3.zero;

        if(cameraTransform != null) {
            Vector3 camForward = cameraTransform.forward;
            Vector3 camRight = cameraTransform.right;
            camForward.y = 0;
            camRight.y = 0;
            camForward.Normalize();
            camRight.Normalize();
            moveDir = (camForward * inputVector.y + camRight * inputVector.x).normalized;
        }else {
            moveDir = new Vector3(inputVector.x, 0f, inputVector.y);
        }

        transform.position += moveDir * Time.deltaTime * moveSpeed;
        
        // Moving bilgisini güncelle
        isMoving = moveDir != Vector3.zero;

        if(moveDir != Vector3.zero) {
            transform.forward = Vector3.Slerp(transform.forward, moveDir, Time.deltaTime * turnSpeed);
        }
    }

    // JumpState bu fonksiyonu çağıracak
    public void ApplyJumpForce() {
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }

    // State'ler için yer kontrolü
    public bool IsGrounded() {
        float extraHeight = 0.5f;
        Vector3 rayStartPoint = transform.position + Vector3.up * extraHeight; 
        float rayLength = 1.2f; 
        return Physics.Raycast(rayStartPoint, Vector3.down, rayLength, groundLayer);
    }

    public bool IsMoving() {
       return isMoving;
    }

    private void OnCollisionEnter(Collision collision) {
        if(collision.gameObject.CompareTag("Engel")) {
            GameManager.Instance.EndGame();
        }
    }

    private void OnDestroy() {
        if(gameInput != null) {
            gameInput.jumpEvent -= GameInput_OnJumpAction;
        }
    }
}