using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JoystickMove : MonoBehaviour
{
    public Joystick movementJoystick;
    public float playerSpeed;
    public Rigidbody2D rb2d;

    void FixedUpdate()
    {
        if (movementJoystick.Direction.y != 0)
        {
            rb2d.velocity = new Vector2(
                movementJoystick.Direction.x * playerSpeed,
                movementJoystick.Direction.y * playerSpeed
            );
        }

        else
        {
            rb2d.velocity = Vector2.zero;
        }
    }
}