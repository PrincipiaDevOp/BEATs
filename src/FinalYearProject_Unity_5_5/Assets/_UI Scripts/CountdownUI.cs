using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CountdownUI : MonoBehaviour
{

    public float _startTime = 5.0f;
    public float _currentTime = 0.0f;
    public Text _countdownText;

    void Start()
    {
        _currentTime = _startTime;
    }
    void OnEnable()
    {
        EventsManager.OnCountdownStarted += StartCountdown;
    }
    public void StartCountdown(GameMaster.GamePlayState gameState)
    {
        if (gameState == GameMaster.GamePlayState.COUNTDOWN)
        {
            _currentTime -= Time.deltaTime;
            if (_currentTime > 0)
            {
                _countdownText.text = ((int)_currentTime).ToString();
            }
            else
            {
                _currentTime = _startTime;
                GameMaster.Instance.GameState = GameMaster.GamePlayState.PLAYING;
                EventsManager.UpdateUIView();
                PopUpTextController.Initialize();
                AudioFileLoader.Instance.PlayCurrentSong(AudioFileLoader.Instance._selectedSongIndex);
            }
        }
    }
}
