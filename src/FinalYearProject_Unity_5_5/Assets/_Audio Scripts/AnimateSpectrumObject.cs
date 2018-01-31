using UnityEngine;
using System.Collections;
/// <summary>
/// This class controls the behaviour of objects that do not interact with the player.
/// Gameobjects controlled by this script are animated using frequency bands or amplitude.
/// This script must be a component of the GameObject that needs to be animated.
/// </summary>
/// 
[RequireComponent(typeof(Gradient))]
public class AnimateSpectrumObject : MonoBehaviour
{
    public enum frequencyBand { ONE, TWO, THREE, FOUR, FIVE, SIX, SEVEN, EIGHT }
    public frequencyBand _frequencyBand = frequencyBand.ONE;
    public int FrequencyBand
    {
        get { return (int) _frequencyBand; }
        set { _frequencyBand = (frequencyBand) value;}
    }
    public bool _randomFrequencyBand;
    // Scale Animation
    [Space(20)]
    public bool _animateScale;
    public enum animateScaleWith { FREQUENCY_BAND, AMPLITUDE }
    public animateScaleWith _animateScaleWith = animateScaleWith.FREQUENCY_BAND;
    public Vector3 _scaleMultipliers;
    private Vector3 _startScale;
    private Vector3 _newScale;
    // Material Animation
    [Space(20)]
    public bool _animateMaterial;
    public enum animateMaterialWith { SCALE, SPEED }
    public animateMaterialWith _animateMaterialWith = animateMaterialWith.SCALE;
    [Range(5, 10)]
    public float _animationSpeed;
    public Gradient _gradient;
    private Material _material;
    // Rotation Animation
    [Space(20)]
    public bool _animateRotation;
    public enum animateRotationWith { AMPLTIUDE_SPEED, SPEED }
    public animateRotationWith _animateRotationWith = animateRotationWith.AMPLTIUDE_SPEED;
    public enum rotationType { HORIZONTAL, VERTICAL, HORIZONTAL_VERTICAL }
    public rotationType _rotationType = rotationType.HORIZONTAL;
    private Transform _parentChildTransform;
    [Range(0, 1)]
    public float _horizontalAxisSpeed;
    [Range(0, 1)]
    public float _verticalAxisSpeed;
    private Vector3 _startPos;
    // Use this for initialization
    void Start()
    {
        // The starting localScale of the object
        _startScale = gameObject.transform.localScale;
        // The material of the object
        if(GetComponent<MeshRenderer>() != null)
        _material = GetComponent<MeshRenderer>().materials[0];
        // The starting position of the object
        _startPos = transform.position;
        // Check whether frequency band needs to be randomised
        if (_randomFrequencyBand) RandomiseFrequencyBand();
        // To know whether the object this script is attached to has a parent object
        // This is required to animate rotation in the x-z plane
        _parentChildTransform = transform.parent != null ? transform.parent : transform;
    }

    // Update is called once per frame
    void Update()
    {
        if(SpectrumAnalyser.Instance._isSampling)
        {
            // Animate scale transform
            AnimateScale();
            // Animate material color
            AnimateMaterial();
            // Move object around y axis
            AnimateRotation();
        }
        /* Debuging Section */
        //Debug.Log("LocalScale: " + transform.localScale.sqrMagnitude); // dont use: big integer values
        //Debug.Log("Scale: " + transform.localScale.magnitude);
    }
    /// <summary>
    /// This function randomises the frequency band used to animate an object
    /// </summary>
    void RandomiseFrequencyBand()
    {
        _frequencyBand = (frequencyBand)Random.Range(0, 7);
    }
    /// <summary>
    /// Animate the object by changing its local scale with the spectrum data.
    /// </summary>
    void AnimateScale()
    {
        if (_animateScale)
        {
            if (_animateScaleWith == animateScaleWith.FREQUENCY_BAND)
            {

                // Calculate new scale vector
                _newScale.Set(SpectrumAnalyser.Instance._normalisedSmoothFreqBands[(int)_frequencyBand] * _scaleMultipliers.x, SpectrumAnalyser.Instance._normalisedSmoothFreqBands[(int)_frequencyBand] * _scaleMultipliers.y, SpectrumAnalyser.Instance._normalisedSmoothFreqBands[(int)_frequencyBand] * _scaleMultipliers.z);
                // Apply new scale vector 
                transform.localScale = _startScale + _newScale;
            }

            if (_animateScaleWith == animateScaleWith.AMPLITUDE)
            {
                // Calculate new scale vector 
                _newScale.Set(SpectrumAnalyser.Instance._smoothAmplitude * _scaleMultipliers.x, SpectrumAnalyser.Instance._smoothAmplitude * _scaleMultipliers.y, SpectrumAnalyser.Instance._smoothAmplitude * _scaleMultipliers.z);
                // Apply new scale vector
                transform.localScale = _startScale + _newScale;
            }
        }
    }
    /// <summary>
    /// Rotate the object using amplitude and/or speed.
    /// </summary>
    void AnimateRotation()
    {
        if (_animateRotation)
        {
            if (_animateRotationWith == animateRotationWith.AMPLTIUDE_SPEED)
            {
                if (_rotationType == rotationType.HORIZONTAL)
                {
                    float _rotationAngle = SpectrumAnalyser.Instance._smoothAmplitude * _horizontalAxisSpeed;
                    _parentChildTransform.RotateAround(Vector3.zero, Vector3.up, _rotationAngle);
                }
                if (_rotationType == rotationType.VERTICAL)
                {
                    float _rotationAngle = SpectrumAnalyser.Instance._smoothAmplitude * _verticalAxisSpeed;
                    _parentChildTransform.RotateAround(Vector3.zero, Vector3.right, _rotationAngle);
                }
            }
            else if (_animateRotationWith == animateRotationWith.SPEED)
            {
                if (_rotationType == rotationType.HORIZONTAL)
                {
                    float _rotationAngle = _horizontalAxisSpeed;
                    _parentChildTransform.RotateAround(Vector3.zero, Vector3.up, _rotationAngle);
                }
                if (_rotationType == rotationType.VERTICAL)
                {
                    float _rotationAngle = _verticalAxisSpeed;
                    _parentChildTransform.RotateAround(Vector3.zero, Vector3.right, _rotationAngle);
                }
                if (_rotationType == rotationType.HORIZONTAL_VERTICAL)
                {
                    _parentChildTransform.RotateAround(Vector3.zero, Vector3.up, _horizontalAxisSpeed);

                    float angle = Time.timeSinceLevelLoad * _verticalAxisSpeed;
                    float ypos = _startPos.y + Mathf.Sin(angle);
                    Vector3 newPos = new Vector3(transform.position.x, ypos, transform.position.z);
                    transform.position = newPos;
                }
            }
        }
    }
    /// <summary>
    /// Animate material color using speed or scale.
    /// </summary>
    void AnimateMaterial()
    {
        if (_animateMaterial)
        {
            if (_animateMaterialWith == animateMaterialWith.SCALE)
                // Change material color 
                SetMaterialColor(_material, transform.localScale.magnitude/2f);
            else
                SetMaterialColor(_material, _animationSpeed);
        }
    }
    /// <summary>
    /// This function animtes the emission color of a material using a Gradient
    /// </summary>
    /// <param name="_mat">This is the material to be animated</param>
    /// <param name="_duration">This determines the new emission color of the material</param>
    void SetMaterialColor(Material _mat, float _duration)
    {
        // Mathf.Repeat returns the remainder of the division Time.time/_duration.
        // The result is then normalised (0 to 1) by dividing it with _duration.
        // This normalised value is then used to evaluate the gradient color 
        // Evaluate function takes a value from 0 to 1 to calculate the gradient color at a given time
        float t = Mathf.Repeat(Time.time, _duration) / _duration;
        //Debug.Log(t); // Use this to debug the value of t (Tip: Avoid having more than one instance of this script active)
        _mat.SetColor("_EmissionColor", _gradient.Evaluate(t));
    }
}

