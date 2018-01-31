using UnityEngine;
using System.Collections;

public class Shooting : MonoBehaviour
{
    public GameObject _bulletPrefab;
    public GameObject _firePoint;
    public ParticleEmitter _particleGun;

    // Maximum spread angle of bullets
    public float _maxSpreadAngle = 15.0f;
    // Bullet speed for default direction shooting
    public float _defaultSpeed;
    // Bullet speed for random direction shooting
    public float _randomSpeed;

    // Update is called once per frame
    void Update()
    {
        if(GameMaster.Instance.GameState == GameMaster.GamePlayState.PLAYING)
        {
            if (!GameMaster.Instance._gameSettings._powerUpSettings._isPowerUp)
                FireNormal();
            else if (GameMaster.Instance._gameSettings._powerUpSettings._isPowerUp)
                _particleGun.FireParticles();
        }
    }
    /// <summary>
    /// Fire a single bullet
    /// </summary>
    void FireNormal()
    {
        if (Input.GetMouseButtonDown(0))
        {
            // Instantiate bullet at fire point
            GameObject tempBullet = Instantiate(_bulletPrefab, _firePoint.transform.position, _firePoint.transform.rotation);
            // Apply force to the bullet
            tempBullet.GetComponentInChildren<Rigidbody>().AddForce(_firePoint.transform.forward * _defaultSpeed);
            // Increment shots counter in ScoreManager
            ScoreManager.Instance._shots++;
        }
    }
    // Below: Unfinished function desgined to create a shotgun effect for firing bullets
    void FireRandom()
    {
        //Vector3 fireDirection = transform.forward;

        //Quaternion fireRotation = Quaternion.LookRotation(fireDirection);
        //Quaternion randomRotation = Random.rotation;

        //float randomX = Random.Range(0, 360);
        //float randomY = Random.Range(0, 360);
        //float randomZ = 0f;
        //Vector3 rot = new Vector3(randomX, randomY, randomZ);

        //Quaternion randomRot = new Quaternion(0f, randomY, 0f, 1f);
        ////fireRotation = Quaternion.RotateTowards(fireRotation, randomRotation, Random.Range(0.0f, maxSpreadAngle));
        //fireRotation = Quaternion.RotateTowards(fireRotation, randomRot, 1f);
        //if (Input.GetMouseButtonDown(1))
        //{
        //    // Instantiate bullet at fire point
        //    GameObject tempBullet = Instantiate(_bulletPrefab, _firePoint.transform.position, _firePoint.transform.rotation);
        //    tempBullet.transform.Rotate(randomRot.eulerAngles);
        //    print("FIRE ROTATION: " + fireRotation.eulerAngles);
        //    // Apply force to the bullet
        //    tempBullet.GetComponentInChildren<Rigidbody>().AddForce(transform.forward * _randomSpeed);
        //    // Increment shots counter in ScoreManager
        //    ScoreManager.Instance._shots++;
        //}
    }
}
