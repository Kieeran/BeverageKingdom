using UnityEngine;

public class VillagerMovement : MonoBehaviour
{
    [HideInInspector]
    public float moveSpeed = 2f;
    [HideInInspector]
    public bool IsEnemyInRange;
    public DetectionRange DetectionRange;
    [HideInInspector]
    public Transform Target;

    void Awake()
    {
        DetectionRange.OnInRange += SetEnemyInRange;
        DetectionRange.OnOutRange += SetEnemyOutRange;
    }

    void Update()
    {
        if (Target != null && IsEnemyInRange == true)
        {
            Vector3 directionToPlayer = (Target.position - transform.position).normalized;
            transform.parent.position += directionToPlayer * moveSpeed * Time.deltaTime;
        }
    }

    void SetEnemyInRange(Transform transform)
    {
        IsEnemyInRange = true;
        Target = transform;
    }

    void SetEnemyOutRange()
    {
        IsEnemyInRange = false;
        Target = null;
    }
}