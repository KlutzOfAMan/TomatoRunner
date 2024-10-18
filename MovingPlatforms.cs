using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatforms : MonoBehaviour
{
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
            }
            else
            {
                target = pointB.position;
                movingToB = true;
            }
        }

    }
    // Only make the player a child when they land on the platform (i.e., from above)
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // Check if the player is above the platform when the collision occurs
            if (collision.contacts[0].normal.y < -0.5f)
            {
                collision.transform.SetParent(transform);
            }
        }
    }

    // When the player leaves the platform, remove the parent-child relationship
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.transform.SetParent(null);
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
}
