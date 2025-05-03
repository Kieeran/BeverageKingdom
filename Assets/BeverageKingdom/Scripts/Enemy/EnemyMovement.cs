using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : Movement
{
    public float moveSpeed = 2f;
    [HideInInspector]
    public Transform Target;
    [HideInInspector]
    public bool IsFriendlyInRange;
    public DetectionRange DetectionRange;

    void Awake()
    {
        DetectionRange.OnInRange += SetFriendlyInRange;
        DetectionRange.OnOutRange += SetFriendlyOutRange;
    }

    void Update()
    {
        if (Target != null && IsFriendlyInRange == true)
        {
            Vector3 directionToPlayer = (Target.position - transform.position).normalized;
            transform.parent.position += directionToPlayer * moveSpeed * Time.deltaTime;
        }
        else
        {
            // Di chuyển qua trái
            transform.parent.position += Vector3.left * moveSpeed * Time.deltaTime;
        }
    }

    void SetFriendlyInRange(Transform transform)
    {
        IsFriendlyInRange = true;
        Target = transform;
    }

    void SetFriendlyOutRange()
    {
        IsFriendlyInRange = false;
        Target = null;
    }
}