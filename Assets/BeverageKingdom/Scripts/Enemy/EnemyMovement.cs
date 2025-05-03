using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public float moveSpeed = 2f;
    // public float detectionRange = 5f;
    private Transform player;
    public bool IsEnemeyInRange;
    public DetectionRange DetectionRange;

    void Awake()
    {
        DetectionRange.OnInRange += SetEnemyInRange;
        DetectionRange.OnOutRange += SetEnemyOutRange;
    }

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
        if (player != null && IsEnemeyInRange == true)
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

    void SetEnemyInRange()
    {
        IsEnemeyInRange = true;
    }

    void SetEnemyOutRange()
    {
        IsEnemeyInRange = false;
    }
}