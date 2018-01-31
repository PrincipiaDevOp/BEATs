using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameMaster : MonoBehaviour
{
    public static GameMaster Instance { get; private set; } // Singleton Instance
    // Enums
    public enum GamePlayState { MENU, COUNTDOWN, PLAYING, PAUSE, WIN, LOSE }
    public enum GamePlayMode { EASY, MEDIUM, HARD }
    [SerializeField]
    private GamePlayState _gamePlayState = GamePlayState.MENU;
    [SerializeField]
    private GamePlayMode _gamePlayMode = GamePlayMode.EASY;
    // Getters & Setters
    public GamePlayState GameState
    {
        get { return _gamePlayState; }
        set { _gamePlayState = value; }
    }
    public GamePlayMode GameMode
    {
        get { return _gamePlayMode; }
        set { _gamePlayMode = value; }
    }

    public GameProperties _gameSettings;

    [HideInInspector] public int _songCount = 0;
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
    void Start()
    {
        // initialise game settings 
        _gameSettings._spectrum.Reset();
        _gameSettings._visualiser.Reset();
        _gameSettings._frequencyNotes.Init_Dictionaries();
    }

    void Update()
    {
        if (GameMaster.Instance.GameState == GameMaster.GamePlayState.PLAYING && Input.GetKeyDown(KeyCode.Escape))
        {
            GameMaster.Instance.GameState = GameMaster.GamePlayState.PAUSE;
            AudioListener.pause = !AudioListener.pause;
            EventsManager.UpdateUIView();
        }
        if (GameMaster.Instance.GameState == GameMaster.GamePlayState.COUNTDOWN)
            EventsManager.GameCountdownStart();

        if (GameMaster.Instance.GameState == GameMaster.GamePlayState.PLAYING && !Camera.main.GetComponent<AudioSource>().isPlaying)
        {
            GameMaster.Instance.GameState = GameMaster.GamePlayState.WIN;
            EventsManager.UpdateUIView();
        }
        if(ScoreManager.Instance._combo >= _gameSettings._powerUpSettings._powerUpThreshold)
        {
            // Increase the spawning rate of notes
            _gameSettings._frequencyNotes._timeBetweenSpawns = 0.1f;
            // Enable the particle gun by setting isPowerUp to true
            _gameSettings._powerUpSettings._isPowerUp = true;
            // Reset the combo counter
            ScoreManager.Instance._combo = 0;
        }
    }
}
