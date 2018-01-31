using UnityEngine;
using System.Collections.Generic;
/// <summary>
/// This class instantiates GameObjects that do not interact with the player.
/// The GameObjects instantiated with this script are stored in a List.
/// This script must be attached to an empty GameObject in the scene, i.e. GameManager.
/// </summary>
public class SpawnByShape : MonoBehaviour
{
    private List<GameObject> _audioObjects;
    private GameObject _prefabType;
    private GameObject _folder;
    private Vector3 _startPos;

    // Use this for initialization
    void Start()
    {
        _startPos = transform.position;
        SpawnObjectsByShape();
        _folder.SetActive(false);
    }

    void Update()
    {
        if (GameMaster.Instance.GameState == GameMaster.GamePlayState.PLAYING)
        {
            _folder.SetActive(true);
            AnimateObjects();
        }
        else
        {
            _folder.SetActive(false);
        }
    }
    /// <summary>
    /// Spawns a list of GameObjects in a specific geometric shape. 
    /// </summary>
    void SpawnObjectsByShape()
    {
        // Identify type of prefab: cylinder, box or column
        if (GameMaster.Instance._gameSettings._visualiser._type == PrefabTypes.CYLINDER)
        {
            _prefabType = GameMaster.Instance._gameSettings._prefabs._cylinderPrefab;
        }
        else if (GameMaster.Instance._gameSettings._visualiser._type == PrefabTypes.BOX)
        {
            _prefabType = GameMaster.Instance._gameSettings._prefabs._boxPrefab;
        }
        else if (GameMaster.Instance._gameSettings._visualiser._type == PrefabTypes.COLUMN)
        {
            _prefabType = GameMaster.Instance._gameSettings._prefabs._columnPrefab;
        }
        else
        {
            Debug.LogError("GameMaster - Visualiser Settings: Not prefap type selected.");
        }

        // Create list of objects
        _audioObjects = MathA.SpawnObjectsByShape(_prefabType, GameMaster.Instance._gameSettings._visualiser._radius, GameMaster.Instance._gameSettings._visualiser._amount, GameMaster.Instance._gameSettings._visualiser._shape);

        //Organize objects in this "folder" under this transform
        _folder = new GameObject("Object-" + _audioObjects.Count);
        _folder.transform.SetParent(transform);

        foreach (var obj in _audioObjects)
        {
            obj.transform.SetParent(_folder.transform);
        }

        Debug.Log("Audio objects initialisation finished.");
    }
    /// <summary>
    /// Animates the scale of the instantiated GameObjects using raw spectrum data.
    /// </summary>
    void AnimateObjects()
    {
        if (GameMaster.Instance._gameSettings._visualiser._type == PrefabTypes.CYLINDER || GameMaster.Instance._gameSettings._visualiser._type == PrefabTypes.BOX)
        {
            for (int i = 0; i < _audioObjects.Count; i++)
            {
                float level = SpectrumAnalyser.Instance._spectrumLeftChannel[i] * GameMaster.Instance._gameSettings._visualiser._sensitivity * Time.deltaTime * 1000f;

                // Animate scale of objects 
                Vector3 previousScale = _audioObjects[i].transform.localScale;
                previousScale.y = Mathf.Lerp(previousScale.y, level, GameMaster.Instance._gameSettings._visualiser._speed * Time.deltaTime);
                _audioObjects[i].transform.localScale = previousScale;

                // If prefab type is cylinder, adjust position in respect to currect localScale
                if (GameMaster.Instance._gameSettings._visualiser._type == PrefabTypes.CYLINDER)
                {
                    Vector3 pos = new Vector3(_audioObjects[i].transform.position.x, 0, _audioObjects[i].transform.position.z);
                    pos.y = _startPos.y + previousScale.y;
                    _audioObjects[i].transform.position = pos;
                }
            }
        }
        else
        {
            for (int i = 0; i < _audioObjects.Count; i++)
            {
                _audioObjects[i].GetComponentInChildren<AnimateSpectrumObject>().FrequencyBand = i;
            }
        }
    }
}
