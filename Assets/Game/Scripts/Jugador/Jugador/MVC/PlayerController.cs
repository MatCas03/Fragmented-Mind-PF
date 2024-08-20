using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController
{
    private PlayerModel _m;

    private Vector3 _direction;

    public PlayerController(PlayerModel model)
    {
        _m = model;
        _direction = Vector3.zero;
    }

    public void UpdateKeys()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift) && _m.canDash &&  !_m.dash)
            _m.Dash();

        if (Input.GetKey(KeyCode.Space))
        {
            _m.Jump();
        }
    }

    public void Keys()
    {
        _direction.x = Input.GetAxis("Horizontal");
        _direction.z = Input.GetAxis("Vertical");

        _m.Movement(_direction);
    }

}
