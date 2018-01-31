using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class RadialButtonController : MonoBehaviour
{
    public GameObject[] _firstLayerButtons = new GameObject[3];
    private Vector3[] _buttonPos;
    public Transform _centerButton;
    public Transform _spectrumObjs; 
    public float _buttonDistance;
    private Vector3 _centerPos;
    private bool _isOpen;

    // Use this for initialization
    void Start()
    {
        _centerPos = _centerButton.position;
        float angle = (Mathf.PI / 2) / (_firstLayerButtons.Length - 1);
        float startAngle = 315f * Mathf.Deg2Rad;

        _buttonPos = new Vector3[_firstLayerButtons.Length];

        for (int i = 0; i < _firstLayerButtons.Length; i++)
        {
            float xpos = Mathf.Cos(startAngle + (angle * i)) * _buttonDistance;
            float ypos = Mathf.Sin(startAngle + (angle * i)) * _buttonDistance;
            Vector3 pos = new Vector3(xpos, ypos, 0);
            _buttonPos[i] = pos;
        }
    }

    public void OpenMenuButtons()
    {
        _isOpen = !_isOpen;

        if (_isOpen)
        {
            _centerButton.position = new Vector3(_centerButton.position.x - 300, _centerButton.position.y, _centerButton.position.z);
            _spectrumObjs.position = new Vector3(_spectrumObjs.position.x - 20, _spectrumObjs.position.y, _spectrumObjs.position.z);
        }
        else
        {
            _centerButton.position = new Vector3(_centerButton.position.x + 300, _centerButton.position.y, _centerButton.position.z);
            _spectrumObjs.position = new Vector3(_spectrumObjs.position.x + 20, _spectrumObjs.position.y, _spectrumObjs.position.z);

        }

        for (int i = 0; i < _firstLayerButtons.Length; i++)
        {
            if (_isOpen)
                _firstLayerButtons[i].transform.position = _centerPos + _buttonPos[i];
            else
                _firstLayerButtons[i].transform.position = _centerPos;
        }
    }
}
