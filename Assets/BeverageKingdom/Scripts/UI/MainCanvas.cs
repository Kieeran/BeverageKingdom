using UnityEngine;

public class MainCanvas : MonoBehaviour
{
    [SerializeField] Joystick _joystick;

    public Joystick GetJoystick() { return _joystick; }
}
