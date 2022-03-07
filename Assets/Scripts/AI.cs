using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI : MonoBehaviour
{
    private const float MAX_FOWARD_ACCELERATION         = 20.0F;
    private const float MAX_BACKWARD_ACCELERATION       = 10.0f;
    private const float JUMP_ACCELERATION               = 500.0f;
    private const float Gravity_ACCELERATION            = 30.0f;
    private const float MAX_STRAFE_ACCELERATION         = 15.0F;
    private const float MAX_FOWARD_VELOCITY             = 6.0F;
    private const float MAX_BACKWARD_VELOCITY           = 4.0f;
    private const float MAX_STRAFE_VELOCITY             = 5.0F;
    private const float MAX_FALL_VELOCITY               = 50.0F;

    private CharacterController     _controller;
    private Vector3                 _motion;
    private Vector3                 _velocity;
    private Vector3                 _acceleration;
    private bool                    _jump;
    [Range(-1, 1)]
    [SerializeField] float          input_Foward = 0f;
    [Range(-1, 1)]
    [SerializeField] float          input_Strafe = 0f;
    [SerializeField] GameObject     player;
    [SerializeField] Transform      body;
    private bool                    seesPlayer = false;
    [SerializeField] Transform      lastPlayerLocation;

    private void Start()
    {
        _controller     = GetComponent<CharacterController>();
        _velocity       = Vector3.zero;
        _acceleration   = Vector3.zero;
        _jump           = false;
    }

    private void FixedUpdate()
    {
        GetPlayer();
        UpdateAcceleration();
        UpdateVelocity();
        UpdatePosition();
    }

    private void GetPlayer()
    {
        //vector to pthe player
        Vector3 vectToPlayer = player.transform.position - transform.position;

        float angleDeg = CalcAngle(vectToPlayer);

        //checks if player is in vision
        if(angleDeg < 45 && !CheckForObsticals(vectToPlayer))
            seesPlayer = true;
        else if(seesPlayer)
        {
            seesPlayer = false;
            lastPlayerLocation.position = Vector3.MoveTowards
                (lastPlayerLocation.position, player.transform.position, 1000);
        }
        
        //opninal to put angle normal
        Vector3 playerLocal = body.InverseTransformPoint(player.transform.position.x, 
            player.transform.position.y, player.transform.position.z);

        angleDeg = (playerLocal.x < body.forward.x) ? 360-angleDeg: angleDeg;

        if (seesPlayer)
            print(angleDeg);
        else
            print("?" + lastPlayerLocation.position + "?");
    }

    private bool CheckForObsticals(Vector3 vectToPlayer)
    {
        RaycastHit hit; 
        Physics.Raycast(body.position, vectToPlayer, out hit);
        print(hit.collider.tag);
        
        return !(hit.collider.tag == "Player");
    }

    private float CalcAngle(Vector3 vectToPlayer)
    {
        //calc angle to player in rads
        float angleRad = Mathf.Acos((body.forward.x * vectToPlayer.x +
            body.forward.z * vectToPlayer.z) / (Mathf.Sqrt(Mathf.Pow(body.forward.x, 2)
            + Mathf.Pow(body.forward.z, 2)) * Mathf.Sqrt(Mathf.Pow(vectToPlayer.x, 2)
            + Mathf.Pow(vectToPlayer.z, 2))));

        //convert to degrees
        return angleRad * Mathf.Rad2Deg;
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
        _acceleration.z = input_Foward;
        _acceleration.z *= (_acceleration.z > 0)? MAX_FOWARD_ACCELERATION : MAX_BACKWARD_ACCELERATION;

        _acceleration.x = input_Strafe * MAX_STRAFE_ACCELERATION;

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
            ? Mathf.Lerp(_velocity.z, 0.0f, 0.7f) : Mathf.Clamp(_velocity.z , -MAX_BACKWARD_VELOCITY , MAX_FOWARD_VELOCITY);
        _velocity.z = (Mathf.Abs(_velocity.z) < 0.1f) ? 0.0f : _velocity.z;

        _velocity.x = (_acceleration.x == 0 || _acceleration.x * _velocity.x < 0.0f) 
            ? Mathf.Lerp(_velocity.x, 0.0f, 0.7f) : Mathf.Clamp(_velocity.x , -MAX_STRAFE_VELOCITY , MAX_STRAFE_VELOCITY);
        _velocity.x = (Mathf.Abs(_velocity.x) < 0.1f) ? 0.0f : _velocity.x;

        _velocity.y = (_acceleration.y == 0.0f)? -0.1f : Mathf.Max(-MAX_FALL_VELOCITY , _velocity.y);
    }
    private void UpdatePosition() 
    {
        _motion = transform.TransformVector(_velocity * Time.fixedDeltaTime);
        
        _controller.Move(_motion);
    }

}

