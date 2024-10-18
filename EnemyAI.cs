using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    private Rigidbody2D rb;
    public Transform pointA; // First point
    public Transform pointB; // Second point
    public float speed = 3f; // Speed of the platform
    private Vector3 target; // Current target point
    private bool movingToB = true; // Track which direction platform is moving

    public Color gizmoColor = Color.red; // Color for the Gizmo in the editor
    public float gizmoSize = 0.2f; // Size of the Gizmo markers

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        // Set initial target to point B
        target = pointB.position;
    }

    // Update is called once per frame
    void Update()
    {

        // Move the platform toward the target point
        transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);

        // Check if the platform reached its target point
        if (Vector3.Distance(transform.position, target) < 0.1f)
        {
            // Switch target between point A and B
            if (movingToB)
            {
                target = pointA.position;
                movingToB = false;
                Flip(); // Flip the sprite to face the other direction
            }
            else
            {
                target = pointB.position;
                movingToB = true;
                Flip(); // Flip the sprite to face the other direction

            }
        }
  
    }

    private void OnDrawGizmos()
    {
        if (pointA != null && pointB != null)
        {
            // Set the Gizmo color
            Gizmos.color = gizmoColor;

            // Draw spheres at pointA and pointB
            Gizmos.DrawSphere(pointA.position, gizmoSize);
            Gizmos.DrawSphere(pointB.position, gizmoSize);

            // Draw a line between pointA and pointB
            Gizmos.DrawLine(pointA.position, pointB.position);
        }
    }
    void Flip()
    {
        Vector3 localScale = transform.localScale;
        localScale.x *= -1; // Invert the x-axis scale
        transform.localScale = localScale;
    }

}
