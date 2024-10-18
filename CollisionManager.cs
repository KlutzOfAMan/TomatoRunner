using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionManager : MonoBehaviour
{
    public Collections coll;
    public Health hs;
    public float force;

    public GameObject playerBody;
    public Rigidbody2D rb;
    public Animator animator;

    public void Start()
    {
        rb = playerBody.GetComponent<Rigidbody2D>();
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.transform.tag == "Deathzone")
        {
            rb.velocity = new Vector2(rb.velocity.x, force);

            if (!hs.invincibility)
            {
                hs.GotHurt();
            }
        }

        if (other.CompareTag("Enemy"))
        {
            if (!hs.invincibility)
            {
                hs.GotHurt();
            }
            
        }

        if (other.CompareTag("Hazard"))
        {
            if (!hs.invincibility)
            {
                hs.GotHurt();
            }

        }

    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.transform.tag == "Coins")
        {
            FindObjectOfType<AudioManager>().Play("CoinSFX");
            Destroy(other.gameObject);
            coll.CoinCounter++;

        }
    }



}
