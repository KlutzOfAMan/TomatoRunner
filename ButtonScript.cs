using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonScript : MonoBehaviour
{
    
    public GameObject objectToActivate; // The object to activate/deactivate
    public KeyCode interactionKey = KeyCode.E; // The key to press to activate the button
    private bool playerInRange; // Tracks if the player is in range of the button

    private bool isPressed = false; // Tracks if the button is pressed

    public Sprite unpressedSprite; // Sprite when button is not pressed
    public Sprite pressedSprite; // Sprite when button is pressed
    private SpriteRenderer spriteRenderer; // Reference to the SpriteRenderer component

    void Start()
    {
        // Get the SpriteRenderer component on this GameObject
        spriteRenderer = GetComponent<SpriteRenderer>();

        // Set the default sprite to unpressedSprite
        if (spriteRenderer != null && unpressedSprite != null)
        {
            spriteRenderer.sprite = unpressedSprite;
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Check if the player is in range and presses the interaction key
        if (playerInRange && Input.GetKeyDown(interactionKey))
        {
            // Toggle the active state of the target object
            if (objectToActivate != null)
            {
                objectToActivate.SetActive(!objectToActivate.activeSelf);
                FindObjectOfType<AudioManager>().Play("ButtonSFX");

                // Toggle the button's pressed state
                isPressed = !isPressed;

                // Change the button's sprite based on the pressed state
                if (spriteRenderer != null)
                {
                    spriteRenderer.sprite = isPressed ? pressedSprite : unpressedSprite;
                }
            }

            // Optional: Add more logic here if needed
        }
    }

    // When the player enters the button's trigger area
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            playerInRange = true; // Player is in range of the button
        }
    }

    // When the player exits the button's trigger area
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            playerInRange = false; // Player is no longer in range
        }
    }
}