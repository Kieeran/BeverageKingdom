using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    ItemDespawn itemDespawn;

    private float height = 0f;
    private float verticalSpeed = 5f;
    private float gravity = -9.8f;
    private Vector3 basePosition;
    private void Start()
    {
        itemDespawn = GetComponent<ItemDespawn>();
       
    }
    private void OnEnable()
    {
        basePosition = transform.position;
        height = 0f;
        verticalSpeed = 5f; // lực bay lên
        StartCoroutine(SimulateVerticalBounce());
    }

    IEnumerator SimulateVerticalBounce()
    {
        while (true)
        {
            verticalSpeed += gravity * Time.deltaTime;
            height += verticalSpeed * Time.deltaTime;

            if (height <= 0f)
            {
                height = 0f;
                break; // chạm đất -> kết thúc
            }

            // Di chuyển sprite lên theo trục Y (hoặc Z nếu dùng layer sorting)
            transform.position = basePosition + new Vector3(0, height, 0);

            yield return null;
        }

        transform.position = basePosition; // đảm bảo trở lại vị trí gốc
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("BB"))
        {
            PickUp();
            itemDespawn.DeSpawnObj(); // Call the despawn method to handle item removal
        }
            
    }
    protected virtual void PickUp()
    {

        itemDespawn.DeSpawnObj(); // Call the despawn method to handle item removal
        Destroy(gameObject); // Destroy the item after pickup
    }
}
