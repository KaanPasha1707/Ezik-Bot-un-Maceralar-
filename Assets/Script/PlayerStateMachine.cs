public class PlayerStateMachine {
    public PlayerBaseState currentState { get; private set; }

    public void Initialize(PlayerBaseState startingState) {
        currentState = startingState;
        currentState.EnterState();
    }

    public void ChangeState(PlayerBaseState newState) {
        currentState.ExitState(); // Eski durumdan çık
        currentState = newState;  // Yeni duruma geç
        currentState.EnterState(); // Yeni duruma gir
    }
}