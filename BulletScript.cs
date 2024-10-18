using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    public float lifetime = 2.0f;
    public EnemyHealth enemy;
    public float speed;
    private Rigidbody2D rb;
    [SerializeField] float damage = 20f;

    void Start()
    {
        Physics2D.IgnoreLayerCollision(7, 8);
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = transform.right * speed;
    }

    void Update()
    {
        lifetime -= Time.deltaTime;

        if (lifetime <= 0f)
        {
            Destroy(this.gameObject);
        }
    }

    void OnBecameInvisible()
    {
        Destroy(this.gameObject);
    }


    private void OnTriggerEnter2D(Collider2D other)
    {

        if (other.tag == "Enemy")
        {
            
            other.GetComponent<EnemyHealth>().TakeDamage(damage);
            Destroy(this.gameObject);
        }

        Destroy(this.gameObject);

    }


}

