using UnityEngine;
using System.Collections;
public class MouseLookMovement : MonoBehaviour
{
    [Range(5f, 20f)]
    public float _lookSensitivity;
    // Rotation around perpendicular axis (Rotate on camera's up vector)
    [System.NonSerialized]
    public float _yawRotation;
    // Rotation around lateral axis (Rotate on the cross product of the other camera's up and direction vector)
    private float _pitchRotation;
    // Clamping variables pitch rotation
    [Range(-360,360)]
    public float _maxPitch, _minPitch;
    // Smooth damping variables
    public float _smoothPitchRotation;
    float _smoothYawRotation;
    // Velocity of pitch rotation
    public float _pitchVelocity;
    // Velocity of yaw rotation
    public float _yawVelocity;
    // Time in seconds used to smooth 
    [Range(0.0f, 0.5f)]
    public float _smoothTime;

    // Update is called once per frame
    void Update()
    {
        if (GameMaster.Instance.GameState == GameMaster.GamePlayState.PLAYING)
            RotateCamera();
    }
    /// <summary>
    /// Rotate the camera in the scene by applying yaw and pitch rotation.
    /// </summary>
    void RotateCamera()
    {
        // Read mouse X and mouse Y input
        _yawRotation += Input.GetAxis("Mouse X") * _lookSensitivity;
        _pitchRotation -= Input.GetAxis("Mouse Y") * _lookSensitivity;
        // Clamp pitch rotation
        _pitchRotation = Mathf.Clamp(_pitchRotation, -_maxPitch, -_minPitch);
        // Smooth pitch and yaw rotation
        _smoothPitchRotation = Mathf.SmoothDamp(_smoothPitchRotation, _pitchRotation, ref _pitchVelocity, _smoothTime);
        _smoothYawRotation = Mathf.SmoothDamp(_smoothYawRotation, _yawRotation, ref _yawVelocity, _smoothTime);
        // Apply rotation 
        transform.rotation = Quaternion.Euler(_smoothPitchRotation, _smoothYawRotation, 0);
    }
}
