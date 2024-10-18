using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] float health, maxHealth = 100f;
    

    private void Start()
    {
        health = maxHealth;
    }

    public void TakeDamage (float damage)
    {
        health -= damage;

        if (health <= 0f)
        {
            Die();
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.transform.tag == "RollHitBox")
        {
            Die();
        }
    }

    public void Die()
    {
        //Add Animation Here
        Destroy(this.gameObject);
    }


}
