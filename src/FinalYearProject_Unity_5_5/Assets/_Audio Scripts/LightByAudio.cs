using UnityEngine;
using System.Collections;
/// <summary>
/// This script changes the intensity of a Light component.
/// This script must be attached to a GameObject with a Light component.
/// </summary>
/// 
[RequireComponent(typeof(Light))]
public class LightByAudio : MonoBehaviour
{
    private Light _light;
    [Space(10)]
    public _frequencyBands _frequencyBand = _frequencyBands.ONE;
    [Space(10)]
    public bool _animateLightColor;
    public enum AnimateWith { FREQUENCY_BAND, AMPLITUDE }
    public enum _frequencyBands { ONE, TWO, THREE, FOUR, FIVE, SIX, SEVEN, EIGHT };
    public AnimateWith _animateColorWith = AnimateWith.FREQUENCY_BAND;
    [Space(20)]
    public bool _animateLightIntensity;
    public AnimateWith _animateIntensityWith = AnimateWith.FREQUENCY_BAND;
    [Range(0, 8)]
    public float _minIntensity, _maxIntensity;
    // 
    [Space(10)]
    public Gradient _gradient;
    // Increase damping to slow down color transition and viceversa
    [Range(1, 1000)]
    public float _damping; 

    // Use this for initialization
    void Start()
    {
        _light = GetComponent<Light>();
    }

    // Update is called once per frame
    void Update()
    {
        // Animate Light
        AnimateLight();
    }

    void AnimateLight()
    {
        if (_animateLightColor)
        {
            if (_animateColorWith == AnimateWith.FREQUENCY_BAND)
            {
                _light.color = SetLightColor(SpectrumAnalyser.Instance._normalisedSmoothFreqBands[(int)_frequencyBand]);
            }
            if (_animateColorWith == AnimateWith.AMPLITUDE)
            {
                _light.color = SetLightColor(SpectrumAnalyser.Instance._smoothAmplitude);
            }
        }

        if (_animateLightIntensity)
        {
            if (_animateIntensityWith == AnimateWith.FREQUENCY_BAND)
            {
                _light.intensity = (SpectrumAnalyser.Instance._normalisedSmoothFreqBands[(int)_frequencyBand] * (_maxIntensity - _minIntensity)) + _minIntensity;
            }
            if (_animateIntensityWith == AnimateWith.AMPLITUDE)
            {
                _light.intensity = (SpectrumAnalyser.Instance._smoothAmplitude * (_maxIntensity - _minIntensity)) + _minIntensity;
            }
        }
    }

    /// <summary>
    /// This function return a color from a Gradient.
    /// </summary>
    /// <param name="_transitionTime">This determines the new emission color of the material</param>
    Color SetLightColor(float _transitionTime)
    {
        // Mathf.Repeat returns the remainder of the division Time.time/_duration.
        // The result is then normalised (0 to 1) by dividing it with _duration.
        // This normalised value is then used to calculate the gradient color at a given time
        // Evaluate takes a value from 0 to 1 to calculate the gradient color.

        float t = Mathf.Repeat(Time.time / _damping, _transitionTime) / _transitionTime;
        //Debug.Log("T: " + t);

        return _gradient.Evaluate(t);
    }
}
