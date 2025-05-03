using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private WeaponController WeaponController;

    public static Player instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

    }
    private void Start()
    {
        WeaponController = GetComponent<WeaponController>();

    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            WeaponController.Attack();
        }
    }
}
