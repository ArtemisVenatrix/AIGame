using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*public class Controller : MonoBehaviour
{
    //unity controls and constants input
    [SerializeField] public float accelerationMod;
    [SerializeField] public float xAxisSensitivity;
    [SerializeField] public float yAxisSensitivity;
    [SerializeField] public float zAxisSensitivity;
    [SerializeField] public string forwards;
    [SerializeField] public string backwards;
    [SerializeField] public string left;
    [SerializeField] public string right;
    [SerializeField] public string up;
    [SerializeField] public string down;
    [SerializeField] public string rotateRight;
    [SerializeField] public string rotateLeft;
    
    private Vector3 moveSpeed;
    private float zTheta;
    
    void Start()
    {
        moveSpeed = new Vector3();
    }

    // Update is called once per frame
    void Update()
    {
        //acceleration this iteration
        Vector3 acceleration = new Vector3();
        Vector3 deceleration = new Vector3();
        
        //mouse input
        float rotationHorizontal = xAxisSensitivity * Input.GetAxis("Mouse X");
        float rotationVertical = yAxisSensitivity * Input.GetAxis("Mouse Y");
        zTheta = 0;

        if (Input.GetKey(rotateLeft))
        {
            zTheta += zAxisSensitivity;
        }

        if (Input.GetKey(rotateRight))
        {
            zTheta -= zAxisSensitivity;
        }

        //applying mouse rotation
        transform.localEulerAngles = transform.localEulerAngles + new Vector3 (-rotationVertical, rotationHorizontal, zTheta);

        //Debug.Log(moveSpeed);
        deceleration = moveSpeed;
        Debug.Log(deceleration);
        deceleration.Normalize();
        deceleration *= accelerationMod;
        deceleration *= -1;
        
        //key input detection
        if (Input.GetKey(forwards))
        {
            //Debug.Log(forwards);
            acceleration += transform.forward;
            deceleration -= -transform.forward * accelerationMod;
            //Debug.Log(acceleration);
        }
        
        if (Input.GetKey(left))
        {
            //Debug.Log(left);
            acceleration += -transform.right;
            deceleration -= transform.right * accelerationMod;
        }

        if (Input.GetKey(backwards))
        {
            //Debug.Log(backwards);
            acceleration += -transform.forward;
            deceleration -= transform.forward * accelerationMod;
        }

        if (Input.GetKey(right))
        {
            //Debug.Log(right);
            acceleration += transform.right;
            deceleration -= -transform.right * accelerationMod;
        }

        if (Input.GetKey(up))
        {
            //Debug.Log(up);
            acceleration += transform.up;
            deceleration -= -transform.up * accelerationMod;
        }

        if (Input.GetKey(down))
        {
            //Debug.Log(down);
            acceleration += -transform.up;
            deceleration -= transform.up * accelerationMod;
        }

        Debug.Log("acceleration: " + acceleration);
        Debug.Log("deceleration: " + deceleration);
        acceleration *= accelerationMod;
        acceleration += deceleration;
        moveSpeed += acceleration;
        
        if (Math.Abs(moveSpeed.x) < deceleration.x)
        {
            moveSpeed.x = 0;
        }

        if (Math.Abs(moveSpeed.y) < deceleration.y)
        {
            moveSpeed.y = 0;
        }

        if (Math.Abs(moveSpeed.z) < deceleration.z)
        {
            moveSpeed.z = 0;
        }
        
        transform.position += moveSpeed;
    }
}*/

public class MovementController : MonoBehaviour
{
    [Header("Constants")]

    //unity controls and constants input
    public float AccelerationMod;
    public float XAxisSensitivity;
    public float YAxisSensitivity;
    public float DecelerationMod;

    [Space]

    [Range(0, 89)] public float MaxXAngle = 60f;

    [Space]

    public float MaximumMovementSpeed = 1f;

    [Header("Controls")]

    public KeyCode Forwards = KeyCode.W;
    public KeyCode Backwards = KeyCode.S;
    public KeyCode Left = KeyCode.A;
    public KeyCode Right = KeyCode.D;
    public KeyCode Up = KeyCode.Q;
    public KeyCode Down = KeyCode.E;

    public bool LockInput { get; private set; }
    
    private Vector3 _moveSpeed;

    public void SetLockInput(bool value)
    {
        LockInput = value;
    }
    
    private void Start()
    {
        _moveSpeed = Vector3.zero;
        LockInput = false;
    }

    // Update is called once per frame
    private void Update()
    {
        Vector3 acceleration = new Vector3();
        if (!LockInput)
        {
            HandleMouseRotation();
            
            acceleration = HandleKeyInput();
        }
        
        _moveSpeed += acceleration;

        HandleDeceleration(acceleration);

        // clamp the move speed
        if(_moveSpeed.magnitude > MaximumMovementSpeed)
        {
            _moveSpeed = _moveSpeed.normalized * MaximumMovementSpeed;
        }

        transform.Translate(_moveSpeed);
    }

    private Vector3 HandleKeyInput()
    {
        var acceleration = Vector3.zero;

        //key input detection
        if (Input.GetKey(Forwards))
        {
            acceleration.z += 1;
        }

        if (Input.GetKey(Backwards))
        {
            acceleration.z -= 1;
        }

        if (Input.GetKey(Left))
        {
            acceleration.x -= 1;
        }

        if (Input.GetKey(Right))
        {
            acceleration.x += 1;
        }

        if (Input.GetKey(Up))
        {
            acceleration.y += 1;
        }

        if (Input.GetKey(Down))
        {
            acceleration.y -= 1;
        }

        return acceleration.normalized * AccelerationMod;
    }

    private float _rotationX;

    private void HandleMouseRotation()
    {
        //mouse input
        var rotationHorizontal = XAxisSensitivity * Input.GetAxis("Mouse X");
        var rotationVertical = YAxisSensitivity * Input.GetAxis("Mouse Y");

        //applying mouse rotation
        // always rotate Y in global world space to avoid gimbal lock
        transform.Rotate(Vector3.up * rotationHorizontal, Space.World);

        var rotationY = transform.localEulerAngles.y;

        _rotationX += rotationVertical;
        _rotationX = Mathf.Clamp(_rotationX, -MaxXAngle, MaxXAngle);

        transform.localEulerAngles = new Vector3(-_rotationX, rotationY, 0);
    }

    private void HandleDeceleration(Vector3 acceleration)
    {
        //deceleration functionality
        if (Mathf.Approximately(Mathf.Abs(acceleration.x), 0))
        {
            if (Mathf.Abs(_moveSpeed.x) < DecelerationMod)
            {
                _moveSpeed.x = 0;
            }
            else
            {
                _moveSpeed.x -= DecelerationMod * Mathf.Sign(_moveSpeed.x);
            }
        }

        if (Mathf.Approximately(Mathf.Abs(acceleration.y), 0))
        {
            if (Mathf.Abs(_moveSpeed.y) < DecelerationMod)
            {
                _moveSpeed.y = 0;
            }
            else
            {
                _moveSpeed.y -= DecelerationMod * Mathf.Sign(_moveSpeed.y);
            }
        }

        if (Mathf.Approximately(Mathf.Abs(acceleration.z), 0))
        {
            if (Mathf.Abs(_moveSpeed.z) < DecelerationMod)
            {
                _moveSpeed.z = 0;
            }
            else
            {
                _moveSpeed.z -= DecelerationMod * Mathf.Sign(_moveSpeed.z);
            }
        }
    }
}
