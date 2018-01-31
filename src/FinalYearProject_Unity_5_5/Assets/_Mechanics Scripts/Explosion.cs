using UnityEngine;
using System.Collections;

public class Explosion : MonoBehaviour
{
    public GameObject _ps;
    public int _scoreValue;
    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == "Bullet")
        {
            AnimateExplosion();
        }
    }

    public void AnimateExplosion()
    {
        Debug.Log("Explosion");
        // Increment score counter in ScoreManager
        ScoreManager.Instance.AddScore(_scoreValue);
        // Increment hits counter in ScoreManager
        ScoreManager.Instance._hits++;
        // Show Score pop-up text
        PopUpTextController.CreatePopUpText(_scoreValue.ToString(), gameObject.transform);
        // Instantiate explosion
        GameObject _go = (GameObject)Instantiate(_ps, gameObject.transform.position, Quaternion.identity);
        ParticleSystem _explosion = _go.GetComponentInChildren<ParticleSystem>();
        // Play animation
        _explosion.Play();
        // Disable note rendering
        gameObject.SetActive(false);
        // Destroy note and explosion gameobjects after particle animation finishes
        Destroy(gameObject, _explosion.main.duration);
        Destroy(_go, _explosion.main.duration);
    }
}
