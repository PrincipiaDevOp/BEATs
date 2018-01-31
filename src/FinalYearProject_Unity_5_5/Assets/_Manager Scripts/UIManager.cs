using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{

    public GameObject _menuCanvas;
    public GameObject _levelCanvas;
    public GameObject _pausePanel;
    public GameObject _countdownPanel;
    public GameObject _winPanel;
    public GameObject _losePanel;
    
    void OnEnable()
    {
        EventsManager.OnGameStateChanged += ToggleUIObjects;
    }
    public void ToggleUIObjects(GameMaster.GamePlayState gameState)
    {
        switch (gameState)
        {
            case GameMaster.GamePlayState.MENU:
                _menuCanvas.SetActive(true);
                _levelCanvas.SetActive(false);
                _pausePanel.SetActive(false);
                _countdownPanel.SetActive(false);
                _winPanel.SetActive(false);
                _losePanel.SetActive(false);
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                break;
            case GameMaster.GamePlayState.COUNTDOWN:
                _menuCanvas.SetActive(false);
                _levelCanvas.SetActive(true);
                _countdownPanel.SetActive(true);
                _pausePanel.SetActive(false);
                _winPanel.SetActive(false);
                _losePanel.SetActive(false);
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                break;
            case GameMaster.GamePlayState.PLAYING:
                _menuCanvas.SetActive(false);
                _levelCanvas.SetActive(true);
                _pausePanel.SetActive(false);
                _countdownPanel.SetActive(false);
                _winPanel.SetActive(false);
                _losePanel.SetActive(false);
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                break;
            case GameMaster.GamePlayState.PAUSE:
                _menuCanvas.SetActive(false);
                _levelCanvas.SetActive(true);
                _pausePanel.SetActive(true);
                _countdownPanel.SetActive(false);
                _winPanel.SetActive(false);
                _losePanel.SetActive(false);
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                break;
            case GameMaster.GamePlayState.WIN:
                _menuCanvas.SetActive(false);
                _levelCanvas.SetActive(true);
                _pausePanel.SetActive(false);
                _countdownPanel.SetActive(false);
                _winPanel.SetActive(true);
                _losePanel.SetActive(false);
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                break;
            case GameMaster.GamePlayState.LOSE:
                _menuCanvas.SetActive(false);
                _levelCanvas.SetActive(true);
                _pausePanel.SetActive(false);
                _countdownPanel.SetActive(false);
                _winPanel.SetActive(false);
                _losePanel.SetActive(true);
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                break;
        }
    }
}
