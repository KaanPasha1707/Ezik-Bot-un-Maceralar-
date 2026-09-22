using UnityEngine;
using TMPro;

public class GameUIManager : MonoBehaviour {
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private GameObject mainMenuPanel;

    private void Start() {
       GameManager.Instance.stateChanged += GameManager_OnStateChanged;
       GameManager.Instance.pausedEvent += GameManager_OnGamePaused;
       GameManager.Instance.unpauseEvent += GameManager_OnGameUnpaused;
        
        // Oyun oynanıyor mu? 
        if(GameManager.Instance.IsGamePlaying()) {
            HideAll(); // Evet oynanıyor, o zaman menüleri gizle
        }
        else {
            ShowMainMenu(); // Hayır, ilk kez çalıştı, menüyü göster
        }
    }

    private void GameManager_OnGamePaused(object sender, System.EventArgs e) {
        ShowMainMenu(); 
    }

    private void GameManager_OnGameUnpaused(object sender, System.EventArgs e) {
        HideAll();
    }

    private void GameManager_OnStateChanged(object sender, System.EventArgs e) {
       // Oyun başladıysa Menüyü kapat
        if(GameManager.Instance.IsGamePlaying()) {
            HideAll();
        }
        // Oyun bittiyse Game Over'ı aç
        else if(GameManager.Instance.IsGameOver()) {
            ShowGameOver();
        }
    }

    private void ShowMainMenu() {
        mainMenuPanel.SetActive(true);
        gameOverPanel.SetActive(false);
    }

    private void ShowGameOver() {
        mainMenuPanel.SetActive(false);
        gameOverPanel.SetActive(true);
    }

    private void HideAll() {
        mainMenuPanel.SetActive(false);
        gameOverPanel.SetActive(false);
    }
}