using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SongTimer : MonoBehaviour
{
    public static int min, sec;
    public static float _songLength;
    public static float _timeBetweenUpdates = 1.0f, _timeSinceLastUpdate = 0.0f;

    public static void Set(Text _textObj, string _txt)
    {
        _textObj.text = _txt;
    }

    public static void SetSongTime(Text _textObj, float _clipLength)
    {
        _songLength = _clipLength;

        float dummy = _clipLength / 60;
        min = Mathf.FloorToInt(dummy);
        sec = Mathf.RoundToInt((dummy - min) * 60);
        string minsStr, secStr;

        minsStr = "0" + min.ToString();
        if (sec <= 9) secStr = ":0" + sec.ToString();
        else secStr = ":" + sec.ToString();

        _textObj.text = minsStr + secStr;
    }

    public static IEnumerator UpdateSongTime(Text _textObj)
    {
        _timeSinceLastUpdate += Time.deltaTime;
        if (_timeSinceLastUpdate >= _timeBetweenUpdates)
        {
            _songLength -= _timeSinceLastUpdate;
            float dummy = _songLength / 60;
            min = Mathf.FloorToInt(dummy);
            sec = Mathf.RoundToInt((dummy - min) * 60);
            string minsStr, secStr;

            minsStr = "0" + min.ToString();
            if (sec <= 9) secStr = ":0" + sec.ToString();
            else secStr = ":" + sec.ToString();

            _textObj.text = minsStr + secStr;

            _timeSinceLastUpdate = 0.0f;
        }

        yield return new WaitForSeconds(1 / (1 / _timeBetweenUpdates));
    }
}
