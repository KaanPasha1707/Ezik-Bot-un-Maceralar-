using UnityEngine;

public abstract class PlayerBaseState {
    protected Player player; // Karakterin ana koduna erişmek için referans
    protected PlayerStateMachine stateMachine; // Durum değiştirmek için referans

    public PlayerBaseState(Player player, PlayerStateMachine stateMachine) {
        this.player = player;
        this.stateMachine = stateMachine;
    }

    public abstract void EnterState(); // Bu duruma girince 1 kere çalışır
    public abstract void UpdateState(); // Bu durumdayken her frame çalışır
    public abstract void ExitState(); // Bu durumdan çıkarken 1 kere çalışır
    
    // Zıplama tuşuna basıldığında ne olacağına her durum kendi karar verecek
    public virtual void HandleJumpInput() { } 
}