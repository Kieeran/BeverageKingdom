using UnityEngine;

public class Villager : MonoBehaviour
{
    private Vector3 targetPosition;
    float _moveSpeed = 5f;

    public void SetTargetPosition(Vector3 pos)
    {
        targetPosition = pos;
    }

    void Update()
    {
        transform.position = Vector3.MoveTowards(
            transform.position,
            targetPosition,
            _moveSpeed * Time.deltaTime
        );
    }
}
