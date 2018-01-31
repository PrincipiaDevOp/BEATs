using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HighScoreScript : MonoBehaviour
{
    public GameObject _rank;
    public GameObject _name;
    public GameObject _song;
    public GameObject _score;

    public void SetHighScore(string rank, string name, string song, string score)
    {
        this._rank.GetComponent<Text>().text = rank;
        this._name.GetComponent<Text>().text = name;
        this._song.GetComponent<Text>().text = song;
        this._score.GetComponent<Text>().text = score;
    }
}