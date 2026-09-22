using UnityEngine;
using System;
using UnityEngine.InputSystem;

public class GameInput : MonoBehaviour {
    public event EventHandler jumpEvent;
    public event EventHandler pauseEvent;
    public event EventHandler attackEvent;

    private InputSystem_Actions inputActions; 

    public void Awake() {
        inputActions = new InputSystem_Actions();
        
        // Inputları aktif et
        inputActions.Player.Enable();

        // Tuşlara fonksiyonları bağla
        inputActions.Player.Jump.performed += Jump_performed;
        inputActions.Player.Pause.performed += Pause_performed;
        inputActions.Player.Attack.performed += Attack_performed;
    }

    private void OnDestroy() {
        if(inputActions == null) return;

        // Bağlantıları kopar
        inputActions.Player.Jump.performed -= Jump_performed;
        inputActions.Player.Pause.performed -= Pause_performed;
        inputActions.Player.Attack.performed -= Attack_performed;
        
        inputActions.Player.Disable();
        inputActions.Dispose();
    }

    private void Attack_performed(InputAction.CallbackContext obj) {
        attackEvent?.Invoke(this, EventArgs.Empty);
    }

    private void Pause_performed(InputAction.CallbackContext obj) {
        pauseEvent?.Invoke(this, EventArgs.Empty);
    }

    private void Jump_performed(InputAction.CallbackContext obj) {
        jumpEvent?.Invoke(this, EventArgs.Empty);
    }

    public Vector2 GetMovementVectorNormalized() { 
        Vector2 inputVector = inputActions.Player.Move.ReadValue<Vector2>();
        return inputVector.normalized;
    }
}