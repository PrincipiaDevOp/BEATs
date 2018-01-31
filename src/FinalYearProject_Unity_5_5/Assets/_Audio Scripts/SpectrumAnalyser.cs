using UnityEngine;
using System.Collections;
/// <summary>
/// Spectrum analyser.
/// Unity documentation used to implement this script:
/// - Get spectrum data (frequencies) from an audio source
/// https://docs.unity3d.com/ScriptReference/AudioSource.GetSpectrumData.html
/// - List of FFT window algorithm provided by Unity:
/// https://docs.unity3d.com/ScriptReference/FFTWindow.html
/// - Audio Frequency:
/// https://en.wikipedia.org/wiki/Audio_frequency
/// - Sample Rates:
/// http://wiki.audacityteam.org/wiki/Sample_Rates
/// - Feature scaling:
/// https://en.wikipedia.org/wiki/Feature_scaling
/// </summary>
public class SpectrumAnalyser : MonoBehaviour
{
    // Singleton Pattern Instance
    public static SpectrumAnalyser Instance { get; private set; }
    [SerializeField]
    private AudioSource _audioSource;
    // Non-normalised Spectrum Data Variables
    [HideInInspector]
    public float[] _spectrumLeftChannel;
    [HideInInspector]
    public float[] _spectrumRightChannel;
    [HideInInspector]
    public float[] _freqBands = new float[8];
    [HideInInspector]
    private float[] _smoothFreqBands = new float[8];
    // Normalised Spectrum Data Variables
    [HideInInspector]
    public float[] _normalisedFreqBands = new float[8];
    //[HideInInspector]
    public float[] _normalisedSmoothFreqBands = new float[8];
    private float[] _freqBandsHighestValues = new float[8];
    // Smoothing Variables
    private float[] _SmoothingValues = new float[8];
    private float _smoothingValue; // 0.005-0.010 for best results
    private float _smoothingMultiplier; //1.2-2.0 for best results
    // Average Amplitude Data Variables
    public float _amplitude, _smoothAmplitude;
    private float _amplitudeHighest;
    [HideInInspector]
    public bool _isSampling = false;

    // Awake Method
    void Awake()
    {
        #region SINGLETON_PATTERN
        // Check if there are any other instances conflicting
        if (Instance != null && Instance != this)
        {
            // if tha tis the case, we destroy other instances
            Destroy(gameObject);
        }
        // Save singleton instance
        Instance = this;
        // Don't destroy this intance between scenes
        DontDestroyOnLoad(gameObject);
        #endregion
    }
    // Use this for initialization
    void Start()
    {
        _spectrumLeftChannel = new float[(int)GameMaster.Instance._gameSettings._spectrum._FFTSamples];
        _spectrumRightChannel = new float[(int)GameMaster.Instance._gameSettings._spectrum._FFTSamples];
        _audioSource = Camera.main.GetComponent<AudioSource>();
        _smoothingValue = 0.05f;
        _smoothingMultiplier = 1.2f;
    }

    // Update is called once per frame
    void Update()
    {
        if (_audioSource == null)
            GetAudioSource();

        if (_audioSource.isPlaying && !AudioListener.pause)
        {
            _isSampling = true;
            GetSprectrumDataFromAudioSource();
            CreateFrequencyBands(8, _freqBands);
            CreateSmoothBandBuffer();
            CreateNormalisedBands();
            GetNormalisedAmplitude();
        }
        else
        {
            _isSampling = false;
            //Debug.Log("SpectrumAnalyzer. Update(): No AudioSource is playing.");
        }

        //print("SpectrumAnalyzer is sampling: "+_isSampling);
    }
    /// <summary>
    /// Sample spectrum data from audio source.
    /// </summary>
    void GetSprectrumDataFromAudioSource()
    {
		// Get spectrum data and store in array. Can use one or both channels of audio source - this is set-up in GM gameObject in Inspector.
        if (GameMaster.Instance._gameSettings._spectrum._channels == Channels.ALL)
        {
            _audioSource.GetSpectrumData(_spectrumLeftChannel, 0, GameMaster.Instance._gameSettings._spectrum._FFTWindow);
            _audioSource.GetSpectrumData(_spectrumRightChannel, 1, GameMaster.Instance._gameSettings._spectrum._FFTWindow);
        }
        else if(GameMaster.Instance._gameSettings._spectrum._channels == Channels.LEFT)
        {
            _audioSource.GetSpectrumData(_spectrumLeftChannel, 0, GameMaster.Instance._gameSettings._spectrum._FFTWindow);
        }
        else
        {
            _audioSource.GetSpectrumData(_spectrumRightChannel, 1, GameMaster.Instance._gameSettings._spectrum._FFTWindow);
        }
    }
    /// <summary>
    /// Create an array of discrete frequency bands using spectrum data.
    /// </summary>
    void CreateFrequencyBands(int numberOfBands, float[] _bandBuffer)
    {
        #region INFORMATION
        /*
        This function is based on the human audio frequency range which determines that the average audible hearing range of a human is between 20Hz and 20kHz.
        
        Human Audio Frequency Range:
        20 - 60 Hz(Sub-bass)
        60 - 250 Hz(Bass)
        250 - 500 Hz(Midrange)
        500 - 2000 Hz 
        2000 - 4000 Hz 
        4000 - 6000 Hz(High Mids)
        6000 - 20000 Hz(High Frequency)
        
        Important Concepts:
        Sample Rate (44100 Hz): is the number of samples of audio carried per second
        Number of samples: 8192 
        Sample Rate/Number of Samples = 43Hz/sample
        
        44.1 kHz (44100 Hz) is the sampling rate of audio CDs giving a 20 kHz maximum frequency. 20 kHz is the highest frequency generally audible by humans.
        
        7 bands (from human freq range) + 1 band (0 to 20Hz range) 
        
        Frequency Bands: (Simplified version using powers of 2)
        Band[0] : 2 samples = 86 Hz /////// 0-86hz ~= 20-60 Hz  
        Band[1] : 4 samples = 172 Hz ////// 87-258(172+86)Hz ~= 60-250 Hz
        Band[2] : 8 samples = 344 Hz ////// 259-692 ~= 250-500 Hz 
        Band[3] : 16 samples = 688 Hz ///// 603-1290 ~= 500-2000 Hz (+17 samples) 603 - 2021Hz
        Band[4] : 32 samples = 1376 Hz //// 1291-2666 ~= 2000-4000 Hz (+32 samples) 2022 - 4042Hz
        Band[5] : 64 samples = 2752 /////// 2667-5418 ~= 4000-6000 Hz (+16 samples) 4043 - 6100Hz
        Band[6] : 128 samples = 5504 Hz /// 5419-10922 ~= 6000-20000 Hz (+256 samples) 6101 - 21930Hz
        Band[7] : 256 samples = 11008 Hz // 10923-21930 ~= 6000-20000 Hz

		Following code is optimised for 8192 and 4096 samples using an FFT window.
        */
        #endregion
        // The maximum number of Hz per sample, i.e. frequency resolution 
        int HzPerSample = (int)Mathf.Floor((int)AudioSettings.outputSampleRate /
            (int)GameMaster.Instance._gameSettings._spectrum._FFTSamples);
        // The Hz max limit of frequency range for all bands
        // E.g: First band should have a frequency range from 0 to 60 Hz
        // with error margin the freq range can go up to expected range + HzErrorMargin
        int HzErrorMargin = 0;
        // Multiply total number of required samples by this
        int samplesMultiplier = 0;
        // The total number of frequency bands
        int maxBandNumber = numberOfBands;

        if (HzPerSample <= 60)
        {
            HzErrorMargin = 20;
            samplesMultiplier = (int)Mathf.Floor(60 / HzPerSample) + HzErrorMargin / HzPerSample;
        }
        else samplesMultiplier = 1;
        if (HzPerSample == 43)
        {
            samplesMultiplier = 2;
        }
        //Debug.Log("HzPerSample: " + HzPerSample + "/ sampleMultiplier: " + samplesMultiplier);

        // Nth frequency band
        int curBand = 0;
        //int c = 0;
        for (int i = 0; i < maxBandNumber; i++)
        {
            float average = 0;
            // Calculate number of required samples per frequency band
            int nSamples = (int)Mathf.Pow(2, i) * samplesMultiplier;
            // Add more samples to last frequency band so that FFTSamples/2 bins are used across all bands
            if (i == 7) nSamples += samplesMultiplier;
            for (int j = 0; j < nSamples; j++)
            {
                if (GameMaster.Instance._gameSettings._spectrum._channels == Channels.ALL)
                {
                    average += _spectrumLeftChannel[curBand] * _spectrumRightChannel[curBand] * (curBand + 1);
                }
                else if (GameMaster.Instance._gameSettings._spectrum._channels == Channels.LEFT)
                {
                    average += _spectrumLeftChannel[curBand] * (curBand + 1);
                }
                else
                {
                    average += _spectrumRightChannel[curBand] * (curBand + 1);
                }
                // Increase for next band
                curBand++;
            }
            average = Mathf.Abs(average / curBand);
            // Scale up average and add to frequency bands array
            _bandBuffer[i] = average * 10f;
            //c += nSamples;
            //Debug.Log("Added Samples: " + c);
        }
    }
    /// <summary>
    /// Smooth low to high frequency transitions using buffers.
    /// </summary>
    void CreateSmoothBandBuffer()
    {
        for (int g = 0; g < 8; ++g)
        {
            if (_freqBands[g] > _smoothFreqBands[g])
            {
                // If frequency band is higher than band buffer,
                // set band buffer equal to frequency band
                _smoothFreqBands[g] = _freqBands[g];
                //  Initialise smoothing values array
                _SmoothingValues[g] = _smoothingValue;
            }
            if (_freqBands[g] < _smoothFreqBands[g])
            {
                // If frequency band is lower than smooth frequency band, decrease smooth frequency band
                // High to Low transition must be slow
                _smoothFreqBands[g] -= _SmoothingValues[g];
                // Increase smooothing values to accelerate the decrease
                // Low to High transition must be fast 
                _SmoothingValues[g] *= _smoothingMultiplier;
            }
        }
    }
    /// <summary>
    /// Normalise frequency bands, i.e. values [0 1].
    /// </summary>
    void CreateNormalisedBands()
    {
        for (int i = 0; i < 8; i++)
        {
            if (_freqBands[i] > _freqBandsHighestValues[i])
            {
                // Get the highest frequency values in bands
                _freqBandsHighestValues[i] = _freqBands[i];
            }
            // Normalise frequency values to store them in arrays
            _normalisedFreqBands[i] = (_freqBands[i] / _freqBandsHighestValues[i]);
            _normalisedSmoothFreqBands[i] = (_smoothFreqBands[i] / _freqBandsHighestValues[i]);
        }
    }
    /// <summary>
    /// Calculate a normalised amplitude from all frequency bands.
    /// </summary>
    void GetNormalisedAmplitude()
    {
        // Local variables to calculate amplitude
        float _currentAmplitude = 0;
        float _currentSmoothAmplitude = 0;
        for (int i = 0; i < 8; i++)
        {
            // Calculate amplitude
            _currentAmplitude += _normalisedFreqBands[i];
            _currentSmoothAmplitude += _normalisedSmoothFreqBands[i];
        }
        // Find highest amplitude value
        if (_currentAmplitude > _amplitudeHighest)
        {
            _amplitudeHighest = _currentAmplitude;
        }
        // Normalise sum of amplitudes and store values
        _amplitude = Mathf.Clamp01(Mathf.Abs(_currentAmplitude / _amplitudeHighest));
        _smoothAmplitude = Mathf.Clamp01(Mathf.Abs(_currentSmoothAmplitude / _amplitudeHighest));
    }
    /// <summary>
    /// Get the current Audio Source component attached to the main camera in the scene.
    /// </summary>
    public void GetAudioSource()
    {
        _audioSource = Camera.main.GetComponent<AudioSource>();
        Debug.Log("SpectrumAnalyzer: Scene changed. New AudioSource referenced.");
    }
}
