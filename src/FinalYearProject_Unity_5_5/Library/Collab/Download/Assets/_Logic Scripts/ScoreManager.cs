using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance { get; private set; }

    public int _score;  // score counter
    public int _combo;  // number of consecutibe notes hit
    public int _shots;  // number of shots fired
    public int _hits;   // number of notes hit
    public int _misses; // number of notes missed

    // Level screen
    public ScoreBar _scoreBar;
    public Text _scoreText;

    // Win screen 
    public GameObject _winPanel;
    public Text _winSong;
    public Text _winScore;
    public Text _winHits;
    public Text _winMisses;
    public Text _winShots;
    public Text _winName;

    // Use this for initialization
    void Awake()
    {
        #region SINGLETON PATTERN
        // Check if there are any other instances conflicting
        if (Instance != null && Instance != this)
        {
            // if that's the case, we destroy other instances
            Destroy(gameObject);
        }
        // Save singleton instance
        Instance = this;
        // Don't destroy this intance between scenes
        DontDestroyOnLoad(gameObject);
        #endregion
    }
    void OnEnable()
    {
        EventsManager.OnGameStateChanged += OnWinGame;
    }
    public void OnWinGame(GameMaster.GamePlayState gameState)
    {
        if (gameState == GameMaster.GamePlayState.WIN)
        {
            _winSong.text = AudioFileLoader.Instance._songsData[AudioFileLoader.Instance._selectedSongIndex]._fileName;
            _winScore.text = _score.ToString();
            _winHits.text = _hits.ToString();
            _winMisses.text = _misses.ToString();
            _winShots.text = _shots.ToString();
        }
    }
    /// <summary>
    /// Increment current score by amount.
    /// </summary>
    /// <param name="amount"></param>
    public void AddScore(int amount)
    {
        _score += amount;
        _scoreText.text = "" + _score;
        _scoreBar.Increase(amount);
    }
    /// <summary>
    /// Decrement current score by amount.
    /// </summary>
    /// <param name="amount"></param>
    public void SubScore(int amount)
    {
        _score -= amount;
        _scoreText.text = "" + _score;
        _scoreBar.Decrease(amount);
    }
    /// <summary>
    /// Reset ScoreManager counter variables.
    /// </summary>
    public void ResetScore()
    {
        _score = 0;
        _hits = 0;
        _misses = 0;
        _shots = 0;
        _scoreText.text = _score.ToString();
    }
}
