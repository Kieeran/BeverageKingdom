using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JoystickMove : MonoBehaviour
{
    public Vector2 move;

    public Joystick Joystick;

    public void SetJoystick(Joystick joystick)
    {
        Joystick = joystick;
    }

    void Update()
    {
        if (Joystick == null) return;

        if (Joystick.Direction.y != 0)
        {
            move = new Vector2(
                Joystick.Direction.x,
                Joystick.Direction.y
            );
        }

        else
        {
            move = Vector2.zero;
        }
    }
}