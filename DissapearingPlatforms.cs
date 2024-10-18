using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DissapearingPlatforms : MonoBehaviour
{

    public Sprite Visible; // Sprite when button is not pressed
    public Sprite Unvisible; // Sprite when button is pressed
    private SpriteRenderer spriteRenderer; // Reference to the SpriteRenderer component
    private BoxCollider2D boxCollider;

    
    private Material originalMaterial; // The material that was in use, when the script started.
    private Coroutine flashRoutine; // The currently running coroutine.


    [Tooltip("Material to switch to during the flash.")]
    [SerializeField] private Material flashMaterial;

    [Tooltip("Duration of the flash.")]
    [SerializeField] private float duration;


    // Start is called before the first frame update
    void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        // Get the SpriteRenderer component on this GameObject
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (spriteRenderer.sprite == null && Visible != null)
        {
            spriteRenderer.sprite = Visible;
        }
        originalMaterial = spriteRenderer.material;
    }

  

    // Only make the player a child when they land on the platform (i.e., from above)
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player")) //If in contact with player
        {
            Flash();

        }
    }

    private IEnumerator Reappearing()
    {
        yield return new WaitForSeconds(3f);

        if (boxCollider != null)
        {
            // Disable the BoxCollider2D component
            boxCollider.enabled = true;
            ChangeSprite();

        }


    }

    public void ChangeSprite()
    {
        if (spriteRenderer.sprite == Visible)
        {
            spriteRenderer.sprite = Unvisible;
        }
        else if (spriteRenderer.sprite == Unvisible)
        {
            // If the current sprite is spriteB, change to spriteA
            spriteRenderer.sprite = Visible;
        }
    }

    public void Flash()
    {
        // If the flashRoutine is not null, then it is currently running.
        if (flashRoutine != null)
        {
            // In this case, we should stop it first.
            // Multiple FlashRoutines the same time would cause bugs.
            StopCoroutine(flashRoutine);
        }

        // Start the Coroutine, and store the reference for it.
        flashRoutine = StartCoroutine(FlashRoutine());
    }

    public IEnumerator FlashRoutine()
    {
        
        // Swap to the flashMaterial.
        spriteRenderer.material = flashMaterial;

        // Pause the execution of this function for "duration" seconds.
        yield return new WaitForSeconds(duration);

        // After the pause, swap back to the original material.
        spriteRenderer.material = originalMaterial;

        // Pause the execution of this function for "duration" seconds.
        yield return new WaitForSeconds(duration);

        // Swap to the flashMaterial.
        spriteRenderer.material = flashMaterial;

        // Pause the execution of this function for "duration" seconds.
        yield return new WaitForSeconds(duration);

        // After the pause, swap back to the original material.
        spriteRenderer.material = originalMaterial;

        // Set the routine to null, signaling that it's finished.
        flashRoutine = null;

        if (boxCollider != null)
        {
            boxCollider.enabled = false;
            
        }

        ChangeSprite();
        StartCoroutine(Reappearing());
    }


}
