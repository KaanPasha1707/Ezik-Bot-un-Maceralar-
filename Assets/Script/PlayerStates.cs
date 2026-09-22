using UnityEngine;

public class PlayerIdleState : PlayerBaseState {
    public PlayerIdleState(Player player, PlayerStateMachine stateMachine) : base(player, stateMachine) { }

    public override void EnterState() { }

    public override void ExitState() { }

    public override void UpdateState()
    {
        // Eğer yere basmıyorsak düşüyoruz demektir -> AirState
        if(!player.IsGrounded()) {
            stateMachine.ChangeState(player.AirState);
            return;
        }

        // Eğer hareket girdisi varsa -> MoveState
        if(player.GameInput.GetMovementVectorNormalized() != Vector2.zero) {
            stateMachine.ChangeState(player.MoveState);
        }
    }
 
    public override void HandleJumpInput() {
        // Dururken zıplayabilir
        stateMachine.ChangeState(player.JumpState);
    }
}

public class PlayerMoveState : PlayerBaseState {
    public PlayerMoveState(Player player, PlayerStateMachine stateMachine) : base(player, stateMachine) { }

    public override void EnterState() { }
    public override void ExitState() { }

    public override void UpdateState() {
        // Hareket mantığını Player class'ındaki fonksiyondan çağırıyoruz
        player.HandleMovementLogic();

        // Hareket tuşunu bıraktıysa -> IdleState
        if(player.GameInput.GetMovementVectorNormalized() == Vector2.zero) {
            stateMachine.ChangeState(player.IdleState);
        }
        
        // Yürürken boşluğa düştüyse -> AirState
        if(!player.IsGrounded()) {
            stateMachine.ChangeState(player.AirState);
        }
    }

    public override void HandleJumpInput() {
        // Yürürken zıplayabilir
        stateMachine.ChangeState(player.JumpState);
    }
}

public class PlayerJumpState : PlayerBaseState {
    public PlayerJumpState(Player player, PlayerStateMachine stateMachine) : base(player, stateMachine) { }

    public override void EnterState() {
        player.IsJumping = true; 
        player.ApplyJumpForce();
        stateMachine.ChangeState(player.AirState);
    }

    public override void UpdateState() { }
    public override void ExitState() { }
}

public class PlayerAirState : PlayerBaseState {
    public PlayerAirState(Player player, PlayerStateMachine stateMachine) : base(player, stateMachine) { }

    public override void EnterState() {
        if(player.IsJumping) {
            player.SecondJumpTimeCounter = 0; // Zıpladıysak ek süre yok
            player.IsJumping = false;     
        }else {
            // Zıplamadık, demek ki yürürken düştük -> Süreyi başlat
            player.SecondJumpTimeCounter = player.SecondJumpDuration;
        }
    }
    public override void ExitState() { }

    public override void UpdateState() {
        // Sayacı geriye doğru saydır
        player.SecondJumpTimeCounter -= Time.deltaTime;

        player.HandleMovementLogic(); 

        if(player.IsGrounded() && player.rb.linearVelocity.y <= 0.1f) {
             if(player.GameInput.GetMovementVectorNormalized() != Vector2.zero)
                stateMachine.ChangeState(player.MoveState);
             else stateMachine.ChangeState(player.IdleState);
        }
    }

    public override void HandleJumpInput() {
        // Eğer hala süremiz varsa zıplamaya izin ver
        if (player.SecondJumpTimeCounter > 0){
            stateMachine.ChangeState(player.JumpState);
        }
    }
}