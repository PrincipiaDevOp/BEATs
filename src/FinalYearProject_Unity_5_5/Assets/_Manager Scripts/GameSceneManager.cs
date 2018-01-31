using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameSceneManager : MonoBehaviour
{
    // Singleton Instance
    public static GameSceneManager Instance { get; private set; }
    public string _newScene = "Menu";

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
        //SceneManager.sceneUnloaded += SceneManager_sceneUnloaded;
        //SceneManager.sceneLoaded += SceneManager_sceneLoaded;
        SceneManager.activeSceneChanged += SceneManager_activeSceneChanged;
    }
    void OnDisable()
    {
        SceneManager.activeSceneChanged -= SceneManager_activeSceneChanged;
    }
    public void OpenGameScene()
    {
        Debug.Log("Opening Level Scene...");
        GameMaster.Instance.GameState = GameMaster.GamePlayState.COUNTDOWN;
        EventsManager.UpdateUIView();
        SceneManager.LoadScene("Level"); 
    }
    public void OpenMenuScene()
    {
        Debug.Log("Opening Menu Scene...");
        GameMaster.Instance.GameState = GameMaster.GamePlayState.MENU;
        EventsManager.UpdateUIView();
        SceneManager.LoadScene("Menu");
        // Reset Score
        ScoreManager.Instance.Reset();
    }

    //private void SceneManager_sceneLoaded(Scene arg0, LoadSceneMode arg1)
    //{
    //    if (arg0.name == "Menu" || arg0.name == "Level")
    //    {
    //        AudioFileLoader.Instance.GetAudioSource();
    //        SpectrumAnalyser.Instance.GetAudioSource();
    //        Debug.Log("SceneManager_sceneLoaded: " + arg0.name);
    //    }
    //}

    //private void SceneManager_sceneUnloaded(Scene arg0)
    //{
    //    if (arg0.name == "Menu" || arg0.name == "Level")
    //    {
    //        AudioFileLoader.Instance.GetAudioSource();
    //        SpectrumAnalyser.Instance.GetAudioSource();
    //        Debug.Log("SceneManager_sceneUnloaded: " + arg0.name);
    //    }
    //}

    private void SceneManager_activeSceneChanged(Scene arg0, Scene arg1)
    {
        arg1 = SceneManager.GetActiveScene();
        //print(arg0.name + "/" + arg1.name);

        if (arg1.name == "Level")
        {
            _newScene = "Level";
            Debug.Log("SceneManager_activeSceneChanged: " + arg1.name);

        }
        else if (arg1.name == "Menu")
        {
            _newScene = "Menu";
            Debug.Log("SceneManager_activeSceneChanged: " + arg1.name);
        }
    }
}
