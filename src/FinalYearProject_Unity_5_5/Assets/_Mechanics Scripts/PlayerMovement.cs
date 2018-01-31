using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    public int _speed;
    public float _jumpHeight;
    // X and Y axis input
    float _horizontalAxis;
    float _verticalAxis;
    
    Rigidbody _rb;

    void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        if (GameMaster.Instance.GameState == GameMaster.GamePlayState.PLAYING)
            MovePlayer();
    }
    /// <summary>
    /// Move the player using keyboard.
    /// </summary>
    void MovePlayer()
    {
        // Set player object's y-axis rotation equal to the camera's yaw rotation
        transform.rotation = Quaternion.Euler(0f, Camera.main.GetComponent<MouseLookMovement>()._yawRotation, 0f);
        // Read horizontal and vertical input
        _horizontalAxis = Input.GetAxis("Horizontal") * _speed * Time.deltaTime;
        _verticalAxis = Input.GetAxis("Vertical") * _speed * Time.deltaTime;
        // Set motion vector
        Vector3 motionVector = new Vector3(_horizontalAxis, 0, _verticalAxis);
        // Move player object
        transform.Translate(motionVector);
        // Jump
        if (Input.GetKeyDown(KeyCode.Space))
        {
            _rb.AddForce(new Vector3(0, _jumpHeight, 0));
        }
    }
}
