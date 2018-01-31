using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopUpTextController : MonoBehaviour
{

    private static ScorePopUpText _popUpTextPrefab;
    private static GameObject _canvas;
    public static void Initialize()
    {
        if (!_canvas)
            _canvas = GameObject.FindGameObjectWithTag("levelCanvas");
        if (!_popUpTextPrefab)
            _popUpTextPrefab = Resources.Load<ScorePopUpText>("Prefabs/PopUpText/PopUpTextParent");
    }

    public static void CreatePopUpText(string text, Transform textPosition)
    {
        ScorePopUpText instance = Instantiate(_popUpTextPrefab);
        //Vector2 screePos = Camera.main.WorldToScreenPoint(textPosition.position);
        Vector2 screePos = Camera.main.WorldToScreenPoint(new Vector2(textPosition.position.x + Random.Range(-.1f, .1f), textPosition.position.y + Random.Range(-.1f, .1f)));
        instance.transform.SetParent(_canvas.transform, false);
        instance.transform.position = screePos;
        instance.SetText(text);
    }
}
