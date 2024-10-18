using UnityEngine;
using System.Collections.Generic;

public class BulletButton : MonoBehaviour
{
    public List<GameObject> objectsToActivate; // The object to activate/deactivate
    private bool hitByBullet;

    private bool isPressed = false; // Tracks if the button is pressed
    private bool hasTriggered = false; // Ensures the button can only be pressed once per bullet

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
        // Check if the button has been hit by a bullet
        if (hitByBullet && !hasTriggered)
        {
            foreach (GameObject obj in objectsToActivate)
            {
                if (obj != null)
                {
                    obj.SetActive(!obj.activeSelf); // Toggle active state
                }
            }

            FindObjectOfType<AudioManager>().Play("ButtonSFX");

            // Toggle the button's pressed state
            isPressed = !isPressed;

            // Change the button's sprite based on the pressed state
            if (spriteRenderer != null)
            {
                spriteRenderer.sprite = isPressed ? pressedSprite : unpressedSprite;
            }

            // Set hasTriggered to true to prevent further presses
            hasTriggered = true;

            // Optional: Disable the collider to prevent further hits
            GetComponent<Collider2D>().enabled = false; // Disable the button's collider if needed
        }
    }

    // When a bullet enters the button's trigger area
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            // Only activate the button if it hasn't been triggered before
            if (!hasTriggered)
            {
                hitByBullet = true; // Set hitByBullet to true to indicate a bullet has hit the button
            }
        }
    }
}
