using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScorePopUpText : MonoBehaviour
{
    public Animator _animator;
    private Text _popUpText;

    // Use this for initialization
    void OnEnable()
    {
        // Get clips info from animator
        AnimatorClipInfo[] clipInfo = _animator.GetCurrentAnimatorClipInfo(0);
        // Destroy object after clip is finished playing
        Destroy(gameObject, clipInfo[0].clip.length);
        // Get a reference to the Text of the UI element in the canvas
        _popUpText = _animator.GetComponent<Text>();
    }

    public void SetText(string txt)
    {
        _popUpText.text = txt;
    }
}
