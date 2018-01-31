using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class MenuSpawner : MonoBehaviour
{
    public static MenuSpawner _instance;
    public List<GameObject> _menuSpectrumObjs;
    public GameObject _spectrumObjsFolder;
    public GameObject _menuSpectrumObjPrefab;
    public float _radius;
    public int _amount;

    public Button _songButtonPrefab;
    public GameObject _songButtonParent;

    // Use this for initialization
    void Start()
    {
        // Instantiate menu objects
        SpawnMenuSpectrumObjects();
        Invoke("SpawnSongScrollList", 1f);
    }
    void SpawnMenuSpectrumObjects()
    {
        _menuSpectrumObjs = MathA.SpawnMenuObjects(_menuSpectrumObjPrefab, _radius, _amount);

        foreach (var obj in _menuSpectrumObjs)
        {
            obj.transform.SetParent(_spectrumObjsFolder.transform);
        }
    }

    public void SpawnSongScrollList()
    {
        for (int i = 0; i < GameMaster.Instance._songCount; i++)
        {
            Button button = Instantiate(_songButtonPrefab) as Button;
            button.transform.SetParent(_songButtonParent.transform);
            button.GetComponentInChildren<Text>().text = AudioFileLoader.Instance._songsData[i]._fileName;
            button.GetComponent<ScrollListElementInfo>()._songID = AudioFileLoader.Instance._songsData[i].index;
            button.GetComponent<ScrollListElementInfo>()._songName = AudioFileLoader.Instance._songsData[i]._fileName;
            button.onClick.AddListener(() => { GameSceneManager.Instance.OpenGameScene(); });
            //button.onClick.AddListener(() => { AudioFileLoader.Instance.PlayCurrentSong(button.GetComponent<ScrollListElementInfo>()._songID); });
            button.onClick.AddListener(() => { AudioFileLoader.Instance._selectedSongIndex = button.GetComponent<ScrollListElementInfo>()._songID; });
        }
    }

    void OnApplicationQuit()
    {
        _menuSpectrumObjs.Clear();
    }
}
