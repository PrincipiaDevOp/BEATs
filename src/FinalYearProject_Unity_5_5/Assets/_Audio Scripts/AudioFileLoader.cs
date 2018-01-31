using NAudio;
using NAudio.Wave;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Audio file loader.
/// - Link to the github project that contains NAudio source code used to implement this script:
/// https://github.com/naudio/NAudio
/// </summary>
public class AudioFileLoader : MonoBehaviour
{
    public static AudioFileLoader Instance { get; private set; } // Singleton Pattern Instance

    public AudioSource _audioSource;

    [System.Serializable]
    public struct SongData
    {
        public string _path;
        public string _tempDir;
        public string _fileName;
        public int index;
        public AudioClip clip;
    }

    public string _path;
    public string _tempDir;

    public List<SongData> _songsData = new List<SongData>();
    public List<AudioClip> _clips = new List<AudioClip>();
    private string[] _files;
    public int _currentSongIndex = 0;
    public int _loadingCounter = 0;
    public int _selectedSongIndex = -1;

    public Text _songTitle;
    public Text _songTime;

    public bool _readyToPlay = false;

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
    void Start()
    {
        Application.targetFrameRate = 60;
        AudioListener.pause = false;

        _audioSource = Camera.main.GetComponent<AudioSource>();

        // Search for audio files in directory and populate _songsData if files found
        InitialiseSongsData();
    }
    // Update is called once per frame
    void Update()
    {
        if (_audioSource == null)
        {
            if (GameSceneManager.Instance._newScene == "Level")
                GetAudioSource();
            else if (GameSceneManager.Instance._newScene == "Menu")
            {
                GetAudioSource();
                PlayCurrentSong(_selectedSongIndex);
            }
        }
        
        PlayAudioOnStartGame();

#region Keyboard Input Logic
        //if (Input.GetKeyDown(KeyCode.Return))
        //{
        //    if (AudioListener.pause == true)
        //    {
        //        AudioListener.pause = false;
        //        GameMaster.Instance.GameState = GameMaster.GamePlayState.PLAYING;
        //    }
        //    else if (AudioListener.pause == false)
        //    {
        //        AudioListener.pause = true;
        //        GameMaster.Instance.GameState = GameMaster.GamePlayState.PLAY_IDLE;
        //    }
        //    if (!_audioSource.isPlaying)
        //    {
        //        PlayCurrentSong(_currentSongIndex);
        //        _isPlaying = true;
        //    }
        //}
        if (Input.GetKeyDown(KeyCode.RightArrow))
            PlayNextSong();
        if (Input.GetKeyDown(KeyCode.LeftArrow))
            PlayPreviousSong();
        if (GameMaster.Instance.GameState == GameMaster.GamePlayState.PLAYING)
        {
            Debug.Log("Song Timer updating...");
            StartCoroutine(SongTimer.UpdateSongTime(_songTime));
        }

#endregion
    }
    /// <summary>
    /// Initialise all path data.
    /// </summary>
    void InitialiseSongsData()
    {
        Debug.Log("Searching through directory...");
        
#if UNITY_EDITOR
            _path = Application.dataPath + _path;
#else
            _path = Directory.GetCurrentDirectory() + _path;
#endif
        Debug.Log(_path);

        // go through the directory and search for songs
        if (Directory.Exists(_path))
        {
            // Retrieve any file names (including their paths) found in _dirPath
            _files = Directory.GetFiles(_path, "*.mp3", SearchOption.AllDirectories);
            // Print the number of .mp3 files found in directory _path
            Debug.Log(_files.Length.ToString() + " mp3's found.");
            GameMaster.Instance._songCount = _files.Length;
        }

        if (_files.Length == 0) Debug.LogError("No audio files could be found in directory. Please add music!");

        for (int i = 0; i < _files.Length; i++)
        {
            SongData _song = new SongData();
            _song._path = _files[i];
            _song._tempDir = Application.dataPath + _tempDir + Path.GetFileNameWithoutExtension(_files[i]) + ".wav";
            _song._fileName = Path.GetFileNameWithoutExtension(_files[i]);
            _song.index = i;
            if (!string.IsNullOrEmpty(_song._path))
                _songsData.Add(_song);
        }

        // Load all available clips from directory
        LoadAllClips(); // comment to stream audio data
    }
    /// <summary>
    /// Play the current song.
    /// </summary>
    public void PlayCurrentSong(int songIndex)
    {
        //StartCoroutine(LoadSong(_songsData[_currentSongIndex], true)); // Decomment if streaming audio data
        _currentSongIndex = songIndex;
        if (_clips.Count != 0)
            _audioSource.clip = _clips[_currentSongIndex];
        else
            Debug.LogError("AudioFileLoader: AudioSource.clip could not be assigned.");

        if (_audioSource.clip != null)
            _audioSource.Play();
        else
            Debug.LogError("No AudioClip attached to Camera.main.AudioSource");

        if (_songTitle != null) SongTimer.Set(_songTitle, Path.GetFileNameWithoutExtension(_songsData[_currentSongIndex]._tempDir));
        if (_songTime != null) SongTimer.SetSongTime(_songTime, _audioSource.clip.length);
        Debug.Log("Playing " + _songsData[_currentSongIndex]._tempDir);
    }
    /// <summary>
    /// Play the next song.
    /// </summary>
    void PlayNextSong()
    {
        _audioSource.Stop();
        _audioSource.timeSamples = 0;

        _currentSongIndex = (_currentSongIndex == _files.Length - 1) ? _currentSongIndex = 0 : ++_currentSongIndex;
        _audioSource.clip = _clips[_currentSongIndex];

        _audioSource.Play();
        if (_songTitle != null) SongTimer.Set(_songTitle, Path.GetFileNameWithoutExtension(_songsData[_currentSongIndex]._tempDir));
        if (_songTime != null) SongTimer.SetSongTime(_songTime, _audioSource.clip.length);

        Debug.Log("Playing " + _songsData[_currentSongIndex]._tempDir);

        //StartCoroutine(LoadSong(_songsData[_currentSongIndex], true)); // Decomment if streaming audio data
    }
    /// <summary>
    /// Play the previous song.
    /// </summary>
    void PlayPreviousSong()
    {
        _audioSource.Stop();
        _audioSource.timeSamples = 0;
        if (_currentSongIndex == 0) _currentSongIndex = _files.Length - 1;
        else
            _currentSongIndex = _currentSongIndex < 0 ? _currentSongIndex = 0 : --_currentSongIndex;

        _audioSource.clip = _clips[_currentSongIndex];

        _audioSource.Play();
        if (_songTitle != null) SongTimer.Set(_songTitle, Path.GetFileNameWithoutExtension(_songsData[_currentSongIndex]._tempDir));
        if (_songTime != null) SongTimer.SetSongTime(_songTime, _audioSource.clip.length);

        Debug.Log("Playing " + _songsData[_currentSongIndex]._tempDir);
    }
    /// <summary>
    /// Start LoadSong() coroutine for all files found in directory.
    /// </summary>
    void LoadAllClips()
    {
        for (int i = 0; i < _files.Length; ++i)
        {
            StartCoroutine(LoadSong(_songsData[i]));
        }
        Debug.Log("All LoadSong coroutines started successfuly.");
    }
    /// <summary>
    /// Convert .mp3 to .wav and load audio data for reproduction.
    /// </summary>
    /// <param name="_songObject"></param>
    /// <param name="playImmediate"></param>
    /// <returns></returns>
    IEnumerator LoadSong(SongData _songObject)
    {
        Debug.Log("Loading song");
        if (!File.Exists(_songObject._tempDir) || _songObject.clip == null)
        {
            // create the temp wav file
            Debug.Log("Converting mp3 to wav: " + _songObject._path);
            if (!Directory.Exists(Application.dataPath + _tempDir))
            {
                Directory.CreateDirectory(Application.dataPath + _tempDir);
            }
            using (Mp3FileReader reader = new Mp3FileReader(_songObject._path))
            {
                WaveFileWriter.CreateWaveFile(_songObject._tempDir, reader);
            }
            // Pass full path of song to www to load it 
            // https://docs.unity3d.com/ScriptReference/WWW.html
            WWW song = new WWW("file:///" + _songObject._tempDir);
            yield return song;

            if (!string.IsNullOrEmpty(song.error))
            {
                Debug.LogError("Could not load " + _songObject._tempDir);
            }

            // Return a stream of data downloaded previously (song)
            //AudioClip clip = song.GetAudioClip(false, true); // Decomment and comment line below to stream audio data
            AudioClip clip = song.audioClip;
            while (clip.loadState != AudioDataLoadState.Loaded && clip.loadState != AudioDataLoadState.Failed)
                yield return song;

            _songObject.clip = clip;

            _clips.Add(clip); // Decomment to use LoadAllClips()
        }

        if (_songObject.clip.loadState == AudioDataLoadState.Failed)
        {
            Debug.LogError("Failed to load.");
        }
        else
        {
            Debug.Log("Finished loading next track " + _songObject._tempDir);
            if(!_readyToPlay)
                _readyToPlay = true;
        }
        yield return null;
    }
    /// <summary>
    /// Clear the temp folder by deleting all .wav and their corresponding .meta files.
    /// </summary>
    void OnApplicationQuit()
    {
        if (_songsData == null || _songsData.Count == 0) return;

        foreach (SongData _songObject in _songsData)
        {
            if (File.Exists(_songObject._tempDir))
            {
                File.Delete(_songObject._tempDir);
                string meta = _songObject._tempDir;
                meta = meta.Replace(".mp3", ".meta");
                if (File.Exists(meta))
                    File.Delete(meta);
            }
        }

        _songsData.Clear();

        Debug.Log("Song data unloaded.");
    }
    public void GetAudioSource()
    {
        _audioSource = Camera.main.GetComponent<AudioSource>();
        Debug.Log("AudioFileLoader: Scene changed. New AudioSource referenced.");
    }
    public void PlayAudioOnStartGame()
    {
        if (_readyToPlay)
        {
            PlayCurrentSong(_currentSongIndex);
            _readyToPlay = false;
        }
        else if (!_audioSource.isPlaying && GameMaster.Instance.GameState == GameMaster.GamePlayState.MENU)
        {
            PlayNextSong();
        }
    }
}
