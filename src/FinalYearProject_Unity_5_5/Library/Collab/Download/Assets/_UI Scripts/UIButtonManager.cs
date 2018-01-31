using UnityEngine;
using System.Collections;

public class UIButtonManager : MonoBehaviour
{
    public GameObject _menuSpectrumObjs;    // Menu spectrum objects
    public GameObject _menuButtons;         // Menu options panel
    public GameObject _songlistPanel;       // Songslist panel 
    public GameObject _leaderboardPanel;    // Leaderboard panel 

    /// <summary>
    /// To open the song list view 
    /// </summary>
    public void Play()
    {
        _songlistPanel.SetActive(true);
        _menuButtons.SetActive(false);
        _menuSpectrumObjs.SetActive(false);
    }
    /// <summary>
    /// To open the options view to configure game settings etc
    /// </summary>
    public void Leaderboard()
    {
        Debug.Log("Menu: Leaderboard button clicked.");
        _menuSpectrumObjs.SetActive(false);
        _menuButtons.SetActive(false);
        _songlistPanel.SetActive(false);
        _leaderboardPanel.SetActive(true);
    }
    /// <summary>
    /// To quit the game 
    /// </summary>
    public void Quit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
    /// <summary>
    /// To go back to the menu options view
    /// </summary>
    public void Back()
    {
        _menuSpectrumObjs.SetActive(true);
        _menuButtons.SetActive(true);
        _songlistPanel.SetActive(false);
        _leaderboardPanel.SetActive(false);
    }
    /// <summary>
    /// To continue the game by exiting the pause state and returning to playing state.
    /// </summary>
    public void Continue()
    {
        GameMaster.Instance.GameState = GameMaster.GamePlayState.PLAYING;
        EventsManager.UpdateUIView();
        AudioListener.pause = false;
    }
    /// <summary>
    /// To restart the current level
    /// </summary>
    public void Retry()
    {
        //NotesManager.Instance._container.Clear();
        NotesManager.Instance.CleanNotesFolder();
        GameSceneManager.Instance.OpenGameScene();
        AudioListener.pause = false;
    }
    /// <summary>
    /// To go back to the menu
    /// </summary>
    public void Menu()
    {
        GameSceneManager.Instance.OpenMenuScene();
        AudioListener.pause = false;
    }
}
