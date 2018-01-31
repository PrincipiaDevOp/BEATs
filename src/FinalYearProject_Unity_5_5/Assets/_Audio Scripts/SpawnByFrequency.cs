using UnityEngine;
using System.Collections;
using System.Collections.Generic;
/// <summary>
/// This class controls the behaviour of GameObjects that interact with the player.
/// This script must be attached to an empty GameObject in the scene, i.e. GameManager.
/// </summary>
public class SpawnByFrequency : MonoBehaviour
{
    [SerializeField]private float _fundamentalFrequency;
    [SerializeField]private float _absoluteMaxFrequency;
    [SerializeField]private float _FFTSamples;
    [SerializeField]private float _timeBetweenSpawns;

    private Dictionary<string, int> _audioRanges;
    private Dictionary<string, Color> _rangeColors;

    // Set up references between scripts
    void Start()
    {
        _absoluteMaxFrequency = (float)GameMaster.Instance._gameSettings._spectrum._sampleRate;
        _FFTSamples = (int)GameMaster.Instance._gameSettings._spectrum._FFTSamples;
        _timeBetweenSpawns = GameMaster.Instance._gameSettings._frequencyNotes._timeBetweenSpawns;
        _audioRanges = GameMaster.Instance._gameSettings._frequencyNotes._ranges;
        _rangeColors = GameMaster.Instance._gameSettings._frequencyNotes._colors;
    }
    void OnEnable()
    {
        EventsManager.OnGameStateChanged += StartSpawning;
    }
    void OnDisable()
    {
        StopSpawning();
        EventsManager.OnGameStateChanged -= StartSpawning;
    }
    void StartSpawning(GameMaster.GamePlayState gameState)
    {
        if (gameState == GameMaster.GamePlayState.PLAYING)
        {
            Debug.Log("Spawning notes.");
            InvokeRepeating("SpawnNotes", 1f, _timeBetweenSpawns); 
        }
        else
        {
            StopSpawning();
        }
    }

    void StopSpawning()
    {
        Debug.Log("Stopped spawning notes.");
        CancelInvoke();
    }
    /// <summary>
    /// Spawns a GameObject with an _EmissionColor based on a fundamental frequency and adds it to a List of GameObjects.
    /// </summary>
    /// <returns></returns>
    /*
    public IEnumerator SpawnNotes()
    {
        Debug.Log("SpawnByFrequency: IEnumerator.SpawnNotes()");
        _timeSinceLastSpawn += Time.deltaTime;

        if (_timeSinceLastSpawn >= _timeBetweenSpawns)
        {
            float _theta = Mathf.Floor(Random.Range(0, 360));
            _fundamentalFrequency = Mathf.Floor(GetFundamentalFrequency());
            Vector3 pos = new Vector3(Mathf.Cos(_theta) * Random.Range(5, 10), 5f, Mathf.Sin(_theta) * Random.Range(5, 10));

            GameObject newObject = (GameObject)Instantiate(GameMaster.Instance._gameSettings._frequencyNotes._frequencyNotePrefab, pos, Quaternion.identity);
            newObject.GetComponent<Explosion>()._scoreValue = SetScoreValueByFrequency(_fundamentalFrequency);
            newObject.name = "Note 1: " + _fundamentalFrequency.ToString();
            newObject.GetComponent<MeshRenderer>().materials[0].SetColor("_EmissionColor", SetColorByFrequencyRange(_fundamentalFrequency));

            newObject.transform.SetParent(_folder.transform);
            _timeSinceLastSpawn = 0.0f;
        }

        yield return new WaitForSeconds(1 / (1 / _timeBetweenSpawns));
    }
    */
    void SpawnNotes()
    {
        float _theta = Mathf.Floor(Random.Range(0, 360));
        _fundamentalFrequency = Mathf.Floor(GetFundamentalFrequency());
        Vector3 pos = new Vector3(Mathf.Cos(_theta) * Random.Range(5, 10), 5f, Mathf.Sin(_theta) * Random.Range(5, 10));

        GameObject newObject = (GameObject)Instantiate(GameMaster.Instance._gameSettings._frequencyNotes._frequencyNotePrefab, pos, Quaternion.identity);
        newObject.GetComponent<MeshRenderer>().materials[0].SetColor("_EmissionColor", SetColorByFrequencyRange(_fundamentalFrequency));
        newObject.GetComponent<Explosion>()._scoreValue = SetScoreValueByFrequency(_fundamentalFrequency);

        newObject.transform.SetParent(NotesManager.Instance._folder.transform);
        newObject.name = "Note 1: " + _fundamentalFrequency.ToString();
    }

    /// <summary>
    /// Returns the fundamental frequency of the strongest frequency in the FFTSamples.
    /// </summary>
    /// <returns></returns>
    float GetFundamentalFrequency()
    {
        float _index = 0;
        float _strongest = 0f;

        for (int i = 0; i < SpectrumAnalyser.Instance._spectrumLeftChannel.Length; i++)
        {
            // Find strongest signal
            if (_strongest < SpectrumAnalyser.Instance._spectrumLeftChannel[i])
            {
                _strongest = SpectrumAnalyser.Instance._spectrumLeftChannel[i];
                _index = i;
            }
        }
        float _fFreq = _index * _absoluteMaxFrequency / _FFTSamples;

        return _fFreq;
    }
    /// <summary>
    /// Returns a color based on the fundamental frequency fFreq.
    /// </summary>
    /// <param name="_fFreq"></param>
    /// <returns></returns>
    Color SetColorByFrequencyRange(float _fFreq)
    {
        if (_fFreq <= _audioRanges["Subbass"])
        {
            //Debug.Log("Subbass.Red");
            return _rangeColors["Red"];
        }
        else if (_fFreq > _audioRanges["Subbass"] && _fFreq <= _audioRanges["Bass"])
        {
            //Debug.Log("Bass.Green");
            return _rangeColors["Green"];
        }
        else if (_fFreq > _audioRanges["Bass"] && _fFreq <= _audioRanges["Midrange"])
        {
            //Debug.Log("Midrange.Blue");
            return _rangeColors["Blue"];
        }
        else if (_fFreq > _audioRanges["Midrange"] && _fFreq <= _audioRanges["Highmids"])
        {
            //Debug.Log("Highmids.Yellow");
            return _rangeColors["Yellow"];
        }
        else if (_fFreq > _audioRanges["Highmids"] && _fFreq <= _audioRanges["Highfrequency"])
        {
            //Debug.Log("Highfrequency.Purple");
            return _rangeColors["Purple"];
        }
        else
        {
            Debug.LogError("No color within range could be assigned.");
            return Color.white;
        }
    }
    /// <summary>
    /// Set the score value of the instantiated note using its frequency.
    /// </summary>
    /// <param name="_fFreq"></param>
    /// <returns></returns>
    int SetScoreValueByFrequency(float _fFreq)
    {
        if (_fFreq <= _audioRanges["Subbass"])
        {
            //Debug.Log("Subbass.Red");
            return 100;
        }
        else if (_fFreq > _audioRanges["Subbass"] && _fFreq <= _audioRanges["Bass"])
        {
            //Debug.Log("Bass.Green");
            return 150;
        }
        else if (_fFreq > _audioRanges["Bass"] && _fFreq <= _audioRanges["Midrange"])
        {
            //Debug.Log("Midrange.Blue");
            return 200;
        }
        else if (_fFreq > _audioRanges["Midrange"] && _fFreq <= _audioRanges["Highmids"])
        {
            //Debug.Log("Highmids.Yellow");
            return 250;
        }
        else if (_fFreq > _audioRanges["Highmids"] && _fFreq <= _audioRanges["Highfrequency"])
        {
            //Debug.Log("Highfrequency.Purple");
            return 300;
        }
        else
        {
            Debug.Log("No score value within range could be assigned.");
            return 350;
        }
    }
}
