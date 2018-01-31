using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    public int _speed;
    public float _jumpHeight;
    // X and Y axis input
    float _horizontalForce;
    float _verticalForce;

    Rigidbody _rb;

    void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        //// Set player object's y-axis rotation equal to the camera's yaw rotation
        transform.rotation = Quaternion.Euler(0f, Camera.main.GetComponent<MouseLookMovement>()._yawRotation, 0f);
        // Read horizontal and vertical input
        _horizontalForce = Input.GetAxis("Horizontal") * _speed;
        _verticalForce = Input.GetAxis("Vertical") * _speed;
        // Move player object
        transform.Translate(new Vector3(_horizontalForce, 0, _verticalForce));
        //MovePlayer();
    }

    void MovePlayer()
    {
        //if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S))
        //    _rb.velocity = new Vector3(_speed * _horizontalForce,0,_speed * _verticalForce);
        //if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
        //    _rb.velocity = new Vector3(_speed * _horizontalForce, 0, _speed * _verticalForce);
        //if (Input.GetAxis("Jump") > 0)
        //{
        //    print("jump");
        //    _rb.AddForce(new Vector3(0, _jumpHeight, 0));
        //    //_rb.velocity = new Vector3(_rb.velocity.x, _jumpHeight, _rb.velocity.z);
        //}
        //if (Input.GetKey(KeyCode.W))
        //    GetComponent<Rigidbody>().AddForce(GetComponent<Rigidbody>().position
        //        + Vector3.forward * _speed * 0.5f * Time.deltaTime);
        //if (Input.GetKey(KeyCode.S))
        //    GetComponent<Rigidbody>().AddForce(GetComponent<Rigidbody>().position
        //        - Vector3.forward * _speed * 0.5f * Time.deltaTime);
        //if (Input.GetKey(KeyCode.A))
        //    GetComponent<Rigidbody>().AddForce(GetComponent<Rigidbody>().position
        //        - Vector3.right * _speed * 0.5f * Time.deltaTime);
        //if (Input.GetKey(KeyCode.D))
        //    GetComponent<Rigidbody>().AddForce(GetComponent<Rigidbody>().position
        //        + Vector3.right * _speed * 0.5f * Time.deltaTime);
    }
}
