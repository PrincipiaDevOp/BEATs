using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AnimateFrequencyNote : MonoBehaviour
{
    private Vector3 _startPos;
    private float time = 0.0f;
    private float _theta = 0.0f;
    private float _currentTime = 0.0f;

    [SerializeField] private int _randomNumber = 0;
    private float _renderDelayTime = 0.0f;
    private float _LifeTime = 0.0f;

    private float _archemdianSpeed = 0.03f;
    private float _conicalSpeed = 0.03f;
    private float _polarSpeed = 0.010f;

    // Use this for initialization
    void Start()
    {
        _startPos = transform.position;
        _renderDelayTime = GameMaster.Instance._gameSettings._frequencyNotes._renderDelayTime;
        _LifeTime = GameMaster.Instance._gameSettings._frequencyNotes._noteLifeTime;
        _randomNumber = Random.Range(0, 3);

        StartCoroutine(RenderDelay());
    }

    // Update is called once per frame
    void Update()
    {
        if (GameMaster.Instance.GameState == GameMaster.GamePlayState.PLAYING)
        {
            AnimateMotion();
            _currentTime += Time.deltaTime;
            if (_currentTime >= _LifeTime)
            {
                // Increment misses counter in ScoreManager
                ScoreManager.Instance._misses++;
                Destroy(gameObject);
            }
        }
        if (GameMaster.Instance.GameState == GameMaster.GamePlayState.MENU)
        {
            Destroy(gameObject);
        }
    }
    void AnimateMotion()
    {
        switch (_randomNumber)
        {
            case 0: AnimateArchemedianSpiral();
                break;
            case 1: AnimateConicalSpiral();
                break;
            case 2: AnimatePolarRose();
                break;
        }

        #region DEBUG CODE
        //if (_randomNumber == 0)
        //{
        //    AnimateArchemedianSpiral();
        //}
        //else if (_randomNumber == 1)
        //{
        //    AnimateConicalSpiral();
        //}
        //else if (_randomNumber == 2)
        //{
        //    AnimatePolarRose();
        //}
        //if (GameMaster.Instance.GameMode == GameMaster.GamePlayMode.EASY)
        //{
        //    AnimateArchemedianSpiral();
        //}
        //else if (GameMaster.Instance.GameMode == GameMaster.GamePlayMode.MEDIUM)
        //{
        //    AnimateConicalSpiral();
        //}
        //else if (GameMaster.Instance.GameMode == GameMaster.GamePlayMode.HARD)
        //{
        //    AnimatePolarRose();
        //}
        #endregion
    }
    /// <summary>
    /// Rotate the object following an Archemedean spiral path.
    /// </summary>
    void AnimateArchemedianSpiral()
    {
        #region INFORMATION
        //Archimedian spiral:
        //    r = a + b * _theta
        // - Changing parameter a will turn the spiral
        // -Parameter b controls the distance between successive turnings
        //In carteisian coordinates:
        // x = r * cos(_theta)
        // y = r * sin(_theta)
        // z = _theta;
        #endregion

        time = SpectrumAnalyser.Instance._smoothAmplitude * _archemdianSpeed;

        Vector3 pos = _startPos;
        float a = 10f, b = 0.15f;
        pos.x += (a + b * _theta) * Mathf.Cos(_theta);
        pos.y += _theta * 1.5f;
        pos.z += (a + b * _theta) * Mathf.Sin(_theta);

        transform.position = pos;

        _theta += time;
    }
    /// <summary>
    /// Rotate the object following a conical/logarithmic spiral path.
    /// For outside to inside spiral, b must be less than zero.
    /// For inside to outside spiral, b greater than zero.
    /// </summary>
    void AnimateConicalSpiral()
    {
        time = SpectrumAnalyser.Instance._smoothAmplitude * _conicalSpeed;
        Vector3 pos = _startPos;
        float a = 10, b = 0.1f;

        pos.x += a * Mathf.Exp(-b * _theta) * Mathf.Cos(_theta);
        pos.z += a * Mathf.Exp(-b * _theta) * Mathf.Sin(_theta);
        pos.y += _theta * 1.5f;

        transform.position = pos;

        _theta += time;
    }
    /// <summary>
    /// Rotate the object following a polar rose path.
    /// </summary>
    void AnimatePolarRose()
    {
        #region INFOMRATION
        // r = cos(k * _theta)
        // x = cos(k * _theta) * cos(_theta)
        // y = _theta
        // z = cos(k * _theta) * sin(_theta)
        #endregion
        Vector3 pos = _startPos;
        time = SpectrumAnalyser.Instance._smoothAmplitude * _polarSpeed;
        int k = 5, a = 10;

        pos.x += a * Mathf.Cos(k * _theta) * Mathf.Cos(_theta);
        pos.z += a * Mathf.Cos(k * _theta) * Mathf.Sin(_theta);
        pos.y += _theta * 10f;

        transform.position = pos;

        _theta += time;
    }
    IEnumerator RenderDelay()
    {
        yield return new WaitForSeconds(_renderDelayTime);

        gameObject.GetComponent<MeshRenderer>().enabled = true;
        gameObject.GetComponent<TrailRenderer>().enabled = true;
        gameObject.GetComponent<TrailRenderer>().Clear();
    }
}
