// WeaponController.cs
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class WeaponController : MonoBehaviour
{
    [Header("Danh sách ScriptableObject Weapon")]
    public List<Weapon> weapons;
    private int currentIndex = 0;

    [Header("Nơi xuất phát đạn/đòn đánh")]
    public Transform firePoint;

    public Weapon CurrentWeapon => weapons.Count > 0 ? weapons[currentIndex] : null;


    void Start()
    {
        if (weapons == null || weapons.Count == 0)
            Debug.LogWarning("Chưa có weapon nào gán vào WeaponController!");
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
            SwitchWeapon(-1);
        else if (Input.GetKeyDown(KeyCode.E))
            SwitchWeapon(+1);

        // Fire khi bấm chuột trái
        if (Input.GetButton("Fire1") && CurrentWeapon != null)
        {
            // CurrentWeapon.Attack();
        }
    }

    public void SwitchWeapon(int dir)
    {
        if (weapons == null || weapons.Count == 0) return;

        currentIndex = (currentIndex + dir + weapons.Count) % weapons.Count;
        Player.instance.SwapAnimatorController(currentIndex);
    }
    public void Attack()
    {
        weapons[currentIndex].Attack();
    }
    
}
