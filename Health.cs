using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private Material originalMaterial; // The material that was in use, when the script started.
    private Coroutine flashRoutine; // The currently running coroutine.


    [Tooltip("Material to switch to during the flash.")]
    [SerializeField] private Material flashMaterial;

    [Tooltip("Duration of the flash.")]
    [SerializeField] private float duration;
    public float invincibilityDuration = 4f;
    public bool invincibility;

    public int numOfHearts;
    public int health;

    public Image[] hearts;
    public Sprite fullHearts;

    public GameManager gm;
    public GameObject playerBody;


    void Start()
    {
        spriteRenderer = playerBody.GetComponent<SpriteRenderer>();
        originalMaterial = spriteRenderer.material;
        invincibility = false;
    }


    void Update()
    {
        if (health > numOfHearts)
        {
            health = numOfHearts;
        }



        for (int i = 0; i < hearts.Length; i++)
        {

            if (i < numOfHearts)
            {
                hearts[i].enabled = true;
            }
            else
            {
                hearts[i].enabled = false;
            }
        }

    }

    public void GotHurt()
    {
       
        Flash();
        FindObjectOfType<AudioManager>().Play("PainSFX");
        StartCoroutine(InvincibilityFrames());
        numOfHearts--;
     
        if (numOfHearts <= 0)
        {
            //gm.OnDie();
            Debug.Log("YOU'RE DEAD");
            gm.OnDie();

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
        
    }

    public IEnumerator InvincibilityFrames()
    {
        invincibility = true;
        yield return new WaitForSeconds(invincibilityDuration);
        invincibility = false;
    }
}
