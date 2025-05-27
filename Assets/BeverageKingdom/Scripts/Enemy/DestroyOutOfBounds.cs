using UnityEngine;

public class DestroyOutOfBounds : MonoBehaviour
{
    [Header("Boundary Limits")]
    [SerializeField] private float leftLimit = -20f;
    [SerializeField] private float rightLimit = 20f;
    [SerializeField] private float topLimit = 20f;
    [SerializeField] private float bottomLimit = -20f;

    private void Update()
    {
        Vector3 pos = transform.position;
        
        if (pos.x < leftLimit || pos.x > rightLimit || 
            pos.y > topLimit || pos.y < bottomLimit)
        {
            Debug.Log($"Enemy {gameObject.name} destroyed - Out of bounds at position {pos}");
            Destroy(gameObject);
        }
    }

    // Draw the boundaries in the editor for easy visualization
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Vector3 topLeft = new Vector3(leftLimit, topLimit, 0);
        Vector3 topRight = new Vector3(rightLimit, topLimit, 0);
        Vector3 bottomLeft = new Vector3(leftLimit, bottomLimit, 0);
        Vector3 bottomRight = new Vector3(rightLimit, bottomLimit, 0);

        Gizmos.DrawLine(topLeft, topRight);
        Gizmos.DrawLine(topRight, bottomRight);
        Gizmos.DrawLine(bottomRight, bottomLeft);
        Gizmos.DrawLine(bottomLeft, topLeft);
    }
}
