using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JoystickMove : MonoBehaviour
{
    [SerializeField] float _playerSpeed;
    [SerializeField] Rigidbody2D _rb2d;

    Joystick _joystick;

    public void SetJoystick(Joystick joystick)
    {
        _joystick = joystick;
    }

    void FixedUpdate()
    {
        if (_joystick == null) return;

        if (_joystick.Direction.y != 0)
        {
            _rb2d.velocity = new Vector2(
                _joystick.Direction.x * _playerSpeed,
                _joystick.Direction.y * _playerSpeed
            );
        }

        else
        {
            _rb2d.velocity = Vector2.zero;
        }
    }
}