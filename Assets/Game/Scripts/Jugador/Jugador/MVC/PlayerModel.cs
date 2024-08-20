using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

public class PlayerModel : MonoBehaviour 
{
    public event Action OnMovement = delegate { };
    public event Action OnLifeChange = delegate { };
    public event Action OnJump = delegate { };
    public event Action OnDead = delegate { };
    public event Action OnDash = delegate { };

    [Header("References")]
    private Player _p;
    private CharacterController characterController;
    public float _startLife;
    public float _currentLife;

    [Header("Movement")]
    private float _speed;
    private Vector3 _pos;

    [Header("Jump")]
    private float _gravity;
    private float _jumpForce;
    private float _fallVelocity;
 
    [Header("Dash")]
    public bool dash = false;
    private Vector3 _initialScale;
    private Vector3 _finalScale = new Vector3(0.5f, 0.5f, 0.5f);
    private float _dashForce;
    private int _dashDuration;
    public float tiempoUltimoDash;
    public bool canDash = true;
    public float enfriamientoDash = 5f;

    public bool floor;
    private bool step = true;
    private float floorDistance = 0.1f;
      

    public PlayerModel(Player user)
    {
        characterController = user.GetComponent<CharacterController>();

        _currentLife = _startLife = user.StartLife;

        _speed = user.Speed;

        _jumpForce = user.JumpForce;

        _dashForce = user.DashForce;

        _dashDuration = user.DashDuration;

        _gravity = user.Gravity;

        _p = user;

        _initialScale = _p.transform.localScale;
    }
    public void UpdateSpeed(float newSpeed)
    {
        _speed = newSpeed;
    }


    public void Movement(Vector3 direction)
    {
        if (direction.sqrMagnitude > 1)
            direction.Normalize();
        
        Vector3 movement = _p.transform.right * direction.x + _p.transform.forward * direction.z;
        
        if (direction.sqrMagnitude > .1f && step == true && !dash)
        {
            step = false;
            OnMovement();
            ResetStepSound();
        }

        characterController.Move(movement * (_speed * Time.deltaTime));
    }

    public async void ResetStepSound()
    {
        await Task.Delay(400);
        step = true;
    }

    public void Gravity()
    {
        if (characterController.isGrounded)
        {
            _fallVelocity = -_gravity * Time.deltaTime;
            _pos.y = _fallVelocity;
        }
        else
        {
            _fallVelocity -= _gravity * Time.deltaTime;
            _pos.y = _fallVelocity;      
        }

        _pos.y = _fallVelocity * Time.deltaTime * 100;
        characterController.Move(_pos);
    }

    public void Jump()
    {
        if (floor) 
        {
            _fallVelocity = _jumpForce;
            _pos.y = _fallVelocity;
            characterController.Move(_pos);
            SoundFXManager.Instance.PlaySoundFXClip(_p.JumpSound, _p.transform, .65f);
        }
        else return;
        
        OnJump();
    }

    public async void Dash()
    {
        if (!canDash) return;
        canDash = false;
        tiempoUltimoDash = Time.time;

        _p.transform.localScale = _finalScale;
        dash = true;

        _speed *= _dashForce;

        OnDash();

        await Task.Delay(_dashDuration);

        ReiniciarElDash();
    }

    private void ReiniciarElDash()
    {
        dash = false;
        _speed = _p.Speed;
        _p.transform.localScale = _initialScale;
        canDash = true;
    }

    public void TakeDamage(float dmg)
    {
        _currentLife -= dmg;

        OnLifeChange();

        if (_currentLife <= 0)
        {
            Dead();
        }
    }

    void Dead()
    {
        OnDead();
    }

    public void CheckGround()
    {
        floor = Physics.CheckSphere(_p.verifyFloor.position, floorDistance, _p.floorLayer);
    }
}
