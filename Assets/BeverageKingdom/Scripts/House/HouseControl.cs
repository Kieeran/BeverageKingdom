using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HouseControl : MonoBehaviour
{
    public int AllHouseHP;

    public List<House> Houses;

    void Start()
    {
        foreach (House house in Houses)
        {
            house.HP = AllHouseHP;
        }
    }
}
