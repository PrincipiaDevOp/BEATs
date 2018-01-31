using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DBController : MonoBehaviour
{
    // Singleton Instance
    public static DBController Instance { get; private set; }
    // Save variables
    //public int _score = 0;
    //public int _hits = 0;
    //public int _misses = 0;
    //public int _shots = 0;
    //public string _name = "Anonymus";
    //public string _song = "Unknown";
    // Server URLs
    string loadURL = "http://kunet.kingston.ac.uk/k1457777/ItemsData.php";
    string saveURL = "http://kunet.kingston.ac.uk/k1457777/SaveScore.php";

    public GameObject _scoresFolder;
    public GameObject _scorePrefab;
    public string[] _scores;

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
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
            SaveScore();
        if (Input.GetKeyDown(KeyCode.L))
            LoadScores();
    }

    public void SaveScore()
    {
        StartCoroutine(CSaveScore());
    }
    public void LoadScores()
    {
        StartCoroutine(CLoadScores());
    }

    IEnumerator CSaveScore()
    {
        WWWForm form = new WWWForm();

        form.AddField("newName", ScoreManager.Instance._winName.text);
        form.AddField("newSong", ScoreManager.Instance._winSong.text);
        form.AddField("newScore", ScoreManager.Instance._winScore.text);

        //form.AddField("newName", _name.ToString());
        //form.AddField("newSong", _song.ToString());
        //form.AddField("newScore", _score.ToString());

        //form.AddField("newName", _name.ToString());
        //form.AddField("newSong", _song.ToString());
        //form.AddField("newScore", _score.ToString());

        WWW webRequest = new WWW(saveURL, form);

        Debug.Log(saveURL);

        yield return webRequest;

        if (!string.IsNullOrEmpty(webRequest.error))
        {
            print("Error: " + webRequest.error);
        }
        else
        {
            Debug.Log(webRequest.text.ToString());
        }
    }


    IEnumerator CLoadScores()
    {
        WWW webRequest = new WWW(loadURL);
        yield return webRequest;
        //_scoresFolder.GetComponent<Text>().text = webRequest.text;
        string itemDataString = webRequest.text;
        print(itemDataString);
        _scores = itemDataString.Split(';');

        for (int i = 0; i < _scores.Length - 1; i++)
        {
            // Instantiate prefab 
            GameObject score = Instantiate(_scorePrefab) as GameObject;

            string tempName = GetDataValue(_scores[i], "Name: ");
            string tempSong = GetDataValue(_scores[i], "Song: ");
            string tempScore = GetDataValue(_scores[i], "Score: ");

            // Set highscore values
            score.GetComponent<HighScoreScript>().SetHighScore((i + 1).ToString(), tempName, tempSong, tempScore);

            // Set parent transform 
            score.transform.SetParent(_scoresFolder.transform);
        }
        //print(GetDataValue(_scores[0], "Name: "));
    }

    string GetDataValue(string data, string index)
    {
        print(data.IndexOf(index));
        print("index.length: " + index.Length);
        string value = data.Substring(data.IndexOf(index) + index.Length);
        if (value.Contains("|"))
            value = value.Remove(value.IndexOf("|"));

        return value;
    }

    public void ClearScoresFolder()
    {
        for (int i = 0; i < _scoresFolder.transform.childCount; i++)
        {
            Destroy(_scoresFolder.transform.GetChild(i).gameObject);
        }
    }
}
