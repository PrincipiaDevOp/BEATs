using UnityEngine;
using System.Collections;

public class OnAudioColorMesh : MonoBehaviour
{

    Material _material;
    // Use this for initialization
    void Start()
    {
        _material = gameObject.GetComponent<MeshRenderer>().materials[0];
    }

    // Update is called once per frame
    void Update()
    {
        Mesh mesh = GetComponent<MeshFilter>().mesh;
        //Vector3[] vertices = mesh.vertices;
        //int i = 0;
        //while(i <vertices.Length)
        //{
        //    vertices[i] += Vector3.up * Time.deltaTime;
        //    i++;
        //}
        //mesh.vertices = vertices;
        //mesh.RecalculateBounds();
        Color[] newColors = new Color[mesh.vertices.Length];
        for (int i = 0; i < mesh.vertices.Length; i++)
        {
            newColors[i] = new Color(0.5f * SpectrumAnalyser.Instance._smoothAmplitude, 0.1f * SpectrumAnalyser.Instance._smoothAmplitude, 0.3f * SpectrumAnalyser.Instance._smoothAmplitude);
        }
        mesh.colors = newColors;
        // Calculate new color
        Color _color = new Color(SpectrumAnalyser.Instance._normalisedSmoothFreqBands[3], SpectrumAnalyser.Instance._normalisedSmoothFreqBands[3], SpectrumAnalyser.Instance._normalisedSmoothFreqBands[3]);
        // Apply new color to object
        _material.SetColor("_EmissionColor", _color);

    }
}
