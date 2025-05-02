using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public float moveSpeed = 2f;
    public float detectionRange = 5f;
    [SerializeField] private Transform player;

    void Start()
    {
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null)
        {
            player = playerObject.transform;
        }
        else
        {
            Debug.LogWarning("Không tìm thấy đối tượng có tag 'Player'");
        }
    }

    void Update()
    {
        if (player != null && IsPlayerInRange())
        {
            // Di chuyển về phía người chơi
            Vector3 directionToPlayer = (player.position - transform.position).normalized;
            transform.parent.position += directionToPlayer * moveSpeed * Time.deltaTime;
        }
        else
        {
            // Di chuyển qua trái
            transform.parent.position += Vector3.left * moveSpeed * Time.deltaTime;
        }
    }

    bool IsPlayerInRange()
    {
        return Vector3.Distance(transform.parent.position, player.position) <= detectionRange;
    }
}
