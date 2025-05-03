using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputMangager : MonoBehaviour
{
    private static InputMangager instance;
    public static InputMangager Instance { get => instance; }

    [SerializeField] private Vector2 mousePos;
    public Vector2 MousePos { get => mousePos; }
    [SerializeField] private float onfiring;
    public float OnFiring { get => onfiring; }

    [SerializeField] private float horizontal, vertical;
    public float Horizontal { get => horizontal; }
    public float Vertical { get => vertical; }

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void FixedUpdate()
    {
        this.GetMousePosition();

    }
    private void Update()
    {
        GetmouseDown();
        GetAxis();
    }
    protected virtual void GetMousePosition()
    {
        this.mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }
    protected virtual void GetmouseDown()
    {
        onfiring = Input.GetAxis("Fire1");
    }
    protected virtual void GetAxis()
    {
        horizontal = Input.GetAxisRaw("Horizontal");

        vertical = Input.GetAxisRaw("Vertical");
    }
    public bool GetKeyToJump() => Input.GetKeyDown(KeyCode.UpArrow);
    public bool GetKeyToAttack() => Input.GetMouseButtonDown(0);
}
