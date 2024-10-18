using UnityEngine;

public class PressurePlate : MonoBehaviour
{
    public GameObject objectToMove; // The object to move (e.g., a door)
    public Vector3 moveDirection = new Vector3(0, 1, 0); // Direction to move the object (default: upward)
    public float moveDistance = 2f; // Distance to move the object
    public float moveSpeed = 2f; // Speed at which the object moves

    private Vector3 initialPosition; // Store the initial position of the object
    private Vector3 targetPosition; // Store the target position
    private bool isActivated = false; // To track if the pressure plate is pressed
    private bool isMoving = false; // To track if the object is moving

    private SpriteRenderer spriteRenderer; // To change appearance when activated
    public Sprite activatedSprite; // The sprite when the pressure plate is pressed
    public Sprite deactivatedSprite; // The sprite when the pressure plate is not pressed

    private void Start()
    {
        if (objectToMove != null)
        {
            initialPosition = objectToMove.transform.position;
            targetPosition = initialPosition + moveDirection.normalized * moveDistance; // Calculate target position
        }

        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        // Move the object if it's supposed to move
        if (isMoving)
        {
            MoveObject();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") || other.CompareTag("MovableObject"))
        {
            ActivatePlate();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player") || other.CompareTag("MovableObject"))
        {
            DeactivatePlate();
        }
    }

    private void ActivatePlate()
    {
        if (!isActivated)
        {
            isActivated = true;
            spriteRenderer.sprite = activatedSprite;
            isMoving = true; // Start moving the object
        }
    }

    private void DeactivatePlate()
    {
        if (isActivated)
        {
            isActivated = false;
            spriteRenderer.sprite = deactivatedSprite;
            isMoving = true; // Move the object back
        }
    }

    private void MoveObject()
    {
        if (isActivated)
        {
            // Move the object towards the target position
            objectToMove.transform.position = Vector3.MoveTowards(objectToMove.transform.position, targetPosition, moveSpeed * Time.deltaTime);

            // Stop moving once it reaches the target position
            if (objectToMove.transform.position == targetPosition)
            {
                isMoving = false;
            }
        }
        else
        {
            // Move the object back to its initial position
            objectToMove.transform.position = Vector3.MoveTowards(objectToMove.transform.position, initialPosition, moveSpeed * Time.deltaTime);

            // Stop moving once it reaches the initial position
            if (objectToMove.transform.position == initialPosition)
            {
                isMoving = false;
            }
        }
    }
}