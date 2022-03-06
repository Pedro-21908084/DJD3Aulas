using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private const float MAX_FOWARD_ACCELERATION         = 20.0F;
    private const float MAX_BACKWARD_ACCELERATION       = 10.0f;
    private const float JUMP_ACCELERATION               = 500.0f;
    private const float Gravity_ACCELERATION            = 30.0f;
    private const float MAX_STRAFE_ACCELERATION         = 15.0F;
    private const float MAX_FOWARD_VELOCITY             = 4.0F;
    private const float MAX_BACKWARD_VELOCITY           = 2.0f;
    private const float MAX_STRAFE_VELOCITY             = 3.0F;
    private const float MAX_FALL_VELOCITY               = 50.0F;

    private CharacterController     _controller;
    private Vector3                 _motion;
    private Vector3                 _velocity;
    private Vector3                 _acceleration;
    private bool                    _jump;

    private void Start()
    {
        _controller     = GetComponent<CharacterController>();
        _velocity       = Vector3.zero;
        _acceleration   = Vector3.zero;
        _jump           = false;
    }

    private void FixedUpdate()
    {
        UpdateAcceleration();
        UpdateVelocity();
        UpdatePosition();
    }
    private void Update()
    {
        CheckForJump();
    }

    private void CheckForJump()
    {
        if (_controller.isGrounded && Input.GetButtonDown("Jump"))
            _jump = true;
    }

    private void UpdateAcceleration() 
    {
        _acceleration.z = Input.GetAxis("Foward");
        _acceleration.z *= (_acceleration.z > 0)? MAX_FOWARD_ACCELERATION : MAX_BACKWARD_ACCELERATION;

        _acceleration.x = Input.GetAxis("Strafe") * MAX_STRAFE_ACCELERATION;

        if (_jump)
        {
            _acceleration.y = JUMP_ACCELERATION;
            _jump = false;
        }
        else if (_controller.isGrounded)
            _acceleration.y = 0.0f;
        else
            _acceleration.y = -Gravity_ACCELERATION;
            
    }

    private void UpdateVelocity() 
    {
        _velocity += _acceleration * Time.fixedDeltaTime;

        _velocity.z = (_acceleration.z == 0 || _acceleration.z * _velocity.z < 0.0f) 
            ? Mathf.Lerp(_velocity.z, 0.0f, 0.5f) : Mathf.Clamp(_velocity.z , -MAX_BACKWARD_VELOCITY , MAX_FOWARD_VELOCITY);
        _velocity.z = (Mathf.Abs(_velocity.z) < 0.1f) ? 0.0f : _velocity.z;

        _velocity.x = (_acceleration.x == 0 || _acceleration.x * _velocity.x < 0.0f) 
            ? Mathf.Lerp(_velocity.x, 0.0f, 0.5f) : Mathf.Clamp(_velocity.x , -MAX_STRAFE_VELOCITY , MAX_STRAFE_VELOCITY);
        _velocity.x = (Mathf.Abs(_velocity.x) < 0.1f) ? 0.0f : _velocity.x;

        _velocity.y = (_acceleration.y == 0.0f)? -0.1f : Mathf.Max(-MAX_FALL_VELOCITY , _velocity.y);
    }
    private void UpdatePosition() 
    {
        _motion = transform.TransformVector(_velocity * Time.fixedDeltaTime);
        
        _controller.Move(_motion);
    }

}
