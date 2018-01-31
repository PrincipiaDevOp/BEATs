using UnityEngine;
using System.Collections.Generic;

public static class MathA
{

    /// <summary>
    /// Returns a List of GameObjects all evenly spaced in one of 3 shapes.( Wall, SemiCircle, Circle)
    /// </summary>
    /// <param name="pf">Prefab.</param>
    /// <param name="radius">The radius used for semi-circle, circle and for wall length.</param>
    /// <param name="amount">The size of the List of GameObjects.</param>
    /// <param name="shape">The geometric shape in which the objects are placed in the environment.</param>
    /// <returns></returns>
    public static List<GameObject> SpawnObjectsByShape(GameObject pf, float radius, int amount, Shapes shape)
    {
        List<GameObject> objects = new List<GameObject>(amount);

        if (shape == Shapes.WALL)
        {
            for (int i = 0; i < amount; i++)
            {
                float wallPos = -radius + i * radius / amount * 2;
                var pos = new Vector3(wallPos, 0, 1);
                //Instantiate
                GameObject obj = Object.Instantiate(pf, pos, Quaternion.identity) as GameObject;
                objects.Add(obj);
            }
        }
        else
        {
            //Circle or Semi-Circle Shape
            float n = shape == Shapes.CIRCLE ? 2 : 1; //n=2 (Full circle), n=1 (Semi-circle)

            for (int i = 0; i < amount; i++)
            {
                float angle = i * Mathf.PI * n / amount;
                Vector3 position = new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle)) * radius;

                Quaternion rot = Quaternion.LookRotation(position - Vector3.zero);
                GameObject obj = Object.Instantiate(pf, position, rot) as GameObject;

                objects.Add(obj);
            }
        }

        return objects;
    }
    public static List<GameObject> SpawnMenuObjects(GameObject goPrefab, float radius, int amount)
    {
        List<GameObject> objects = new List<GameObject>(amount);

        //Vector3 center = Vector3.zero;
        Vector3 center = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width / 2, Screen.height / 2, Camera.main.farClipPlane / 16 + 10f));
        for (int i = 0; i < amount; i++)
        {
            float angle = i * Mathf.PI * 2 / amount;
            float x = center.x + Mathf.Cos(angle) * radius;
            float y = center.y + Mathf.Sin(angle) * radius;

            Vector3 pos = center + new Vector3(x, y, 0);
            Quaternion rot = Quaternion.LookRotation(Vector3.forward, pos);
            GameObject go = Object.Instantiate(goPrefab,pos,rot) as GameObject;

            objects.Add(go);
        }

        return objects;
    }
}
/*
void Old()
{
    // Start func
    // Initialize cube buffer 
    //_objectBuffer = new GameObject[_numberOfObjects];

    //for (int i = 0; i < _numberOfObjects; i++)
    //{
    //    float angle = i * Mathf.PI * 2 / _numberOfObjects;
    //    Vector3 pos = new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle)) * _radius;
    //    Instantiate(_scorePrefab, pos, Quaternion.identity);
    //}
    //_objectBuffer = GameObject.FindGameObjectsWithTag("Cubes");
    //_objectBuffer = new GameObject[_numberOfObjects];
    // OnInstantiateObjects();
    //for (int i = 0; i < _objectBuffer.Length; i++)
    //{
    //    if (_scorePrefab != null)
    //    {
    //        _objectBuffer[i].transform.localScale = new Vector3(_startScale, (SpectrumAnalyser.Instance._spectrumSamples[i] * _maxScale) + _startScale, _startScale);
    //    }
    //}
}

void SquareInst()
{
    //int i, j; // rows, columns
    //float _xpos = 0f, _zpos = 0f; // coordinate axes
    //float _amount = 16.0f;
    //Vector3 _pos;
    //for (i = 0; i < 5; i++)
    //{
    //    for (j = 0; j < 5; j++)
    //    {
    //        _pos = new Vector3(_xpos, 0, _zpos);
    //        _scorePrefab.transform.localPosition = transform.localPosition;
    //        _scorePrefab.GetComponent<VisualiserObject>()._frequencyBand = (VisualiserObject.FrequencyBand)Random.Range(0, 7);
    //        Instantiate(_scorePrefab, _pos, Quaternion.identity);
    //        _xpos += _amount;
    //    }
    //    // Reset x coordinate for next iteration
    //    _xpos = 0.0f;
    //    // Set new z coordinate for next iteration
    //    _zpos += _amount;
    //}
}
*/
