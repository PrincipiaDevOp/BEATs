using UnityEngine;
using System.Collections;

public class EventsManager : MonoBehaviour {

    /*
    public delegate void ShowMenuButtonsDelegate(); //define the method signature
    public static ShowMenuButtonsDelegate OnShowMenuButtons; // define the event
    */

    public delegate void GameStateChanged(GameMaster.GamePlayState gameState);
    public static GameStateChanged OnGameStateChanged;

    public delegate void GameCountdownStarted(GameMaster.GamePlayState gameState);
    public static GameCountdownStarted OnCountdownStarted;

    public static void UpdateUIView()
    {
        if(OnGameStateChanged != null)
        {
            Debug.Log("Game State changed. Updating UI view.");
            OnGameStateChanged(GameMaster.Instance.GameState);
        }
    }
    public static void GameCountdownStart()
    {
        if(OnCountdownStarted != null)
        {
            Debug.Log("Countdown started...");
            OnCountdownStarted(GameMaster.Instance.GameState);
        }
    }
}
