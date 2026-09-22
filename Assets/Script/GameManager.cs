using UnityEngine;
using System;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {
    public static GameManager Instance{ get; private set;}
    public event EventHandler stateChanged;
    public event EventHandler pausedEvent; 
    public event EventHandler unpauseEvent;

    [SerializeField] private GameInput gameInput;
    private static bool isRestarting = false;

    private enum State {
        WaitingToStart,
        GamePlaying,
        GamePaused,
        GameOver,
    }

    private State state;

    private void Awake(){
        Instance = this;
  
        if(isRestarting) {
            state = State.GamePlaying;
        }
        else {
            state = State.WaitingToStart;
        }
    }

    private void Start() {
        gameInput.jumpEvent += GameInput_OnJumpAction;
        gameInput.pauseEvent += GameInput_OnPauseAction;

        if(isRestarting) {
            stateChanged?.Invoke(this, EventArgs.Empty);
        }
    }

    private void GameInput_OnPauseAction(object sender, EventArgs e) {
        TogglePauseGame();
    }

    public void TogglePauseGame() {
        // Sadece oyun oynarken durdurabiliriz
        if(state == State.GamePlaying) {
            state = State.GamePaused;
            Time.timeScale = 0f; // Zamanı durdur
            pausedEvent?.Invoke(this, EventArgs.Empty);
        }
        // Eğer zaten durmuşsa devam ettir
        else if(state == State.GamePaused) {
            state = State.GamePlaying;
            Time.timeScale = 1f; // Zaman devam etsin
            unpauseEvent?.Invoke(this, EventArgs.Empty);
        }
    }

    private void GameInput_OnJumpAction(object sender, EventArgs e) {

       if(this == null || transform == null) return;

        if(state == State.GameOver) {
            isRestarting = true; 
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    private void OnDestroy() {
        // GameManager yok olurken bağlantıyı kopar
        if(gameInput != null) {
            gameInput.jumpEvent -= GameInput_OnJumpAction;
        }
    }

    private void Update(){}

    public void StartGame() {
        state = State.GamePlaying;
        Time.timeScale = 1f;
        stateChanged?.Invoke(this, EventArgs.Empty);
    }

    public bool IsGamePlaying(){
    return state == State.GamePlaying;
    }

    public bool IsGameOver() {
        return state == State.GameOver;
    }

    public void EndGame() {
        if(state == State.GamePlaying) { // Sadece oyun oynanıyorsa bitir
            state = State.GameOver;
            stateChanged?.Invoke(this, EventArgs.Empty);
        }
    }

    public void RestartGame() {
        // Kayıtlı ilerleme silinmesin, son save noktasından devam etsin
        Time.timeScale = 1f;
        isRestarting = true;

        // Sahneyi yeniden yükle
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void Reset() {
        PlayerPrefs.DeleteKey("SavedFloorIndex");
        PlayerPrefs.DeleteKey("HasWrench");
        PlayerPrefs.Save();

        Time.timeScale = 1f;
        isRestarting = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

}
