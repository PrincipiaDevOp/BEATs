using System.Collections;
using UnityEngine;

// This script is a modified version of the original which was provided by Vasileios Argyriou.
// The php scripts are also a modified version of the orignals also provided by Vasileios Argyriou.
// This script has been modified to meet the requirements of this project and  also extended 
// to implement a leaderboard table that can be displayed in the main menu scene.
public class DBController : MonoBehaviour
{
    // Server URLs
    string loadURL = "http://kunet.kingston.ac.uk/k1457777/ItemsData.php";
    string saveURL = "http://kunet.kingston.ac.uk/k1457777/SaveScore.php";

    public GameObject _scoresFolder;
    public GameObject _scorePrefab;
    public string[] _scores;

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

        form.AddField("newName", ScoreManager.Instance._inputName);
        form.AddField("newSong", ScoreManager.Instance._winSong.text);
        form.AddField("newScore", ScoreManager.Instance._winScore.text);

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
    }

    string GetDataValue(string data, string index)
    {
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
