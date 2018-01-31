using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NotesManager : MonoBehaviour
{
    public static NotesManager Instance { get; private set; }

    public GameObject _folder;
    //[HideInInspector]
    //public List<GameObject> _container;

    // Use this for initialization
    void Awake()
    {
        #region SINGLETON PATTERN
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        #endregion
    }
    
    public void CleanNotesFolder()
    {
        for (int i = 0; i < _folder.transform.childCount; i++)
        {
            Destroy(_folder.transform.GetChild(i).gameObject);
        }
        //foreach (GameObject go in _container)
        //{
        //    //Destroy(go);
        //    _container.c
        //}
        //_container.Clear();
        print("NotesManager: List container has been cleared.");
    }

    void OnApplicationQuit()
    {
        // Empty list container when application is closed
        //_container.Clear();
    }
}
