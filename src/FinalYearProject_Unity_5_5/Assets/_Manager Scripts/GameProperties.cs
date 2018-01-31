using UnityEngine;
using System.Collections;
using System.Collections.Generic;
/// <summary>
/// This script contains game settings data structures used by the GameMaster script.
/// This script does not need to be attached to a GameObject.
/// </summary>

// Enums
public enum Shapes { WALL, SEMICIRCLE, CIRCLE };
public enum PrefabTypes { CYLINDER, BOX, COLUMN };
public enum FFTSamples { _512 = 512, _1024 = 1024, _8192 = 8192 };
public enum SampleRates { _11025Hz = 11025, _22050Hz = 22050, _44100Hz = 44100 }
public enum Channels { ALL, LEFT, RIGHT, };
// Dictionaries

// Structs
[System.Serializable]
public struct GameProperties
{
    public SpectrumSettings _spectrum;
    public PrefabSettings _prefabs;
    public VisualiserSettings _visualiser;
    public FrequencyNoteSettings _frequencyNotes;
    public PowerUpSettings _powerUpSettings;
}
[System.Serializable]
public struct SpectrumSettings
{
    public FFTWindow _FFTWindow;
    public FFTSamples _FFTSamples;
    public SampleRates _sampleRate;
    public Channels _channels;

    public void Reset()
    {
        _FFTWindow = FFTWindow.BlackmanHarris;
        _FFTSamples = FFTSamples._8192;
        _sampleRate = SampleRates._44100Hz;
        _channels = Channels.ALL;
    }
}
[System.Serializable]
public struct PrefabSettings
{
    public GameObject _cylinderPrefab;
    public GameObject _boxPrefab;
    public GameObject _columnPrefab;
}
[System.Serializable]
public struct VisualiserSettings
{
    public PrefabTypes _type;
    public Shapes _shape;

    public int _amount;
    [Range(10, 100)]
    public float _radius;
    [Range(10, 200)]
    public float _sensitivity;
    [Range(1, 10)]
    public float _speed;

    public void Reset()
    {
        _type = PrefabTypes.CYLINDER;
        _shape = Shapes.CIRCLE;
        _amount = 200; 
        _radius = 60;
        _sensitivity = 40;
        _speed = 5;
    }
}

[System.Serializable]
public struct FrequencyNoteSettings
{
    public GameObject _frequencyNotePrefab;
    public float _renderDelayTime;
    public float _timeBetweenSpawns;
    public float _noteLifeTime;
    public Dictionary<string, int> _ranges;
    public Dictionary<string, Color> _colors;

    public void Init_Dictionaries()
    {
        _ranges = new Dictionary<string, int>();
        _colors = new Dictionary<string, Color>();

        _ranges.Add("Subbass", 60); // Sub-bass
        _ranges.Add("Bass", 250); // Bass
        _ranges.Add("Midrange", 4000); // Midrange
        _ranges.Add("Highmids", 6000); // High Mids
        _ranges.Add("Highfrequency", 20000); // High Frequency

        _colors.Add("Red", Color.red);
        _colors.Add("Green", Color.green);
        _colors.Add("Blue", Color.blue);
        _colors.Add("Yellow", Color.yellow);
        _colors.Add("Purple", new Color(0.62f, 0.12f, 0.94f));
    }
}
[System.Serializable]
public struct PowerUpSettings
{
    public bool _isPowerUp;
    public int _powerUpTime;
    public int _powerUpMultiplier;
    public int _powerUpThreshold;
}