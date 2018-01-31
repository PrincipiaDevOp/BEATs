using UnityEngine;
using System.Collections;

public class HealthBar : MonoBehaviour
{
    public RectTransform _insideBar;

    public float _barWidth; // Get the width of the rect transform 
    public float _minBarPos; // The minimum position of the bar
    public float _maxBarPos = 0; // The start position of the bar
    [Range(0f, 1f)]
    public float _reduceAmount = 0.3f;

    private Vector2 _startPos; // The starting anchored position of RectTransform

    public float _lerpTime;
    public float _scoreMultiplier;

    public float _timeBetweenHits = 2f;
    public float _timeSinceLastHit = 0f;

    bool _wasHit = false;
    bool _firstCall = true;
    // Use this for initialization
    void Start()
    {
        _barWidth = _insideBar.sizeDelta.x;
        _minBarPos = -((_barWidth / 2) + (_barWidth / 2) * 0.75f);
        _startPos = _insideBar.anchoredPosition;

        // Initialise Score Bar Settings
        SetScoreSetitings();
    }
    // Update is called once per frame
    void Update()
    {
        if (GameMaster.Instance.GameState == GameMaster.GamePlayState.PLAYING)
        {
            if (_insideBar.anchoredPosition.x > _minBarPos)
            {
                StartCoroutine(AnimateBar());
            }
            else if (_insideBar.anchoredPosition.x <= _maxBarPos)
            {
                GameMaster.Instance.GameState = GameMaster.GamePlayState.LOSE;
                AudioListener.pause = true;
                EventsManager.UpdateUIView();
                // Reset score bar to start new game
                Reset();
            }
        }
    }
    IEnumerator AnimateBar()
    {
        if (_firstCall)
        {
            yield return new WaitForSeconds(5f);
            _firstCall = false;
        }
        else
        {
            //yield return new WaitForEndOfFrame();
            if (_insideBar.anchoredPosition.x > _minBarPos)
            {
                if (_wasHit == false)
                {
                    Vector2 pos = new Vector2(_insideBar.anchoredPosition.x - _reduceAmount, _insideBar.anchoredPosition.y);
                    _insideBar.anchoredPosition = Vector2.Lerp(_insideBar.anchoredPosition, pos, Time.deltaTime * _lerpTime);
                }
                else if (_wasHit == true)
                {
                    LerpTime();
                }
            }
        }
    }
    void LerpTime()
    {
        _timeSinceLastHit += Time.deltaTime;

        if (_timeSinceLastHit >= _timeBetweenHits)
        {
            _lerpTime = 10f;
            _wasHit = false;
            _timeSinceLastHit = 0f;
        }
    }
    public void Increase(float size)
    {
        if (_insideBar.anchoredPosition.x < (_maxBarPos - (_reduceAmount + size / 100) * _scoreMultiplier))
        {
            Vector2 pos = new Vector2(_insideBar.anchoredPosition.x + (_reduceAmount + size / 100) * _scoreMultiplier, _insideBar.anchoredPosition.y);
            _insideBar.anchoredPosition = Vector2.Lerp(_insideBar.anchoredPosition, pos, Time.deltaTime * size);
            _lerpTime = 1f;
            _wasHit = true;
        }
    }
    public void Decrease(float size)
    {
        Vector2 pos = new Vector2(_insideBar.anchoredPosition.x - (_reduceAmount + size / 100), _insideBar.anchoredPosition.y);
        _insideBar.anchoredPosition = pos;
    }
    public void SetScoreSetitings()
    {
        if (GameMaster.Instance.GameMode == GameMaster.GamePlayMode.EASY)
        {
            _reduceAmount = 1f;
            _scoreMultiplier = 1f;
        }
        else if (GameMaster.Instance.GameMode == GameMaster.GamePlayMode.MEDIUM)
        {
            _reduceAmount = 2f;
            _scoreMultiplier = 3f;
        }
        else if (GameMaster.Instance.GameMode == GameMaster.GamePlayMode.HARD)
        {
            _reduceAmount = 3f;
            _scoreMultiplier = 5f;
        }
    }
    public void Reset()
    {
        _insideBar.anchoredPosition = _startPos;
    }
}
