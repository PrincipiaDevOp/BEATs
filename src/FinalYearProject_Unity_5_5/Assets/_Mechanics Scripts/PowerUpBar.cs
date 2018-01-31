using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PowerUpBar : MonoBehaviour
{
    public Image _loadingBar;

    public float _resetTime;
    public float _currentResetTime;
    void Start()
    {
        _resetTime = GameMaster.Instance._gameSettings._powerUpSettings._powerUpTime;
        _currentResetTime = _resetTime;
    }
    void Update()
    {
        if (GameMaster.Instance.GameState == GameMaster.GamePlayState.PLAYING)
        {
            if (GameMaster.Instance._gameSettings._powerUpSettings._isPowerUp)
                ResetPowerUpBar();
        }
        
    }
    public void SetFillAmount(float amount)
    {
        _loadingBar.fillAmount = ScoreManager.Instance._combo / 100.0f;
    }
    public void ResetPowerUpBar()
    {
        if (_currentResetTime > 0)
        {
            _currentResetTime -= Time.deltaTime;
            _loadingBar.fillAmount = (_currentResetTime / _resetTime);
        }
        else
        {
            GameMaster.Instance._gameSettings._powerUpSettings._isPowerUp = false;
            GameMaster.Instance._gameSettings._frequencyNotes._timeBetweenSpawns = 1f;
            _currentResetTime = _resetTime;
        }
    }
}
